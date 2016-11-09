using System;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public abstract class TypeComparisonResult
    {
        internal TypeComparisonResult(
            [NotNull] Type examineeType,
            [NotNull] TypeMatch typeMatch)
        {
            if (examineeType == null)
            {
                throw new ArgumentNullException(nameof(examineeType));
            }

            if (typeMatch == null)
            {
                throw new ArgumentNullException(nameof(typeMatch));
            }

            ExamineeType = examineeType;
            TypeMatch = typeMatch;
        }

        [NotNull]
        public Type ExamineeType
        {
            get;
        }

        [NotNull]
        public TypeMatch TypeMatch
        {
            get;
        }

        public override string ToString()
            => $@"{GetType().GetQualifiedName()}: {nameof(ExamineeType)} = {
                ExamineeType.GetFullName().ToUIString()}, {nameof(TypeMatch)}.{nameof(TypeMatch.Kind)} = {
                TypeMatch.Kind}";
    }
}