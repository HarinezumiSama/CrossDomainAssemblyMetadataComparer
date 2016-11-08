using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public interface ITypeNameMatcher
    {
        [CanBeNull]
        TypeMatch FindMatchingType([NotNull] Type type);
    }
}