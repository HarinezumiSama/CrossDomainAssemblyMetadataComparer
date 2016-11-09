using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public sealed class MetadataComparisonResult
    {
        internal MetadataComparisonResult([NotNull] ICollection<EnumComparisonResult> enumComparisonResults)
        {
            if (enumComparisonResults == null)
            {
                throw new ArgumentNullException(nameof(enumComparisonResults));
            }

            if (enumComparisonResults.Any(item => item == null))
            {
                throw new ArgumentException(@"The collection contains a null element.", nameof(enumComparisonResults));
            }

            EnumComparisonResults = enumComparisonResults.ToArray().AsReadOnly();
        }

        [NotNull]
        public ReadOnlyCollection<EnumComparisonResult> EnumComparisonResults
        {
            get;
        }
    }
}