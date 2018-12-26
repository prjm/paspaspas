﻿using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Simple {
    /// <summary>
    ///     string type definition
    /// </summary>
    public class ShortStringType : StringTypeBase, IShortStringType {

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="size">string size</param>
        public ShortStringType(int withId, ITypeReference size) : base(withId)
            => Size = size;

        /// <summary>
        ///     unicode string type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.ShortStringType;

        /// <summary>
        ///     string size
        /// </summary>
        public ITypeReference Size { get; }

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes {
            get {
                if (Size.IsIntegralValue(out var integerValue))
                    return 1u + (uint)integerValue.UnsignedValue;

                return 0;
            }
        }

        /// <summary>
        ///     format this type definition as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => $"ShortString[{Size}]";

    }


}
