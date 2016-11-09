using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public interface ITypeNameMatcher
    {
        [NotNull]
        TypeMatch FindMatchingType([NotNull] Type type);
    }
}