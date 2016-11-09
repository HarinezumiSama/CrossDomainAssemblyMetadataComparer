using System;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public sealed class EnumValueComparisonResult
    {
        internal EnumValueComparisonResult(
            [NotNull] object value,
            [CanBeNull] string examineeName,
            [CanBeNull] string comparandName,
            EnumValueMatchKind matchKind)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            Value = value;
            ExamineeName = examineeName;
            ComparandName = comparandName;
            MatchKind = matchKind;
            OverallMatchKind = matchKind.ToOverallMatchKind();
        }

        public object Value
        {
            get;
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

        public OverallMatchKind OverallMatchKind
        {
            get;
        }

        public override string ToString()
            => $@"{GetType().GetQualifiedName()}: {nameof(OverallMatchKind)} = {OverallMatchKind}, {
                nameof(Value)} = {Value}, {nameof(ExamineeName)} = {ExamineeName.ToUIString()}, {
                nameof(ComparandName)} = {ComparandName.ToUIString()}, {nameof(MatchKind)} = {MatchKind}";
    }
}