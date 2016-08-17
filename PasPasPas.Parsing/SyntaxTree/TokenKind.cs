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


        /// <summary>
        ///     token for <c>platform</c>
        /// </summary>
        public const int Platform = 212;

        /// <summary>
        ///     token for <c>deprecated</c>
        /// </summary>
        public const int Deprecated = 213;

        /// <summary>
        ///     token for <c>experimental</c>
        /// </summary>
        public const int Experimental = 214;

        /// <summary>
        ///     token for <c>set</c>
        /// </summary>
        public const int Set = 215;

        /// <summary>
        ///     token for <c>file</c>
        /// </summary>
        public const int File = 216;

        /// <summary>
        ///     token for <c>class</c>
        /// </summary>
        public const int Class = 217;

        /// <summary>
        ///     token for <c>begin</c>
        /// </summary>
        public const int Begin = 218;

        /// <summary>
        ///     token for <c>end</c>
        /// </summary>
        public const int End = 219;

        /// <summary>
        ///     token for sealed
        /// </summary>
        public const int Sealed = 220;

        /// <summary>
        ///     token for <c>abstract</c>
        /// </summary>
        public const int Abstract = 221;

        /// <summary>
        ///     toyken for <c>strict</c>
        /// </summary>
        public const int Strict = 222;

        /// <summary>
        ///     token for <c>protected</c>
        /// </summary>
        public const int Protected = 223;

        /// <summary>
        ///     token for <c>private</c>
        /// </summary>
        public const int Private = 224;

        /// <summary>
        ///     token for <c>published</c>
        /// </summary>
        public const int Published = 225;

        /// <summary>
        ///     token for <c>automated</c>
        /// </summary>
        public const int Automated = 226;

        /// <summary>
        ///     token for <c>public</c>
        /// </summary>
        public const int Public = 227;

        /// <summary>
        ///     token for <c>constructor</c>
        /// </summary>
        public const int Constructor = 228;

        /// <summary>
        ///     token for <c>destructor</c>
        /// </summary>
        public const int Destructor = 229;

        /// <summary>
        ///     token for <c>procedure</c>
        /// </summary>
        public const int Procedure = 230;

        /// <summary>
        ///     token for <c>function</c>
        /// </summary>
        public const int Function = 231;

        /// <summary>
        ///     token for <c>record</c>
        /// </summary>
        public const int Record = 232;



    }
}
