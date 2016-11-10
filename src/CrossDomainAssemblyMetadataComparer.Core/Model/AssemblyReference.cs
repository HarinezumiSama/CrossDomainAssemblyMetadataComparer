using System;
using System.Reflection;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public abstract class AssemblyReference
    {
        [NotNull]
        public abstract string AsString
        {
            get;
        }

        public sealed override string ToString() => $@"{{ {GetType().GetQualifiedName()}: {AsString} }}";

        [NotNull]
        public Assembly Load()
        {
            Assembly assembly;
            try
            {
                assembly = LoadAssembly();
            }
            catch (Exception ex)
                when (!ex.IsFatal())
            {
                throw new InvalidOperationException($@"Error has occurred resolving {AsString}.", ex);
            }

            if (assembly == null)
            {
                throw new InvalidOperationException($@"{AsString} was resolved to null.");
            }

            return assembly;
        }

        [NotNull]
        protected abstract Assembly LoadAssembly();
    }
}