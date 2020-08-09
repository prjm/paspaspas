using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     static array type
    /// </summary>
    internal class StaticArrayType : ArrayType {

        /// <summary>
        ///     create a new static array type
        /// </summary>
        /// <param name="definingUnit"></param>
        /// <param name="baseTypeDefinition"></param>
        /// <param name="indexType"></param>
        public StaticArrayType(IUnitType definingUnit, ITypeDefinition indexType, ITypeDefinition baseTypeDefinition) :
            base(definingUnit, indexType, baseTypeDefinition) { }


        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes {
            get {
                var index = IndexType as IOrdinalType;
                var lowerBound = index?.LowestElement as IOrdinalValue;
                var upperBound = index?.HighestElement as IOrdinalValue;

                if (lowerBound == default || upperBound == default)
                    return 0;

                var l = lowerBound.GetOrdinalValue(TypeRegistry);
                var h = upperBound.GetOrdinalValue(TypeRegistry);
                var size = TypeRegistry.Runtime.Integers.Subtract(h, l);
                var value = size as IIntegerValue ?? throw new InvalidOperationException();
                return (uint)(BaseTypeDefinition.TypeSizeInBytes * value.SignedValue);
            }
        }

        /// <summary>
        ///     array type kind
        /// </summary>
        public override ArrayTypeKind Kind
            => ArrayTypeKind.StaticArray;


    }
}
