﻿using System.Collections.Generic;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     array type definition
    /// </summary>
    public class ArrayType : StructuredTypeBase {

        /// <summary>
        ///     create a new array type
        /// </summary>
        /// <param name="withId"></param>
        public ArrayType(int withId) : base(withId) {
        }

        /// <summary>
        ///     arary index types
        /// </summary>
        public IList<ITypeDefinition> IndexTypes { get; }
            = new List<ITypeDefinition>();

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.ArrayType;

        /// <summary>
        ///     base type id
        /// </summary>
        public int BaseTypeId { get; set; }
            = TypeIds.ErrorType;

        /// <summary>
        ///     base type id
        /// </summary>
        public ITypeDefinition BaseType
            => TypeRegistry.GetTypeByIdOrUndefinedType(BaseTypeId);

        /// <summary>
        ///     <c>true</c> if packed array
        /// </summary>
        public bool Packed { get; set; }
            = false;

        /// <summary>
        ///     check if the type can be assigned from another type
        /// </summary>
        /// <param name="otherType"></param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind == CommonTypeKind.ArrayType && otherType is ArrayType array) {
                var isPackedString = (BaseType.TypeKind.IsChar() && array.BaseType.TypeKind.IsChar()) && (Packed && array.Packed);
                return isPackedString;
            }

            return base.CanBeAssignedFrom(otherType);
        }
    }
}
