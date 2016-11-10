using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using CrossDomainAssemblyMetadataComparer.Core.Model;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core
{
    public sealed class AssemblyReferenceResolver : IDisposable
    {
        private static readonly StringComparer AssemblyNameComparer = StringComparer.OrdinalIgnoreCase;

        private readonly AppDomain _domain;
        private readonly Dictionary<string, AssemblyReferenceInfo[]> _nameMap;

        private AssemblyReferenceResolver([NotNull] ICollection<AssemblyReferenceInfo> assemblyReferenceInfos)
        {
            if (assemblyReferenceInfos == null)
            {
                throw new ArgumentNullException(nameof(assemblyReferenceInfos));
            }

            _domain = AppDomain.CurrentDomain;

            _nameMap = assemblyReferenceInfos
                .GroupBy(obj => obj.AssemblyName.Name, AssemblyNameComparer)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.ToArray(), AssemblyNameComparer);

            _domain.AssemblyResolve += Domain_AssemblyResolve;
        }

        public static AssemblyReferenceResolver Create(
            [NotNull] ICollection<DependencyReference> dependencyReferences)
        {
            if (dependencyReferences == null)
            {
                throw new ArgumentNullException(nameof(dependencyReferences));
            }

            if (dependencyReferences.Any(item => item == null))
            {
                throw new ArgumentException(@"The collection contains a null element.", nameof(dependencyReferences));
            }

            var filePaths = dependencyReferences
                .SelectMany(reference => reference.GetFilePaths())
                .ToArray();

            AssemblyReferenceInfo[] assemblyReferenceInfos;

            var domainName = $@"{nameof(AssemblyReferenceResolver)}-Probing-{Guid.NewGuid():N}";
            var domain = AppDomain.CreateDomain(domainName, null, AppDomain.CurrentDomain.SetupInformation);
            try
            {
                var type = typeof(ResolverHelper);

                var helper = (ResolverHelper)domain.CreateInstanceAndUnwrap(
                    type.Assembly.FullName,
                    type.FullName,
                    false,
                    BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic,
                    null,
                    null,
                    null,
                    null);

                assemblyReferenceInfos = helper.CreateCache(filePaths);
            }
            finally
            {
                AppDomain.Unload(domain);
            }

            return new AssemblyReferenceResolver(assemblyReferenceInfos);
        }

        public void Dispose() => _domain.AssemblyResolve -= Domain_AssemblyResolve;

        private Assembly Domain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            var prefix = $@"[{nameof(AssemblyReferenceResolver)}.{nameof(Domain_AssemblyResolve)}]";

            Trace.TraceInformation($@"{prefix} Resolving '{args.Name}'...");

            var assemblyName = new AssemblyName(args.Name);

            var infos = _nameMap.GetValueOrDefault(assemblyName.Name);
            if (infos.Length != 0 && assemblyName.Version != null)
            {
                infos = infos.Where(obj => obj.AssemblyName.Version == assemblyName.Version).ToArray();
            }

            if (infos.Length == 0)
            {
                Trace.TraceWarning($@"{prefix} No assembly is found for '{assemblyName}'.");
                return null;
            }

            if (infos.Length > 1)
            {
                Trace.TraceWarning($@"{prefix} Multiple matching assemblies are found for '{assemblyName}'.");
            }

            var info = infos.First();
            Trace.TraceInformation($@"{prefix} Assembly is found for '{assemblyName}': {info.FilePath.ToUIString()}.");

            var assembly = Assembly.LoadFrom(info.FilePath);
            return assembly;
        }

        [Serializable]
        private sealed class AssemblyReferenceInfo
        {
            public string FilePath
            {
                get;
                set;
            }

            public AssemblyName AssemblyName
            {
                get;
                set;
            }

            public override string ToString() => $@"[{AssemblyName}] → {FilePath.ToUIString()}";
        }

        private sealed class ResolverHelper : MarshalByRefObject
        {
            public override object InitializeLifetimeService() => null;

            public AssemblyReferenceInfo[] CreateCache(string[] filePaths)
            {
                WriteDebugInfo();

                if (filePaths == null)
                {
                    throw new ArgumentNullException(nameof(filePaths));
                }

                var assemblyReferenceInfos = new List<AssemblyReferenceInfo>(filePaths.Length);
                foreach (var filePath in filePaths)
                {
                    try
                    {
                        var assemblyName = AssemblyName.GetAssemblyName(filePath).EnsureNotNull();

                        var info = new AssemblyReferenceInfo
                        {
                            FilePath = filePath,
                            AssemblyName = assemblyName
                        };

                        assemblyReferenceInfos.Add(info);
                    }
                    catch (Exception ex)
                        when (!ex.IsFatal())
                    {
                        Trace.TraceWarning($@"Error loading the file {filePath.ToUIString()} as assembly: {ex}");
                    }
                }

                return assemblyReferenceInfos.ToArray();
            }

            [Conditional("DEBUG")]
            private void WriteDebugInfo()
            {
                var domain = AppDomain.CurrentDomain;

                Trace.TraceInformation(
                    $@"[{GetType().GetQualifiedName()}] {nameof(AppDomain.CurrentDomain)}: {nameof(domain.Id)} = {
                        domain.Id}, {nameof(domain.FriendlyName)} = {domain.FriendlyName}");
            }
        }
    }
}