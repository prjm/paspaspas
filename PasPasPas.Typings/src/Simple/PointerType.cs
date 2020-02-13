using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     pointer type definition
    /// </summary>
    public class PointerType : TypeDefinitionBase, IFixedSizeType {

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
                if (BaseTypeDefinition == default)
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
    }
}
