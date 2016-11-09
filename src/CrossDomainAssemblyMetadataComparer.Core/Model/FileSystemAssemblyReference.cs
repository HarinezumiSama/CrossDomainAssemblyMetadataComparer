using System;
using System.IO;
using System.Reflection;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public sealed class FileSystemAssemblyReference : AssemblyReference
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="FileSystemAssemblyReference"/> class
        ///     using the specified <see cref="FileInfo"/>.
        /// </summary>
        public FileSystemAssemblyReference([NotNull] FileInfo fileInfo)
        {
            if (fileInfo == null)
            {
                throw new ArgumentNullException(nameof(fileInfo));
            }

            FileInfo = fileInfo;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="FileSystemAssemblyReference"/> class
        ///     using the specified file path.
        /// </summary>
        public FileSystemAssemblyReference([NotNull] string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                throw new ArgumentException(
                    @"The value can be neither empty nor whitespace-only string nor null.",
                    nameof(filePath));
            }

            FileInfo = new FileInfo(filePath);
        }

        [NotNull]
        public FileInfo FileInfo
        {
            get;
        }

        public override string AsString => $@"{{ {GetType().GetQualifiedName()}: Path = ""{FileInfo.FullName}"" }}";

        protected override Assembly LoadAssembly() => Assembly.LoadFrom(FileInfo.FullName);
    }
}