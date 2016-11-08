using System;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core
{
    public sealed class MetadataComparer
    {
        public MetadataComparer(
            [NotNull] AssemblyReference examinee,
            [NotNull] AssemblyReference comparand,
            [NotNull] MetadataComparisonParameters parameters)
        {
            if (examinee == null)
            {
                throw new ArgumentNullException(nameof(examinee));
            }

            if (comparand == null)
            {
                throw new ArgumentNullException(nameof(comparand));
            }

            if (parameters == null)
            {
                throw new ArgumentNullException(nameof(parameters));
            }

            Examinee = examinee;
            Comparand = comparand;
            Parameters = parameters;
        }

        public MetadataComparer([NotNull] AssemblyReference examinee, [NotNull] AssemblyReference comparand)
            : this(examinee, comparand, MetadataComparisonParameters.Default)
        {
            // Nothing to do
        }

        public AssemblyReference Examinee
        {
            get;
        }

        public AssemblyReference Comparand
        {
            get;
        }

        public MetadataComparisonParameters Parameters
        {
            get;
        }

        public MetadataComparisonResult Compare()
        {
            throw new NotImplementedException();
        }
    }
}