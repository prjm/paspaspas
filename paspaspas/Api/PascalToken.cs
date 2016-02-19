using System;
using System.Globalization;

namespace PasPasPas.Api {

    /// <summary>
    ///     exception class for unknown token kinds
    /// </summary>
    public class UnknownTokenKindException : PasPasPasException {

        /// <summary>
        ///     unkown toke kind
        /// </summary>
        private int kind;

        /// <summary>
        ///     creates a new unknown 
        /// </summary>
        /// <param name="kind"></param>
        public UnknownTokenKindException(int kind) {
            this.kind = kind;
        }

        /// <summary>
        ///     Unkown exception kind
        /// </summary>
        public int Kind
            => kind;
    }

    /// <summary>
    ///     generic structur for tokens
    /// </summary>
    public struct PascalToken {

        /// <summary>
        ///     undefined tokenS
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
        ///     format a string as quoted string
        /// </summary>
        /// <param name="deprecated"></param>
        /// <returns></returns>
        internal static string ToQuotedString(string deprecated)
            => "'" + deprecated + "'";

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
        ///     token for protected
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
        ///     white space
        /// </summary>
        public const int WhiteSpace = 600;

        /// <summary>
        ///     create a new token
        /// </summary>
        /// <param name="tokenId">tokenid</param>
        /// <param name="tokenValue">token value</param>
        public PascalToken(int tokenId, string tokenValue) : this() {
            Kind = tokenId;
            Value = tokenValue;
        }

        /// <summary>
        ///     Token value
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        ///     Token kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="left">left</param>
        /// <param name="right">right</param>
        /// <returns><c>bool</c> if the tokens are equal</returns>
        public static bool operator ==(PascalToken left, PascalToken right)
            => (left.Kind == right.Kind) && (string.Equals(left.Value, right.Value, StringComparison.OrdinalIgnoreCase));


        /// <summary>
        ///     check for inequality
        /// </summary>
        /// <param name="left">left</param>
        /// <param name="right">right</param>
        /// <returns><c>bool</c> if the tokens are not equal</returns>
        public static bool operator !=(PascalToken left, PascalToken right)
            => !(left == right);

        /// <summary>
        ///     format token as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => Kind.ToString(CultureInfo.InvariantCulture) + ": " + Value?.Trim();


        /// <summary>
        ///     compare to another token
        /// </summary>
        /// <param name="obj">object to compare to</param>
        /// <returns><c>true</c> if the objects are equal</returns>
        public override bool Equals(object obj) {
            if (!(obj is PascalToken))
                return false;

            var otherToken = (PascalToken)obj;
            return  //
                (otherToken.Kind == Kind) && //
                (string.Equals(otherToken.Value, Value, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        ///     compute the hash chode
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            int hash = 17;
            hash = hash * 23 + Kind;
            hash = hash * 23 + ((Value?.GetHashCode()) ?? 0);
            return hash;
        }
    }
}

