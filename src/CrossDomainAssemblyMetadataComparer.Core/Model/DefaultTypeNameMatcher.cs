using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    internal sealed class DefaultTypeNameMatcher : ITypeNameMatcher
    {
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

            CandidateTypes = candidateTypes;
        }

        public ICollection<Type> CandidateTypes
        {
            get;
        }

        public TypeMatch FindMatchingType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            throw new NotImplementedException();
        }
    }
}