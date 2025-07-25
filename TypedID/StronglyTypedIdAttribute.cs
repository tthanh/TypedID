using System;

namespace TypedID
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class TypedIdAttribute : Attribute
    {
        public TypedIdAttribute(Type backingType)
        {
            BackingType = backingType;
        }

        public Type BackingType { get; }
    }
}
