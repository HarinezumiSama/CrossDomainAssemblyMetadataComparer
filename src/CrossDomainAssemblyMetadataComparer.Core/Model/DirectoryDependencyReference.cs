using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public sealed class DirectoryDependencyReference : DependencyReference
    {
        private static readonly string[] DefaultAssemblyPatterns = { "*.dll", "*.exe" };

        private static readonly StringComparer FilePathEqualityComparer = StringComparer.OrdinalIgnoreCase;

        public DirectoryDependencyReference([NotNull] DirectoryInfo directoryInfo, bool recursive)
        {
            if (directoryInfo == null)
            {
                throw new ArgumentNullException(nameof(directoryInfo));
            }

            DirectoryInfo = directoryInfo;
            Recursive = recursive;
        }

        public DirectoryDependencyReference([NotNull] string directoryPath, bool recursive)
        {
            if (string.IsNullOrWhiteSpace(directoryPath))
            {
                throw new ArgumentException(
                    @"The value can be neither empty nor whitespace-only string nor null.",
                    nameof(directoryPath));
            }

            DirectoryInfo = new DirectoryInfo(directoryPath);
            Recursive = recursive;
        }

        public override string AsString
            => $@"{nameof(DirectoryPath)} = {DirectoryPath.ToUIString()}, {nameof(Recursive)} = {Recursive}";

        [NotNull]
        public DirectoryInfo DirectoryInfo
        {
            get;
        }

        public bool Recursive
        {
            get;
        }

        [NotNull]
        public string DirectoryPath => DirectoryInfo.FullName;

        protected override string[] ResolveFilePaths()
        {
            var searchOption = Recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;

            var allFileInfos = new List<FileInfo>();
            foreach (var pattern in DefaultAssemblyPatterns)
            {
                var fileInfos = DirectoryInfo.GetFiles(pattern, searchOption);
                allFileInfos.AddRange(fileInfos);
            }

            var result = allFileInfos.Select(obj => obj.FullName).Distinct(FilePathEqualityComparer).ToArray();
            return result;
        }
    }
}