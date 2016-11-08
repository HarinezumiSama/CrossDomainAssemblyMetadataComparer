using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    [NotNull]
    public delegate ITypeNameMatcher TypeNameMatcherCreator([NotNull] ICollection<Type> candidateTypes);
}