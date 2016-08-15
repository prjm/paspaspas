﻿namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     Token kinds
    /// </summary>
    public static class TokenKind {


        /// <summary>
        ///     undefined tokens
        /// </summary>
        public const int Undefined = 1;

        /// <summary>
        ///     end-of-input
        /// </summary>
        public const int Eof = 2;

        /// <summary>
        ///     preprocessor token
        /// </summary>
        public const int Preprocessor = 5;

        /// <summary>
        ///     comment
        /// </summary>
        public const int Comment = 6;

        /// <summary>
        ///     token for <c>,</c>
        /// </summary>
        public const int Comma = 10;

        /// <summary>
        ///     token for <c>.</c>
        /// </summary>
        public const int Dot = 11;

        /// <summary>
        ///     token for <c>(</c>
        /// </summary>
        public const int OpenParen = 12;

        /// <summary>
        ///     token for <c>)</c>
        /// </summary>
        public const int CloseParen = 13;

        /// <summary>
        ///     token for <c>;</c>
        /// </summary>
        public const int Semicolon = 14;

        /// <summary>
        ///     token for <c>=</c>
        /// </summary>
        public const int EqualsSign = 15;

        /// <summary>
        ///     token for <c>[</c>
        /// </summary>
        public const int OpenBraces = 16;

        /// <summary>
        ///     token for <c>]</c>
        /// </summary>
        public const int CloseBraces = 17;

        /// <summary>
        ///     token for <c>:</c>
        /// </summary>
        public const int Colon = 18;

        /// <summary>
        ///     token for <c>^</c>
        /// </summary>
        public const int Circumflex = 19;

        /// <summary>
        ///     token for <c>&lt;</c>
        /// </summary>
        public const int AngleBracketsOpen = PascalToken.LessThen;

        /// <summary>
        ///     token for <c>&gt;</c>
        /// </summary>
        public const int AngleBracketsClose = PascalToken.GreaterThen;

        /// <summary>
        ///     toke for <c>..</c>
        /// </summary>
        public const int DotDot = 22;

        /// <summary>
        ///     token for <c>+</c>
        /// </summary>
        public const int Plus = 23;

        /// <summary>
        ///     token for <c>-</c>
        /// </summary>
        public const int Minus = 24;

        /// <summary>
        ///     token for <c>*</c>
        /// </summary>
        public const int Times = 25;

        /// <summary>
        ///     token for <c>/</c>
        /// </summary>
        public const int Slash = 26;


        /// <summary>
        ///     token for <c>@</c>
        /// </summary>
        public const int At = 27;

        /// <summary>
        ///     token for <c>program</c>
        /// </summary>
        public const int Program = 200;

        /// <summary>
        ///     token for <c>unit</c>
        /// </summary>
        public const int Unit = 201;

        /// <summary>
        ///     token for <c>package</c>
        /// </summary>
        public const int Package = 202;

        /// <summary>
        ///     token for <c>library</c>
        /// </summary>
        public const int Library = 203;

        /// <summary>
        ///     token for <c>uses</c>
        /// </summary>
        public const int Uses = 204;

        /// <summary>
        ///     token for <c>in</c>
        /// </summary>
        public const int In = 205;

        /// <summary>
        ///     token for <c>const</c>
        /// </summary>
        public const int Const = 206;

        /// <summary>
        ///     token for <c>pointer</c>
        /// </summary>
        public const int Pointer = 207;

        /// <summary>
        ///     token for <c>array</c>
        /// </summary>
        public const int Array = 208;

        /// <summary>
        ///     token for <c>of</c>
        /// </summary>
        public const int Of = 209;

        /// <summary>
        ///     token for <c>type</c>
        /// </summary>
        public const int TypeKeyword = 210;

        /// <summary>
        ///     token for <c>packed</c>
        /// </summary>
        public const int Packed = 211;

    }
}
