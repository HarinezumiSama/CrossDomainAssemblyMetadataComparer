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
            BaseOverallMatchKind = typeMatch.Kind.ToOverallMatchKind();
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

        public abstract OverallMatchKind OverallMatchKind
        {
            get;
        }

        public override string ToString()
            => $@"{GetType().GetQualifiedName()}: {nameof(OverallMatchKind)} = {OverallMatchKind}, {
                nameof(ExamineeType)} = {ExamineeType.GetFullName().ToUIString()}";

        protected OverallMatchKind BaseOverallMatchKind
        {
            get;
        }
    }
}