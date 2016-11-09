using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    internal sealed class DefaultTypeNameMatcher : ITypeNameMatcher
    {
        private readonly Dictionary<string, Type[]> _strictLookupMap;
        private readonly Dictionary<string, Type[]> _ignoreCaseLookupMap;

        public DefaultTypeNameMatcher([NotNull] ICollection<Type> candidateTypes)
        {
            if (candidateTypes == null)
            {
                throw new ArgumentNullException(nameof(candidateTypes));
            }

            if (candidateTypes.Any(item => item == null))
            {
                throw new ArgumentException(@"The collection contains a null element.", nameof(candidateTypes));
            }

            CandidateTypes = candidateTypes.ToArray().AsReadOnly();

            _strictLookupMap = CandidateTypes
                .GroupBy(GetTypeKey, StringComparer.Ordinal)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.ToArray());

            _ignoreCaseLookupMap = CandidateTypes
                .GroupBy(GetTypeKey, StringComparer.OrdinalIgnoreCase)
                .ToDictionary(grouping => grouping.Key, grouping => grouping.ToArray());

            //// TODO [vmaklai] DefaultTypeNameMatcher: UserDefined lookup(s)
        }

        public ReadOnlyCollection<Type> CandidateTypes
        {
            get;
        }

        public TypeMatch FindMatchingType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var key = GetTypeKey(type);

            var strictTypes = _strictLookupMap.GetValueOrDefault(key).AvoidNull();

            var ignoreCaseTypes = _ignoreCaseLookupMap.GetValueOrDefault(key).AvoidNull().ToHashSet();
            ignoreCaseTypes.ExceptWith(strictTypes);

            var totalFoundCount = strictTypes.Length + ignoreCaseTypes.Count;
            switch (totalFoundCount)
            {
                case 0:
                    return TypeMatch.None;

                case 1:
                    return strictTypes.Length == 1
                        ? new TypeMatch(TypeMatchKind.Strict, strictTypes)
                        : new TypeMatch(TypeMatchKind.CaseInsensitive, ignoreCaseTypes);

                default:
                    return new TypeMatch(TypeMatchKind.Ambiguous, strictTypes.Concat(ignoreCaseTypes).ToArray());
            }
        }

        private static string GetTypeKey([NotNull] Type type) => type.GetQualifiedName();
    }
}