using System;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public sealed class MetadataComparisonResult
    {
        internal MetadataComparisonResult([NotNull] EnumComparisonResult enumComparisonResult)
        {
            if (enumComparisonResult == null)
            {
                throw new ArgumentNullException(nameof(enumComparisonResult));
            }

            EnumComparisonResult = enumComparisonResult;
        }

        [NotNull]
        public EnumComparisonResult EnumComparisonResult
        {
            get;
        }
    }
}