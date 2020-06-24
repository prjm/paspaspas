using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Hidden;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     pointer type definition
    /// </summary>
    internal class PointerType : TypeDefinitionBase, IPointerType {

        /// <summary>
        ///     create a new pointer type definition
        /// </summary>
        public PointerType(IUnitType definingType, ITypeDefinition baseType, string longTypeName) : base(definingType) {
            Name = longTypeName;
            BaseTypeDefinition = baseType;
        }

        /// <summary>
        ///     type size
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.SystemUnit.NativeIntType.TypeSizeInBytes;

        /// <summary>
        ///     long type name
        /// </summary>
        public override string Name { get; }

        /// <summary>
        ///     base type definition
        /// </summary>
        public ITypeDefinition BaseTypeDefinition { get; }

        /// <summary>
        ///     short type name
        /// </summary>
        public override string MangledName {
            get {
                if (BaseTypeDefinition is NilType)
                    return KnownNames.PV;
                else
                    return string.Concat(KnownNames.P, BaseTypeDefinition.MangledName);
            }
        }

        /// <summary>
        ///         pointer type
        /// </summary>
        public override BaseType BaseType
            => BaseType.Pointer;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(ITypeDefinition? other)
            => KnownNames.SameIdentifier(Name, other?.Name) &&
               other is IPointerType p && p.BaseTypeDefinition.Equals(BaseTypeDefinition);
    }
}
