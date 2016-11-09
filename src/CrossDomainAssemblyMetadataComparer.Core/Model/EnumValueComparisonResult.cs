using System;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public sealed class EnumValueComparisonResult
    {
        internal EnumValueComparisonResult([CanBeNull] string examineeName, [CanBeNull] string comparandName)
        {
            ExamineeName = examineeName;
            ComparandName = comparandName;
        }

        public string ExamineeName
        {
            get;
        }

        public string ComparandName
        {
            get;
        }
    }
}