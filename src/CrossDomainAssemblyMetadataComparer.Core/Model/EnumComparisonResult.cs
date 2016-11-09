using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public sealed class EnumComparisonResult
    {
        internal EnumComparisonResult(
            [NotNull] TypeMatch typeMatch,
            [NotNull] ICollection<EnumValueComparisonResult> valueComparisonResults)
        {
            if (typeMatch == null)
            {
                throw new ArgumentNullException(nameof(typeMatch));
            }

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

            TypeMatch = typeMatch;
            ValueComparisonResults = valueComparisonResults.ToArray().AsReadOnly();
        }

        public TypeMatch TypeMatch
        {
            get;
        }

        public ReadOnlyCollection<EnumValueComparisonResult> ValueComparisonResults
        {
            get;
        }
    }
}