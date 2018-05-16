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
        public const int AngleBracketsOpen = LessThen;

        /// <summary>
        ///     token for <c>&gt;</c>
        /// </summary>
        public const int AngleBracketsClose = GreaterThen;

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

        /// <summary>
        ///     token for <c>operator</c>
        /// </summary>
        public const int Operator = 279;

        /// <summary>
        ///     token for <c>absolute</c>
        /// </summary>
        public const int Absolute = 280;

        /// <summary>
        ///     token for <c>name</c>
        /// </summary>
        public const int Name = 281;

        /// <summary>
        ///     token for <c>resident</c>
        /// </summary>
        public const int Resident = 282;

        /// <summary>
        ///     token for <c>AnsiString</c>
        /// </summary>
        public const int AnsiString = 283;

        /// <summary>
        ///     token for <c>ShortString</c>
        /// </summary>
        public const int ShortString = 284;

        /// <summary>
        ///     token for <c>string</c>
        /// </summary>
        public const int String = 285;

        /// <summary>
        ///     token for <c>widestring</c>
        /// </summary>
        public const int WideString = 286;

        /// <summary>
        ///     token for UniCodeString
        /// </summary>
        public const int UnicodeString = 287;

        /// <summary>
        ///     token for reference
        /// </summary>
        public const int Reference = 288;

        /// <summary>
        ///     token for <c>object</c>
        /// </summary>
        public const int Object = 289;

        /// <summary>
        ///     token for <c>to</c>
        /// </summary>
        public const int To = 290;

        /// <summary>
        ///     token for <c>helper</c>
        /// </summary>
        public const int Helper = 291;

        /// <summary>
        ///     token for <c>dispinterfaces</c>
        /// </summary>
        public const int DispInterface = 292;

        /// <summary>
        ///     token for <c>for</c>
        /// </summary>
        public const int For = 293;

        /// <summary>
        ///     token for <c>and</c>
        /// </summary>
        public const int And = 294;

        /// <summary>
        ///     token for <c>as</c>
        /// </summary>
        public const int As = 295;

        /// <summary>
        ///     token for <c>case</c>
        /// </summary>
        public const int Case = 296;

        /// <summary>
        ///     token for <c>div</c>
        /// </summary>
        public const int Div = 297;

        /// <summary>
        ///     token for <c>downto</c>
        /// </summary>
        public const int DownTo = 298;

        /// <summary>
        ///     token for <c>do</c>
        /// </summary>
        public const int Do = 299;

        /// <summary>
        ///     token for <c>else</c>
        /// </summary>
        public const int Else = 300;

        /// <summary>
        ///     token for <c>except</c>
        /// </summary>
        public const int Except = 301;

        /// <summary>
        ///     token for <c>finally</c>
        /// </summary>
        public const int Finally = 302;

        /// <summary>
        ///     token for <c>kexword</c>
        /// </summary>
        public const int GoToKeyword = 303;

        /// <summary>
        ///     token for <c>if</c>
        /// </summary>
        public const int If = 304;

        /// <summary>
        ///     token for <c>inherited</c>
        /// </summary>
        public const int Inherited = 305;

        /// <summary>
        ///     token for <c>is</c>
        /// </summary>
        public const int Is = 306;

        /// <summary>
        ///     token for <c>not</c>
        /// </summary>
        public const int Not = 307;

        /// <summary>
        ///     token for <c>nil</c>
        /// </summary>
        public const int Nil = 308;

        /// <summary>
        ///     token for <c>mod</c>
        /// </summary>
        public const int Mod = 309;

        /// <summary>
        ///     token for <c>or</c>
        /// </summary>
        public const int Or = 310;

        /// <summary>
        ///     token for <c>repeat</c>
        /// </summary>
        public const int Repeat = 311;

        /// <summary>
        ///     token for <c>raise</c>
        /// </summary>
        public const int Raise = 312;

        /// <summary>
        ///     token for <c>shl</c>
        /// </summary>
        public const int Shl = 313;

        /// <summary>
        ///     token for <c>shr</c>
        /// </summary>
        public const int Shr = 314;

        /// <summary>
        ///     token for <c>then</c>
        /// </summary>
        public const int Then = 315;

        /// <summary>
        ///     token for <c>try</c>
        /// </summary>
        public const int Try = 316;

        /// <summary>
        ///     token for <c>until</c>
        /// </summary>
        public const int Until = 318;

        /// <summary>
        ///     token for <c>while</c>
        /// </summary>
        public const int While = 319;

        /// <summary>
        ///     token for <c>with</c>
        /// </summary>
        public const int With = 320;

        /// <summary>
        ///     token for <c>xor</c>
        /// </summary>
        public const int Xor = 321;

        /// <summary>
        ///     token for <c>varargs</c>
        /// </summary>
        public const int VarArgs = 322;

        /// <summary>
        ///     token for <c>external</c>
        /// </summary>
        public const int External = 323;

        /// <summary>
        ///     token for <c>unsafe</c>
        /// </summary>
        public const int Unsafe = 324;

        /// <summary>
        ///     token for <c>forward</c>
        /// </summary>
        public const int Forward = 325;

        /// <summary>
        ///     token for <c>true</c>
        /// </summary>
        public const int True = 326;

        /// <summary>
        ///     token for <c>false</c>
        /// </summary>
        public const int False = 327;

        /// <summary>
        ///     token for <c>&lt;</c>
        /// </summary>
        public const int LessThen = 328;

        /// <summary>
        ///     token for <c>&lt;=</c>
        /// </summary>
        public const int LessThenEquals = 329;

        /// <summary>
        ///     token for <c>&gt;</c>
        /// </summary>
        public const int GreaterThen = 330;

        /// <summary>
        ///     token for <c>&gt;=</c>
        /// </summary>
        public const int GreaterThenEquals = 331;

        /// <summary>
        ///     token for <c>&lt;&gt;</c>
        /// </summary>
        public const int NotEquals = 332;

        /// <summary>
        ///     token for <c>exit</c>
        /// </summary>
        public const int Exit = 333;

        /// <summary>
        ///     token for <c>continue</c>
        /// </summary>
        public const int Continue = 334;

        /// <summary>
        ///     toke for <c>break</c>
        /// </summary>
        public const int Break = 335;

        /// <summary>
        ///     token for <c>:=</c>
        /// </summary>
        public const int Assignment = 336;

        /// <summary>
        ///     token for <c>on</c>
        /// </summary>
        public const int On = 337;

        /// <summary>
        ///     token for any identifier
        /// </summary>
        public const int Identifier = 500;

        /// <summary>
        ///     token for any integer
        /// </summary>
        public const int Integer = 501;

        /// <summary>
        ///     token for any real number
        /// </summary>
        public const int Real = 502;

        /// <summary>
        ///     token for any hex number
        /// </summary>
        public const int HexNumber = 503;

        /// <summary>
        ///     token for quoted strings
        /// </summary>
        public const int QuotedString = 504;

        /// <summary>
        ///     token for strings in double quotes
        /// </summary>
        public const int DoubleQuotedString = 505;

        /// <summary>
        ///     white space
        /// </summary>
        public const int WhiteSpace = 600;

        /// <summary>
        ///     control chars
        /// </summary>
        public const int ControlChar = 601;

        /// <summary>
        ///     token for <c>$A</c>
        /// </summary>
        public const int AlignSwitch = 701;

        /// <summary>
        ///     token for <c>$ALIGN</c>
        /// </summary>
        public const int AlignSwitchLong = 702;

        /// <summary>
        ///     token for <c>$APPTYPE</c>
        /// </summary>
        public const int Apptype = 703;

        /// <summary>
        ///     token for <c>$C</c>
        /// </summary>
        public const int AssertSwitch = 704;

        /// <summary>
        ///     token for <c>$ASSERTIONS</c>
        /// </summary>
        public const int AssertSwitchLong = 705;

        /// <summary>
        ///     token for <c>$B</c>
        /// </summary>
        public const int BoolEvalSwitch = 706;

        /// <summary>
        ///     token for <c>$BOOLEVAL</c>
        /// </summary>
        public const int BoolEvalSwitchLong = 707;

        /// <summary>
        ///     token for <c>$CODEALIGN</c>
        /// </summary>
        public const int CodeAlign = 708;

        /// <summary>
        ///     token for <c>$IFDEF</c>
        /// </summary>
        public const int IfDef = 709;

        /// <summary>
        ///     token for <c>$IFNDEF</c>
        /// </summary>
        public const int IfNDef = 710;


        /// <summary>
        ///     token for <c>$IF</c>
        /// </summary>
        public const int IfCd = 711;

        /// <summary>
        ///     token for <c>$ELSEIF</c>
        /// </summary>
        public const int ElseIf = 712;

        /// <summary>
        ///     token for <c>$ELSE</c>
        /// </summary>
        public const int ElseCd = 713;

        /// <summary>
        ///     token for <c>$ENDIF</c>
        /// </summary>
        public const int EndIf = 714;

        /// <summary>
        ///     token for <c>$IFEND</c>
        /// </summary>
        public const int IfEnd = 715;

        /// <summary>
        ///     token for <c>$D</c>
        /// </summary>
        public const int DebugInfoOrDescriptionSwitch = 716;

        /// <summary>
        ///     token for <c>$DEBUGINFO</c>
        /// </summary>
        public const int DebugInfoSwitchLong = 717;

        /// <summary>
        ///     token for <c>$DEFINE</c>
        /// </summary>
        public const int Define = 718;

        /// <summary>
        ///     token for <c>$DENYPACKAGEUNIT</c>
        /// </summary>
        public const int DenyPackageUnit = 719;

        /// <summary>
        ///     token for <c>$DESIGNONLY</c>
        /// </summary>
        public const int DesignOnly = 720;

        /// <summary>
        ///     token for <c>$DESCRIPTION</c>
        /// </summary>
        public const int DescriptionSwitchLong = 721;

        /// <summary>
        ///     token for <c>$E</c>
        /// </summary>
        public const int ExtensionSwitch = 722;

        /// <summary>
        ///     token for <c>$EXTENSION</c>
        /// </summary>
        public const int ExtensionSwitchLong = 723;

        /// <summary>
        ///     token for <c>$OBJEXPORTALL</c>
        /// </summary>
        public const int ObjExportAll = 724;

        /// <summary>
        ///     token for <c>$X</c>
        /// </summary>
        public const int ExtendedSyntaxSwitch = 725;

        /// <summary>
        ///     token for <c>$EXTENDEDSYNTAX</c>
        /// </summary>
        public const int ExtendedSyntaxSwitchLong = 726;

        /// <summary>
        ///     token for <c>$EXTENDEDCOMPATIBLITY</c>
        /// </summary>
        public const int ExtendedCompatibility = 728;

        /// <summary>
        ///     token for <c>$EXCESSPRECISION</c>
        /// </summary>
        public const int ExcessPrecision = 729;

        /// <summary>
        ///     token for <c>$HIGHCHARUNICODE</c>
        /// </summary>
        public const int HighCharUnicode = 730;

        /// <summary>
        ///     token for <c>$HINTS</c>
        /// </summary>
        public const int Hints = 731;

        /// <summary>
        ///     token for <c>$IFOPT</c>
        /// </summary>
        public const int IfOpt = 732;

        /// <summary>
        ///     token for <c>$IMAGEBASE</c>
        /// </summary>
        public const int ImageBase = 733;

        /// <summary>
        ///     token for <c>$IMPLICBUILD</c>
        /// </summary>
        public const int ImplicitBuild = 734;

        /// <summary>
        ///     token for <c>$G</c>
        /// </summary>
        public const int ImportedDataSwitch = 735;

        /// <summary>
        ///     token for <c>$IMPORTEDATA</c>
        /// </summary>
        public const int ImportedDataSwitchLong = 736;

        /// <summary>
        ///     token for <c>$I</c>
        /// </summary>
        public const int IncludeSwitch = 737;

        /// <summary>
        ///     token for<c>$INCLUDE</c>
        /// </summary>
        public const int IncludeSwitchLong = 738;

        /// <summary>
        ///     token for<c>$IOCHEKS</c>
        /// </summary>
        public const int IoChecks = 739;

        /// <summary>
        ///     token for <c>$LIBPREFIX</c>
        /// </summary>
        public const int LibPrefix = 740;

        /// <summary>
        ///     token for <c>$LIBSUFFIX</c>
        /// </summary>
        public const int LibSuffix = 741;

        /// <summary>
        ///     token for $LEGACYIFEND
        /// </summary>
        public const int LegacyIfEnd = 742;

        /// <summary>
        ///     token for <c>$L</c>
        /// </summary>
        public const int LinkOrLocalSymbolSwitch = 743;

        /// <summary>
        ///     token for <c>$LINK</c>
        /// </summary>
        public const int LinkSwitchLong = 744;

        /// <summary>
        ///     token for <c>$LIBVERSION</c>
        /// </summary>
        public const int LibVersion = 745;

        /// <summary>
        ///     token for <c>$LOCALSYMBOLS</c>
        /// </summary>
        public const int LocalSymbolSwithLong = 746;

        /// <summary>
        ///     token for <c>$H</c>
        /// </summary>
        public const int LongStringSwitch = 747;

        /// <summary>
        ///     token for <c>$LONGSTRINGS</c>
        /// </summary>
        public const int LongStringSwitchLong = 748;

        /// <summary>
        ///     token for <c>$MINSTACKSIZE</c>
        /// </summary>
        public const int MinMemStackSizeSwitchLong = 750;

        /// <summary>
        ///     token for <c>$MAXSTACKSIZE</c>
        /// </summary>
        public const int MaxMemStackSizeSwitchLong = 751;

        /// <summary>
        ///     token for <c>$MESSAGE</c>
        /// </summary>
        public const int MessageCd = 752;

        /// <summary>
        ///     token for <c>$METHODINFO</c>
        /// </summary>
        public const int MethodInfo = 753;

        /// <summary>
        ///     token for <c>$Z</c>
        /// </summary>
        public const int EnumSizeSwitch = 754;

        /// <summary>
        ///     token for <c>$MINENUMSIZE</c>
        /// </summary>
        public const int EnumSizeSwitchLong = 755;

        /// <summary>
        ///     token for <c>$NODEFINE</c>
        /// </summary>
        public const int NoDefine = 756;

        /// <summary>
        ///     token for <c>$NOINCLUDE</c>
        /// </summary>
        public const int NoInclude = 757;

        /// <summary>
        ///     token for <c></c>
        /// </summary>
        public const int ObjTypeName = 758;

        /// <summary>
        ///     token for <c>$OLDTYPELAYOUT</c>
        /// </summary>
        public const int OldTypeLayout = 759;

        /// <summary>
        ///     token for <c>$P</c>
        /// </summary>
        public const int OpenStringSwitch = 760;

        /// <summary>
        ///     token for <c>$OPENSTRINGS</c>
        /// </summary>
        public const int OpenStringSwitchLong = 761;

        /// <summary>
        ///     token for <c>$O</c>
        /// </summary>
        public const int OptimizationSwitch = 762;

        /// <summary>
        ///     token for <c>$Q</c>
        /// </summary>
        public const int OverflowSwitch = 763;

        /// <summary>
        ///     token for <c>$OVERFLOWCHECKS</c>
        /// </summary>
        public const int OverflowSwitchLong = 764;

        /// <summary>
        ///     token for <c>$SETPEFLAGS</c>
        /// </summary>
        public const int SetPEFlags = 765;

        /// <summary>
        ///     token for <c>$SETPEPOPTFLAGS</c>
        /// </summary>
        public const int SetPEOptFlags = 766;

        /// <summary>
        ///     token for <c>$SETPEOSVERSION</c>
        /// </summary>
        public const int SetPEOsVersion = 767;

        /// <summary>
        ///     tkoen for <c>$SETPESUBSYSTEMVERSION</c>
        /// </summary>
        public const int SetPESubsystemVersion = 768;

        /// <summary>
        ///     token for <c>$SETPEUSERVERIONS</c>
        /// </summary>
        public const int SetPEUserVersion = 769;

        /// <summary>
        ///     token for <c>$U</c>
        /// </summary>
        public const int SaveDivideSwitch = 770;

        /// <summary>
        ///     token for <c>$SAVEDIVIDE</c>
        /// </summary>
        public const int SafeDivideSwitchLong = 771;

        /// <summary>
        ///     token for <c>$POINTERMATH</c>
        /// </summary>
        public const int Pointermath = 772;

        /// <summary>
        ///     token for <c>$R</c>
        /// </summary>
        public const int IncludeRessource = 773;

        /// <summary>
        ///     token for <c>$RANGECHECKS</c>
        /// </summary>
        public const int RangeChecks = 774;

        /// <summary>
        ///     token for <c>$REALCOMPATIBILITY</c>
        /// </summary>
        public const int RealCompatibility = 775;

        /// <summary>
        ///     token for <c>$REGION</c>
        /// </summary>
        public const int Region = 776;


        /// <summary>
        ///     token for <c>$OPTIMIZATION</c>
        /// </summary>
        public const int OptimizationSwitchLong = 777;

        /// <summary>
        ///     token for <c>%ENDREGION</c>
        /// </summary>
        public const int EndRegion = 778;

        /// <summary>
        ///     token for <c>$RESSOURCE</c>
        /// </summary>
        public const int IncludeRessourceLong = 779;

        /// <summary>
        ///     token for <c>$RTTI</c>
        /// </summary>
        public const int Rtti = 780;

        /// <summary>
        ///     token for <c>$RUNONLY</c>
        /// </summary>
        public const int RunOnly = 781;

        /// <summary>
        ///     token for <c>$M</c>
        /// </summary>
        public const int TypeInfoSwitch = 782;

        /// <summary>
        ///     token for <c>$TYPEINFO</c>
        /// </summary>
        public const int TypeInfoSwitchLong = 783;

        /// <summary>
        ///     token for <c>$W</c>
        /// </summary>
        public const int StackFramesSwitch = 784;

        /// <summary>
        ///     token for <c>$STACKFRAMES</c>
        /// </summary>
        public const int StackFramesSwitchLong = 785;

        /// <summary>
        ///     token for <c>$SCOPEDENUMS</c>
        /// </summary>
        public const int ScopedEnums = 786;

        /// <summary>
        ///     token for <c>$STRONGLINKTYPES</c>
        /// </summary>
        public const int StrongLinkTypes = 787;

        /// <summary>
        ///     token for <c>$Y</c>
        /// </summary>
        public const int SymbolDeclarationSwitch = 788;

        /// <summary>
        ///     token for <c>$REFERENCINFO</c>
        /// </summary>
        public const int ReferenceInfo = 789;

        /// <summary>
        ///     token for <c>$DEFINTIONINFO</c>
        /// </summary>
        public const int DefinitionInfo = 790;

        /// <summary>
        ///     token for <c>$T</c>
        /// </summary>
        public const int TypedPointersSwitch = 791;

        /// <summary>
        ///     token for <c>$TYPEDADDRESS</c>
        /// </summary>
        public const int TypedPointersSwitchLong = 792;

        /// <summary>
        ///     tkoen for <c>$UNDEF</c>
        /// </summary>
        public const int Undef = 793;

        /// <summary>
        ///     token for <c>$V</c>
        /// </summary>
        public const int VarStringCheckSwitch = 794;

        /// <summary>
        ///     token for <c>$VARCHECKSTRINGS</c>
        /// </summary>
        public const int VarStringCheckSwitchLong = 795;

        /// <summary>
        ///     token for <c>$WARN</c>
        /// </summary>
        public const int Warn = 796;

        /// <summary>
        ///     token for <c>$WARNINGS</c>
        /// </summary>
        public const int Warnings = 797;

        /// <summary>
        ///     token for <c>$WEAKPACKAGFEUNIT</c>
        /// </summary>
        public const int WeakPackageUnit = 798;

        /// <summary>
        ///     token for <c>$WEAKLINKRTTI</c>
        /// </summary>
        public const int WeakLinkRtti = 799;

        /// <summary>
        ///     token for <c>$J</c>
        /// </summary>
        public const int WritableConstSwitch = 800;

        /// <summary>
        ///     token for <c>$WRITEABLECONST</c>
        /// </summary>
        public const int WritableConstSwitchLong = 801;

        /// <summary>
        ///     token for <c>$ZEROBASEDSTRINGS</c>
        /// </summary>
        public const int ZeroBaseStrings = 802;

        /// <summary>
        ///     token for <c>$A1</c>
        /// </summary>
        public const int AlignSwitch1 = 803;

        /// <summary>
        ///     token for <c>$A2</c>
        /// </summary>
        public const int AlignSwitch2 = 804;

        /// <summary>
        ///     token for <c>$A4</c>
        /// </summary>
        public const int AlignSwitch4 = 805;

        /// <summary>
        ///     token for <c>$A8</c>
        /// </summary>
        public const int AlignSwitch8 = 806;

        /// <summary>
        ///     token for <c>$A16</c>
        /// </summary>
        public const int AlignSwitch16 = 807;

        /// <summary>
        ///     token for <c>off</c>
        /// </summary>
        public const int Off = 808;

        /// <summary>
        ///     token for <c>$EXTERNALSYM</c>
        /// </summary>
        public const int ExternalSym = 809;

        /// <summary>
        ///     token for <c>$HPPEMIT</c>
        /// </summary>
        public const int HppEmit = 810;

        /// <summary>
        ///     token for <c>LINKUNIT</c>
        /// </summary>
        public const int LinkUnit = 811;

        /// <summary>
        ///     token for <c>OPENNAMESPACe</c>
        /// </summary>
        public const int OpenNamespace = 812;

        /// <summary>
        ///     token for <c>CLOSENAMESPACE</c>
        /// </summary>
        public const int CloseNamepsace = 813;

        /// <summary>
        ///     token for <c>NOUSINGNAMESPACE</c>
        /// </summary>
        public const int NoUsingNamespace = 814;

        /// <summary>
        ///     token for <c>$YD</c>
        /// </summary>
        public const int SymbolDefinitionsOnlySwitch = 815;

        /// <summary>
        ///     token for <c>ERROR</c>
        /// </summary>
        public const int Error = 815;

        /// <summary>
        ///     token for <c>INHERIT</c>
        /// </summary>
        public const int Inherit = 816;

        /// <summary>
        ///     token for <c>EXPLICIT</c>
        /// </summary>
        public const int Explicit = 817;

        /// <summary>
        ///     token for <c>METHODS</c>
        /// </summary>
        public const int Methods = 818;

        /// <summary>
        ///     token for <c>PROPERTIES</c>
        /// </summary>
        public const int Properties = 819;

        /// <summary>
        ///     token for <c>FIELDS</c>
        /// </summary>
        public const int Fields = 820;

        /// <summary>
        ///     token for <c>vcPrivate</c>
        /// </summary>
        public const int VcPrivate = 821;

        /// <summary>
        ///     toke for <c>vcProtected</c>
        /// </summary>
        public const int VcProtected = 822;

        /// <summary>
        ///     token for <c>vcPublic</c>
        /// </summary>
        public const int VcPublic = 823;

        /// <summary>
        ///     token for <c>vcPublished</c>
        /// </summary>
        public const int VcPublished = 824;

        /// <summary>
        ///     token for <c>Z1</c>
        /// </summary>
        public const int EnumSize1 = 825;

        /// <summary>
        ///     token for <c>Z2</c>
        /// </summary>
        public const int EnumSize2 = 826;

        /// <summary>
        ///     token for <c>Z4</c>
        /// </summary>
        public const int EnumSize4 = 827;

        /// <summary>
        ///     token for <c>at</c>
        /// </summary>
        public const int AtWord = 828;

        /// <summary>
        ///     token for <c>dependency</c>
        /// </summary>
        public const int Dependency = 829;

        /// <summary>
        ///     token for <c>delayed</c>
        /// </summary>
        public const int Delayed = 830;

        /// <summary>
        ///     invalid tokens
        /// </summary>
        public const int Invalid = 999;
    }
}
