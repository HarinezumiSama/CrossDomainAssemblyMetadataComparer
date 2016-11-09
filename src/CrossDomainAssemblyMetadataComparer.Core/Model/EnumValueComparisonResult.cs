using System;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public sealed class EnumValueComparisonResult
    {
        internal EnumValueComparisonResult(
            [CanBeNull] string examineeName,
            [CanBeNull] string comparandName,
            EnumValueMatchKind matchKind)
        {
            ExamineeName = examineeName;
            ComparandName = comparandName;
            MatchKind = matchKind;
        }

        public string ExamineeName
        {
            get;
        }

        public string ComparandName
        {
            get;
        }

        public EnumValueMatchKind MatchKind
        {
            get;
        }

        public override string ToString()
            => $@"{GetType().GetQualifiedName()}: {nameof(MatchKind)} = {MatchKind}, {nameof(ExamineeName)} = {
                ExamineeName.ToUIString()}, {nameof(ComparandName)} = {ComparandName.ToUIString()}";
    }
}