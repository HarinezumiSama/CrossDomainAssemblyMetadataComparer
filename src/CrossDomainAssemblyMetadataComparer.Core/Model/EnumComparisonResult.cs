using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public sealed class EnumComparisonResult : TypeComparisonResult
    {
        internal EnumComparisonResult(
            [NotNull] Type examineeType,
            [NotNull] TypeMatch typeMatch,
            [NotNull] ICollection<EnumValueComparisonResult> valueComparisonResults)
            : base(examineeType, typeMatch)
        {
            if (valueComparisonResults == null)
            {
                throw new ArgumentNullException(nameof(valueComparisonResults));
            }

            if (valueComparisonResults.Any(item => item == null))
            {
                throw new ArgumentException(
                    @"The collection contains a null element.",
                    nameof(valueComparisonResults));
            }

            ValueComparisonResults = valueComparisonResults.ToArray().AsReadOnly();
        }

        [NotNull]
        public ReadOnlyCollection<EnumValueComparisonResult> ValueComparisonResults
        {
            get;
        }
    }
}