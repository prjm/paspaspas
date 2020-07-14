using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     pointer type definition
    /// </summary>
    internal class PointerType : CompilerDefinedType, IPointerType {

        /// <summary>
        ///     create a new pointer type definition
        /// </summary>
        /// <param name="baseType">base type</param>
        /// <param name="definingType">defining unit</param>
        /// <param name="name">pointer name</param>
        internal PointerType(IUnitType definingType, string name, IMangledNameTypeSymbol? baseType) : base(definingType) {
            Name = name;
            BaseNameSymbol = baseType;
        }

        /// <summary>
        ///     type size
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.SystemUnit.NativeIntType.TypeSizeInBytes;

        /// <summary>
        ///     base type definition
        /// </summary>
        public ITypeDefinition? BaseTypeDefinition
            => BaseNameSymbol?.TypeDefinition;

        /// <summary>
        ///     short type name
        /// </summary>
        public override string MangledName {
            get {
                if (BaseNameSymbol is null)
                    return KnownNames.PV;
                else
                    return string.Concat(KnownNames.P, BaseNameSymbol.MangledName);
            }
        }

        /// <summary>
        ///         pointer type
        /// </summary>
        public override BaseType BaseType
            => BaseType.Pointer;

        public override string Name { get; }

        public IMangledNameTypeSymbol? BaseNameSymbol { get; }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(ITypeDefinition? other)
            => other is IPointerType p &&
            ((BaseNameSymbol is null && p.BaseNameSymbol is null) ||
            (!(BaseNameSymbol is null) && BaseNameSymbol.Equals(p.BaseNameSymbol)));

    }
}
