namespace PasPasPas.Parsing.SyntaxTree {

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

        /// <summary>
        ///     token for <c>var</c>
        /// </summary>
        public const int Var = 233;

        /// <summary>
        ///     token for <c>out</c>
        /// </summary>
        public const int Out = 234;

        /// <summary>
        ///     token for <c>resourcestring</c>
        /// </summary>
        public const int Resourcestring = 235;

        /// <summary>
        ///     token for <c>reintroduce</c>
        /// </summary>
        public const int Reintroduce = 236;

        /// <summary>
        ///     token for <c>overload</c>
        /// </summary>
        public const int Overload = 237;

        /// <summary>
        ///     token for <c>message</c>
        /// </summary>
        public const int Message = 238;

        /// <summary>
        ///     token for <c>static</c>
        /// </summary>
        public const int Static = 239;

        /// <summary>
        ///     token for dynamic
        /// </summary>
        public const int Dynamic = 240;

        /// <summary>
        ///     token for override
        /// </summary>
        public const int Override = 241;

        /// <summary>
        ///     token for <c>virtual</c>
        /// </summary>
        public const int Virtual = 242;

        /// <summary>
        ///     token for <c>final</c>
        /// </summary>
        public const int Final = 234;




        /// <summary>
        ///     token for <c>inline</c>
        /// </summary>
        public const int Inline = 244;

        /// <summary>
        ///     token for <c>assembler</c>
        /// </summary>
        public const int Assembler = 245;

        /// <summary>
        ///     token for <c>cdecl</c>
        /// </summary>
        public const int Cdecl = 246;

        /// <summary>
        ///     token for <c>pascal</c>
        /// </summary>
        public const int Pascal = 247;

        /// <summary>
        ///     token for <c>register</c>
        /// </summary>
        public const int Register = 248;

        /// <summary>
        ///     token for <c>safecall</c>
        /// </summary>
        public const int Safecall = 249;

        /// <summary>
        ///     token for <c>stdcall</c>
        /// </summary>
        public const int Stdcall = 250;

        /// <summary>
        ///     token for <c>export</c>
        /// </summary>
        public const int Export = 251;

        /// <summary>
        ///     token for <c>far</c>
        /// </summary>
        public const int Far = 252;

        /// <summary>
        ///     toke for <c>local</c>
        /// </summary>
        public const int Local = 253;


        /// <summary>
        ///     token for <c>near</c>
        /// </summary>
        public const int Near = 254;

        /// <summary>
        ///     token for <c>property</c>
        /// </summary>
        public const int Property = 255;

        /// <summary>
        ///     token for <c>index</c>
        /// </summary>
        public const int Index = 256;

        /// <summary>
        ///     token for <c>read</c>
        /// </summary>
        public const int Read = 257;

        /// <summary>
        ///     token for <c>write</c>
        /// </summary>
        public const int Write = 258;

        /// <summary>
        ///     token for <c>add</c>
        /// </summary>
        public const int Add = 259;

        /// <summary>
        ///     token for <c>remove</c>
        /// </summary>
        public const int Remove = 260;

        /// <summary>
        ///     token for <c>readonly</c>
        /// </summary>
        public const int ReadOnly = 261;

        /// <summary>
        ///     token for <c>writeonly</c>
        /// </summary>
        public const int WriteOnly = 262;

        /// <summary>
        ///     token for <c>dispid</c>
        /// </summary>
        public const int DispId = 263;

        /// <summary>
        ///     token for <c>default</c>
        /// </summary>
        public const int Default = 264;


        /// <summary>
        ///     token for <c>nodefault</c>
        /// </summary>
        public const int NoDefault = 265;

        /// <summary>
        ///     token for <c>stored</c>
        /// </summary>
        public const int Stored = 266;

        /// <summary>
        ///     implements
        /// </summary>
        public const int Implements = 267;

        /// <summary>
        ///     token for <c>contains</c>
        /// </summary>
        public const int Contains = 268;

        /// <summary>
        ///     token for <c>requires</c>
        /// </summary>
        public const int Requires = 269;

        /// <summary>
        ///     token for <c>interface</c>
        /// </summary>
        public const int Interface = 270;

        /// <summary>
        ///     token for <c>implementation</c>
        /// </summary>
        public const int Implementation = 271;

        /// <summary>
        ///     token for <c>initializaion</c>
        /// </summary>
        public const int Initialization = 272;

        /// <summary>
        ///     token for <c>finalization</c>
        /// </summary>
        public const int Finalization = 273;

        /// <summary>
        ///     token for <c>asm</c>
        /// </summary>
        public const int Asm = 274;

        /// <summary>
        ///     token for <c>label</c>
        /// </summary>
        public const int Label = 275;

        /// <summary>
        ///     token for <c>thread</c>
        /// </summary>
        public const int ThreadVar = 276;

        /// <summary>
        ///     token for <c>exports</c>
        /// </summary>
        public const int Exports = 277;

        /// <summary>
        ///     token for <c>assembly</c>
        /// </summary>
        public const int Assembly = 278;
    }
}
