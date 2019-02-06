﻿using System.Text;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Runtime.Values.CharValues;

namespace PasPasPas.Runtime.Values.StringValues {

    /// <summary>
    ///     ANSI string values
    /// </summary>
    public class AnsiStringValue : StringValueBase, IStringValue {

        private readonly string data;

        /// <summary>
        ///     create a new ANSI string value
        /// </summary>
        /// <param name="text"></param>
        /// <param name="typeId">type id</param>
        public AnsiStringValue(int typeId, string text) : base(typeId) {
            var buffer = new StringBuilder(text.Length);
            for (var i = 0; i < text.Length; i++) {
                var c = text[i];
                buffer.Append(c <= 255 ? c : '?');
            }
            data = buffer.ToString();
        }

        /// <summary>
        ///     string data
        /// </summary>
        public override string AsUnicodeString
            => data;

        /// <summary>
        ///     ANSI string type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.LongStringType;

        /// <summary>
        ///     get the content of this string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => InternalTypeFormat;

        /// <summary>
        ///    get a char at
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public override ITypeReference CharAt(int index) {
            if (index < 0 || index >= data.Length)
                return new SpecialValue(SpecialConstantKind.InvalidChar);

            return new AnsiCharValue(KnownTypeIds.AnsiCharType, unchecked((byte)data[index]));
        }

        /// <summary>
        ///     convert this value to in internal string format
        /// </summary>
        public override string InternalTypeFormat
            => $"'{data}'";

        /// <summary>
        ///     number of char elements
        /// </summary>
        public override int NumberOfCharElements
            => data.Length;
    }
}