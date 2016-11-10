using System;
using System.Linq;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public abstract class DependencyReference
    {
        [NotNull]
        public abstract string AsString
        {
            get;
        }

        public sealed override string ToString() => $@"{{ {GetType().GetQualifiedName()}: {AsString} }}";

        [NotNull]
        public string[] GetFilePaths()
        {
            string[] result;
            try
            {
                result = ResolveFilePaths();
            }
            catch (Exception ex)
                when (!ex.IsFatal())
            {
                throw new InvalidOperationException($@"Error has occurred getting file paths for {AsString}.", ex);
            }

            if (result == null)
            {
                throw new InvalidOperationException($@"The list of file paths for {AsString} was resolved to null.");
            }

            if (result.Any(item => item == null))
            {
                throw new InvalidOperationException($@"The list of file paths for {AsString} contains a null item.");
            }

            return result;
        }

        [NotNull]
        public AssemblyReference[] GetAssemblyReferences()
            => GetFilePaths().Select(path => new FileSystemAssemblyReference(path)).ToArray<AssemblyReference>();

        [NotNull]
        protected abstract string[] ResolveFilePaths();
    }
}