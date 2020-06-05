#nullable disable
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     dynamic array type
    /// </summary>
    public class DynamicArrayType : ArrayType {

        /// <summary>
        ///     create a new dynamic array type
        /// </summary>
        /// <param name="definingUnit"></param>
        /// <param name="typeName"></param>
        public DynamicArrayType(IUnitType definingUnit, string typeName) : base(definingUnit, definingUnit.TypeRegistry.SystemUnit.IntegerType)
            => Name = typeName;

        /// <summary>
        ///     type name
        /// </summary>
        public override string Name { get; }

        /// <summary>
        ///     dynamic array
        /// </summary>
        public override ArrayTypeKind Kind
            => ArrayTypeKind.DynamicArray;

        /// <summary>
        ///     type size
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.GetPointerSize();

    }
}
