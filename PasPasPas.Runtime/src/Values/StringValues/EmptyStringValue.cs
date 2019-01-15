﻿using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.StringValues {

    /// <summary>
    ///     constant empty string value
    /// </summary>
    public class EmptyStringValue : StringValueBase {

        /// <summary>
        ///     short string
        /// </summary>
        public override int TypeId
            => KnownTypeIds.ShortStringType;

        /// <summary>
        ///     get the empty string
        /// </summary>
        public override string AsUnicodeString
            => string.Empty;

        /// <summary>
        ///     type kind <c>short string</c>
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.ShortStringType;

        /// <summary>
        ///     format this string to a internal format
        /// </summary>
        public override string InternalTypeFormat
            => "''";

        /// <summary>
        ///     number of chars
        /// </summary>
        public override int NumberOfCharElements
            => 0;

        /// <summary>
        ///     char at index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override ITypeReference CharAt(int index)
            => new SpecialValue(SpecialConstantKind.InvalidChar);
    }
}