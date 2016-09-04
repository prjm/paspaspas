﻿using PasPasPas.Infrastructure.Input;
using System.Globalization;
using System;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree {

    /// <summary>
    ///     token definition
    /// </summary>
    public class PascalToken {

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
        ///     create a new token
        /// </summary>
        public PascalToken() {
            Kind = TokenKind.Undefined;
            Value = string.Empty;
            FilePath = null;
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
        ///     file path
        /// </summary>
        public IFileReference FilePath { get; set; }

        /// <summary>
        ///     token start position
        /// </summary>
        public TextFilePosition StartPosition { get; internal set; }

        /// <summary>
        ///     token end position
        /// </summary>
        public TextFilePosition EndPosition { get; internal set; }

        /// <summary>
        ///     invalid tokens before this token
        /// </summary>
        public IEnumerable<PascalToken> InvalidTokensBefore
        {
            get
            {
                if (invalidTokensBefore.IsValueCreated) {
                    foreach (var token in invalidTokensBefore.Value)
                        yield return token;
                }
            }
        }

        /// <summary>
        ///     invalid tokens after this token
        /// </summary>
        public IEnumerable<PascalToken> InvalidTokensAfter
        {
            get
            {
                if (invalidTokensAfter.IsValueCreated) {
                    foreach (var token in invalidTokensAfter.Value)
                        yield return token;
                }
            }
        }

        /// <summary>
        ///     list of invalid tokens before this token
        /// </summary>
        private Lazy<IList<PascalToken>> invalidTokensBefore =
            new Lazy<IList<PascalToken>>(() => new List<PascalToken>());

        /// <summary>
        ///     list of invalid tokens after this token
        /// </summary>
        private Lazy<IList<PascalToken>> invalidTokensAfter =
            new Lazy<IList<PascalToken>>(() => new List<PascalToken>());

        /// <summary>
        ///     assign remaining tokens
        /// </summary>
        /// <param name="invalidTokens"></param>
        /// <param name="afterwards">add tokens after this token</param>
        public void AssignInvalidTokens(Queue<PascalToken> invalidTokens, bool afterwards) {
            if (invalidTokens.Count > 0) {
                foreach (var token in invalidTokens)
                    if (afterwards)
                        invalidTokensAfter.Value.Add(token);
                    else
                        invalidTokensBefore.Value.Add(token);

                invalidTokens.Clear();
            }
        }

        /// <summary>
        ///     format token as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => Kind.ToString(CultureInfo.InvariantCulture) + ": " + Value?.Trim();

    }

    /// <summary>
    ///     string literal token
    /// </summary>
    public class StringLiteralToken : PascalToken {

        /// <summary>
        ///     string value
        /// </summary>
        public string LiteralValue { get; set; }

    }

    /// <summary>
    ///     string literal token
    /// </summary>
    public class IntegerLiteralToken : PascalToken {

        /// <summary>
        ///     int value
        /// </summary>
        public int LiteralValue { get; set; }

    }

    /// <summary>
    ///     number token
    /// </summary>
    public class NumberLiteralToken : PascalToken {


        /// <summary>
        ///     int value
        /// </summary>
        public int LiteralValue { get; set; }

    }

}

