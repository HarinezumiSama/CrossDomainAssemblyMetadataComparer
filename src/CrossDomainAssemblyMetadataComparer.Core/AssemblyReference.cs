using System.Reflection;

namespace CrossDomainAssemblyMetadataComparer.Core
{
    public abstract class AssemblyReference
    {
        public abstract string AsString
        {
            get;
        }

        public sealed override string ToString() => AsString;

        public Assembly Load() => LoadAssembly();

        protected abstract Assembly LoadAssembly();
    }
}