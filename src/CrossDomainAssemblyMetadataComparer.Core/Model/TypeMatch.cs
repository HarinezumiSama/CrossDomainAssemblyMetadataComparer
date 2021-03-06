﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using JetBrains.Annotations;

namespace CrossDomainAssemblyMetadataComparer.Core.Model
{
    public sealed class TypeMatch
    {
        internal static readonly TypeMatch None = new TypeMatch(TypeMatchKind.None, Type.EmptyTypes);

        private const int MinFoundWhenAmbiguous = 2;

        internal TypeMatch(TypeMatchKind kind, [NotNull] ICollection<Type> foundTypes)
        {
            if (!Enum.IsDefined(typeof(TypeMatchKind), kind))
            {
                throw new InvalidEnumArgumentException(nameof(kind), (int)kind, typeof(TypeMatchKind));
            }

            if (foundTypes == null)
            {
                throw new ArgumentNullException(nameof(foundTypes));
            }

            if (foundTypes.Any(item => item == null))
            {
                throw new ArgumentException(@"The collection contains a null element.", nameof(foundTypes));
            }

            switch (kind)
            {
                case TypeMatchKind.None:
                    if (foundTypes.Count != 0)
                    {
                        throw new ArgumentException(
                            $@"There must be exactly zero types found when '{nameof(kind)}' is <{kind}>.",
                            nameof(foundTypes));
                    }

                    break;

                case TypeMatchKind.Strict:
                case TypeMatchKind.CaseInsensitive:
                case TypeMatchKind.UserDefined:
                    if (foundTypes.Count != 1)
                    {
                        throw new ArgumentException(
                            $@"There must be exactly one type found when '{nameof(kind)}' is <{kind}>.",
                            nameof(foundTypes));
                    }

                    break;

                case TypeMatchKind.Ambiguous:
                    if (foundTypes.Count < MinFoundWhenAmbiguous)
                    {
                        throw new ArgumentException(
                            $@"There must be at least {MinFoundWhenAmbiguous} types found when '{
                                nameof(kind)}' is <{kind}>.",
                            nameof(foundTypes));
                    }

                    break;

                default:
                    throw kind.CreateEnumValueNotImplementedException();
            }

            Kind = kind;
            FoundTypes = foundTypes.ToArray().AsReadOnly();
        }

        public TypeMatchKind Kind
        {
            get;
        }

        public ReadOnlyCollection<Type> FoundTypes
        {
            get;
        }

        public override string ToString()
            => $@"{GetType().GetQualifiedName()}: {nameof(Kind)} = {Kind}";
    }
}