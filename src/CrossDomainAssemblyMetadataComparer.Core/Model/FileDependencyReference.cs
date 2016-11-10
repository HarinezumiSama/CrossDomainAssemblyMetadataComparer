using System;
using System.IO;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public sealed class FileDependencyReference : DependencyReference
    {
        public FileDependencyReference([NotNull] FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException(nameof(fileInfo));
            }

            FileInfo = fileInfo;
        }

        public FileDependencyReference([NotNull] string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException(
                    @"The value can be neither empty nor whitespace-only string nor null.",
                    nameof(filePath));
            }

            FileInfo = new FileInfo(filePath);
        }

        public override string AsString => $@"{nameof(FilePath)} = {FilePath.ToUIString()}";

        [NotNull]
        public FileInfo FileInfo
        {
            get;
        }

        [NotNull]
        public string FilePath => FileInfo.FullName;

        protected override string[] ResolveFilePaths() => FilePath.AsArray();
    }
}