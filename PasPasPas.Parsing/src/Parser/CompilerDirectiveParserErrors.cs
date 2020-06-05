#nullable disable
namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     compiler directive parser error
    /// </summary>
    public static class CompilerDirectiveParserErrors {

        /// <summary>
        ///     invalid align directive
        /// </summary>
        public const uint InvalidAlignDirective
             = 0x0201;

        /// <summary>
        ///     invalid application type
        /// </summary>
        public const uint InvalidApplicationType
             = 0x0202;

        /// <summary>
        ///     invalid assertion type
        /// </summary>
        public const uint InvalidAssertDirective
            = 0x0203;

        /// <summary>
        ///     invalid boolean evaluation switch
        /// </summary>
        public const uint InvalidBoolEvalDirective
            = 0x0204;

        /// <summary>
        ///     invalid code align parameter value
        /// </summary>
        public const uint InvalidCodeAlignDirective
            = 0x0205;

        /// <summary>
        ///     invalid debug info directive
        /// </summary>
        public const uint InvalidDebugInfoDirective
            = 0x0206;

        /// <summary>
        ///     invalid define
        /// </summary>
        public const uint InvalidDefineDirective
            = 0x0207;

        /// <summary>
        ///     invalid undefine
        /// </summary>
        public const uint InvalidUnDefineDirective
            = 0x0208;

        /// <summary>
        ///     invalid ifdef
        /// </summary>
        public const uint InvalidIfDefDirective
            = 0x0209;


        /// <summary>
        ///     invalid ifndef
        /// </summary>
        public const uint InvalidIfNDefDirective
             = 0x0210;

        /// <summary>
        ///     end if without if
        /// </summary>
        public const uint EndIfWithoutIf
             = 0x0211;

        /// <summary>
        ///     else without if
        /// </summary>
        public const uint ElseIfWithoutIf
            = 0x0212;


        /// <summary>
        ///     invalid denypackageunit directive
        /// </summary>
        public const uint InvalidDenyPackageUnitDirective
            = 0x0213;

        /// <summary>
        ///     invalid description directive
        /// </summary>
        public const uint InvalidDescriptionDirective
             = 0x0214;

        /// <summary>
        ///     invalid design-time only directive
        /// </summary>
        public const uint InvalidDesignTimeOnlyDirective
             = 0x0215;

        /// <summary>
        ///     invalid extension directive
        /// </summary>
        public const uint InvalidExtensionDirective
            = 0x0216;

        /// <summary>
        ///     invalid object export directive
        /// </summary>
        public const uint InvalidObjectExportDirective
            = 0x0217;

        /// <summary>
        ///     extended compatibility directive
        /// </summary>
        public const uint InvalidExtendedCompatibilityDirective
            = 0x0218;

        /// <summary>
        ///     extended syntax directive
        /// </summary>
        public const uint InvalidExtendedSyntaxDirective
             = 0x0219;

        /// <summary>
        ///     invalid external symbol directive
        /// </summary>
        public const uint InvalidExternalSymbolDirective
             = 0x0220;

        /// <summary>
        ///     invalid excess precision directive
        /// </summary>
        public const uint InvalidExcessPrecisionDirective
             = 0x0221;

        /// <summary>
        ///     invalid high char unicode switch
        /// </summary>
        public const uint InvalidHighCharUnicodeDirective
            = 0x0222;

        /// <summary>
        ///     invalid hints directive
        /// </summary>
        public const uint InvalidHintsDirective
             = 0x0223;

        /// <summary>
        ///     invalid hpp emit directive
        /// </summary>
        public const uint InvalidHppEmitDirective
             = 0x0224;

        /// <summary>
        ///     invalid image base directive
        /// </summary>
        public const uint InvalidImageBaseDirective
             = 0x0225;

        /// <summary>
        ///     invalid implicit build directive
        /// </summary>
        public const uint InvalidImplicitBuildDirective
            = 0x0226;

        /// <summary>
        ///     invalid io checks directive
        /// </summary>
        public const uint InvalidIoChecksDirective
            = 0x0227;

        /// <summary>
        ///     invalid local symbols directive
        /// </summary>
        public const uint InvalidLocalSymbolsDirective
            = 0x0228;

        /// <summary>
        ///     invalid long string switch directive
        /// </summary>
        public const uint InvalidLongStringSwitchDirective
            = 0x0229;

        /// <summary>
        ///     invalid open strings directive
        /// </summary>
        public const uint InvalidOpenStringsDirective
             = 0x0230;

        /// <summary>
        ///     invalid optimization directive
        /// </summary>
        public const uint InvalidOptimizationDirective
            = 0x0231;

        /// <summary>
        ///     invalid check overflow directive
        /// </summary>
        public const uint InvalidOverflowCheckDirective
             = 0x0232;

        /// <summary>
        ///     invalid safe divide directive
        /// </summary>
        public const uint InvalidSafeDivide
            = 0x0233;

        /// <summary>
        ///     invalid range check directive
        /// </summary>
        public const uint InvalidRangeCheckDirective
            = 0x0234;

        /// <summary>
        ///    invalid stack frames directive
        /// </summary>
        public const uint InvalidStackFramesDirective
            = 0x0235;

        /// <summary>
        ///     invalid zero based string directive
        /// </summary>
        public const uint InvalidZeroBasedStringsDirective
            = 0x0236;

        /// <summary>
        ///     invalid writable constants directive
        /// </summary>
        public const uint InvalidWritableConstantsDirective
            = 0x0237;

        /// <summary>
        ///     invalid weak link directive
        /// </summary>
        public const uint InvalidWeakLinkRttiDirective
            = 0x0238;

        /// <summary>
        ///     invalid warnings directive
        /// </summary>
        public const uint InvalidWarningsDirective
            = 0x0239;

        /// <summary>
        ///     invalid warn directive
        /// </summary>
        public const uint InvalidWarnDirective
            = 0x0240;

        /// <summary>
        ///     invalid string check directive
        /// </summary>
        public const uint InvalidStringCheckDirective
            = 0x0241;

        /// <summary>
        ///     invalid type checked pointer directive
        /// </summary>
        public const uint InvalidTypeCheckedPointersDirective
            = 0x0242;

        /// <summary>
        ///     invalid definition info switch
        /// </summary>
        public const uint InvalidDefinitionInfoDirective
            = 0x0243;

        /// <summary>
        ///     invalid strong link types directive
        /// </summary>
        public const uint InvalidStrongLinkTypesDirective
            = 0x0244;

        /// <summary>
        ///     invalid scoped enumerations directive
        /// </summary>
        public const uint InvalidScopedEnumsDirective
            = 0x0245;

        /// <summary>
        ///     invalid published rtti directive
        /// </summary>
        public const uint InvalidPublishedRttiDirective
            = 0x0246;

        /// <summary>
        ///     invalid run only directive
        /// </summary>
        public const uint InvalidRunOnlyDirective
            = 0x0247;

        /// <summary>
        ///     invalid legacy if end directive
        /// </summary>
        public const uint InvalidLegacyIfEndDirective
            = 0x0248;

        /// <summary>
        ///     invalid legacy var prop setter directive
        /// </summary>
        public const uint InvalidVarPropSetterDirective
            = 0x0249;

        /// <summary>
        ///     invalid real48 mode
        /// </summary>
        public const uint InvalidRealCompatibilityMode
            = 0x0250;

        /// <summary>
        ///     invalid pointer math directive
        /// </summary>
        public const uint InvalidPointerMathDirective
            = 0x0251;

        /// <summary>
        ///     invalid old type layout directive
        /// </summary>
        public const uint InvalidOldTypeLayoutDirective
            = 0x0252;

        /// <summary>
        ///     invalid no define directive
        /// </summary>
        public const uint InvalidNoDefineDirective
            = 0x0253;

        /// <summary>
        ///     invalid object type directive
        /// </summary>
        public const uint InvalidObjTypeDirective
            = 0x0254;

        /// <summary>
        ///     invalid no include directive
        /// </summary>
        public const uint InvalidNoIncludeDirective
             = 0x0255;


        /// <summary>
        ///     invalid min enum size
        /// </summary>
        public const uint InvalidMinEnumSizeDirective
            = 0x0256;

        /// <summary>
        ///     invalid method info
        /// </summary>
        public const uint InvalidMethodInfoDirective
             = 0x0257;

        /// <summary>
        ///     invalid lib directive
        /// </summary>
        public const uint InvalidLibDirective
            = 0x0258;

        /// <summary>
        ///     invalid pe version directive
        /// </summary>
        public const uint InvalidPEVersionDirective
            = 0x0259;

        /// <summary>
        ///     invalid region directive
        /// </summary>
        public const uint InvalidRegionDirective
            = 0x0260;

        /// <summary>
        ///     end region without region
        /// </summary>
        public const uint EndRegionWithoutRegion
            = 0x0261;

        /// <summary>
        ///     invalid weak package unit directive
        /// </summary>
        public const uint InvalidWeakPackageUnitDirective
            = 0x0262;

        /// <summary>
        ///     invalid rtti directive
        /// </summary>
        public const uint InvalidRttiDirective
            = 0x0263;

        /// <summary>
        ///     invalid imported data directive
        /// </summary>
        public const uint InvalidImportedDataDirective
            = 0x0264;

        /// <summary>
        ///     invalid message directive
        /// </summary>
        public const uint InvalidMessageDirective
            = 0x0265;

        /// <summary>
        ///     invalid stack memory size directive
        /// </summary>
        public const uint InvalidStackMemorySizeDirective
            = 0x0266;

        /// <summary>
        ///     invalid file name
        /// </summary>
        public const uint InvalidFileName
            = 0x0267;

        /// <summary>
        ///     invalid link directive
        /// </summary>
        public const uint InvalidLinkDirective
            = 0x0268;

        /// <summary>
        ///     invalid resource directive
        /// </summary>
        public const uint InvalidResourceDirective
            = 0x0269;

        /// <summary>
        ///     invalid include directive
        /// </summary>
        public const uint InvalidIncludeDirective
           = 0x0270;

    }
}