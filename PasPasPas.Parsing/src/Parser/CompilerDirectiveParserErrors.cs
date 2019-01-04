using System;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     compiler directive parser error
    /// </summary>
    public static class CompilerDirectiveParserErrors {

        /// <summary>
        ///     invalid align directive
        /// </summary>
        public static readonly Guid InvalidAlignDirective
             = new Guid(new byte[] { 0x84, 0x38, 0x93, 0x47, 0xd, 0x5b, 0x98, 0x48, 0xa5, 0x30, 0x56, 0xf5, 0xbf, 0x62, 0x48, 0x3b });
        /* {47933884-5b0d-4898-a530-56f5bf62483b} */

        /// <summary>
        ///     invalid application type
        /// </summary>
        public static readonly Guid InvalidApplicationType
             = new Guid(new byte[] { 0xc4, 0xca, 0x37, 0xc6, 0x34, 0xb3, 0xa9, 0x4d, 0xae, 0x93, 0x23, 0xa7, 0x2f, 0xba, 0xd7, 0x85 });
        /* {c637cac4-b334-4da9-ae93-23a72fbad785} */

        /// <summary>
        ///     invalid assertion type
        /// </summary>
        public static readonly Guid InvalidAssertDirective
            = new Guid(new byte[] { 0xaa, 0xa0, 0x93, 0x1f, 0x7, 0x19, 0xee, 0x40, 0xa4, 0xdf, 0xa6, 0x6f, 0x22, 0xcc, 0x79, 0xae });
        /* {1f93a0aa-1907-40ee-a4df-a66f22cc79ae} */

        /// <summary>
        ///     invalid boolean evaluation switch
        /// </summary>
        public static readonly Guid InvalidBoolEvalDirective
            = new Guid(new byte[] { 0x66, 0x25, 0x43, 0xe5, 0xe9, 0xa2, 0xd4, 0x45, 0xae, 0x7c, 0x65, 0x2c, 0x65, 0x7b, 0xb5, 0x33 });
        /* {e5432566-a2e9-45d4-ae7c-652c657bb533} */

        /// <summary>
        ///     invalid code align parameter value
        /// </summary>
        public static readonly Guid InvalidCodeAlignDirective
            = new Guid(new byte[] { 0xf0, 0x4, 0x73, 0xac, 0xee, 0x9, 0xb4, 0x47, 0xa6, 0xf6, 0x12, 0x8, 0xbd, 0x3d, 0x20, 0xa });
        /* {ac7304f0-09ee-47b4-a6f6-1208bd3d200a} */

        /// <summary>
        ///     invalid debug info directive
        /// </summary>
        public static readonly Guid InvalidDebugInfoDirective
            = new Guid(new byte[] { 0xe0, 0x96, 0x67, 0x51, 0x15, 0x6f, 0xfd, 0x4e, 0xb5, 0x2b, 0xbc, 0x41, 0x28, 0xe, 0xe, 0x18 });
        /* {516796e0-6f15-4efd-b52b-bc41280e0e18} */

        /// <summary>
        ///     invalid define
        /// </summary>
        public static readonly Guid InvalidDefineDirective
            = new Guid(new byte[] { 0x8e, 0xe8, 0xc4, 0x1, 0x95, 0x7d, 0xcf, 0x4a, 0x89, 0x92, 0x57, 0xd6, 0x4b, 0x3f, 0x8d, 0xb3 });
        /* {01c4e88e-7d95-4acf-8992-57d64b3f8db3} */

        /// <summary>
        ///     invalid undefine
        /// </summary>
        public static readonly Guid InvalidUnDefineDirective
            = new Guid(new byte[] { 0x91, 0x7a, 0x5c, 0xc5, 0xe2, 0x92, 0xe5, 0x48, 0xbd, 0xf6, 0xc4, 0x1a, 0x94, 0x77, 0x78, 0x10 });
        /* {c55c7a91-92e2-48e5-bdf6-c41a94777810} */

        /// <summary>
        ///     invalid ifdef
        /// </summary>
        public static readonly Guid InvalidIfDefDirective
            = new Guid(new byte[] { 0x56, 0x1e, 0x6d, 0xc4, 0xed, 0x4b, 0x8b, 0x40, 0xb4, 0x90, 0xe4, 0x69, 0xd5, 0x5c, 0x6e, 0x41 });
        /* {c46d1e56-4bed-408b-b490-e469d55c6e41} */


        /// <summary>
        ///     invalid ifndef
        /// </summary>
        public static readonly Guid InvalidIfNDefDirective
             = new Guid(new byte[] { 0x97, 0x4c, 0xfc, 0xbb, 0x7a, 0xf5, 0x24, 0x46, 0x84, 0x1f, 0x3, 0x73, 0x86, 0x4d, 0xa2, 0x35 });
        /* {bbfc4c97-f57a-4624-841f-0373864da235} */

        /// <summary>
        ///     end if without if
        /// </summary>
        public static readonly Guid EndIfWithoutIf
             = new Guid(new byte[] { 0xa7, 0xaa, 0x28, 0x17, 0xe7, 0x86, 0x81, 0x45, 0x9c, 0x7e, 0x9f, 0xa1, 0xdd, 0x76, 0x41, 0x65 });
        /* {1728aaa7-86e7-4581-9c7e-9fa1dd764165} */

        /// <summary>
        ///     else without if
        /// </summary>
        public static readonly Guid ElseIfWithoutIf
            = new Guid(new byte[] { 0x80, 0x63, 0x95, 0x6, 0x28, 0x7e, 0x22, 0x48, 0x9b, 0x2e, 0xf5, 0xd4, 0x5b, 0xf2, 0xf5, 0xb6 });
        /* {06956380-7e28-4822-9b2e-f5d45bf2f5b6} */


        /// <summary>
        ///     invalid denypackageunit directive
        /// </summary>
        public static readonly Guid InvalidDenyPackageUnitDirective
            = new Guid(new byte[] { 0x64, 0x90, 0xc2, 0x84, 0xa2, 0x99, 0x56, 0x49, 0xb5, 0x8, 0x76, 0x27, 0xc, 0xc8, 0xf5, 0xca });
        /* {84c29064-99a2-4956-b508-76270cc8f5ca} */

        /// <summary>
        ///     invalid description directive
        /// </summary>
        public static readonly Guid InvalidDescriptionDirective
             = new Guid(new byte[] { 0x90, 0xc7, 0xa0, 0x3c, 0xae, 0xdb, 0x3f, 0x43, 0x96, 0xf8, 0x63, 0x92, 0x5a, 0xc2, 0xc, 0xb1 });
        /* {3ca0c790-dbae-433f-96f8-63925ac20cb1} */

        /// <summary>
        ///     invalid design-time only directive
        /// </summary>
        public static readonly Guid InvalidDesignTimeOnlyDirective
             = new Guid(new byte[] { 0x18, 0xcc, 0xd4, 0x9e, 0x5, 0xa2, 0xef, 0x47, 0x8d, 0x34, 0xec, 0x5f, 0x90, 0xd5, 0x5f, 0x10 });
        /* {9ed4cc18-a205-47ef-8d34-ec5f90d55f10} */

        /// <summary>
        ///     invalid extension directive
        /// </summary>
        public static readonly Guid InvalidExtensionDirective
            = new Guid(new byte[] { 0x43, 0x87, 0x4a, 0x38, 0x69, 0x5c, 0x97, 0x4c, 0x87, 0xe3, 0x7c, 0x6, 0x80, 0x34, 0x8, 0x68 });
        /* {384a8743-5c69-4c97-87e3-7c0680340868} */


        /// <summary>
        ///     invalid object export directive
        /// </summary>
        public static readonly Guid InvalidObjectExportDirective
            = new Guid(new byte[] { 0x5e, 0xcf, 0x30, 0x41, 0x59, 0x35, 0x6f, 0x4d, 0xaf, 0xfb, 0x74, 0x3c, 0x9d, 0xa0, 0xf1, 0x93 });
        /* {4130cf5e-3559-4d6f-affb-743c9da0f193} */


        /// <summary>
        ///     extended compatibility directive
        /// </summary>
        public static readonly Guid InvalidExtendedCompatibilityDirective
            = new Guid(new byte[] { 0x2e, 0xfb, 0x0, 0xad, 0xd, 0x6d, 0xc4, 0x4b, 0xb8, 0xcd, 0x6, 0xf5, 0x4f, 0x85, 0x29, 0x32 });
        /* {ad00fb2e-6d0d-4bc4-b8cd-06f54f852932} */

        /// <summary>
        ///     extended syntax directive
        /// </summary>
        public static readonly Guid InvalidExtendedSyntaxDirective
             = new Guid(new byte[] { 0x7e, 0xd6, 0x4e, 0xb4, 0xef, 0x91, 0xa9, 0x43, 0xa0, 0x7a, 0xe, 0x24, 0xc, 0xa1, 0x6b, 0x26 });
        /* {b44ed67e-91ef-43a9-a07a-0e240ca16b26} */


        /// <summary>
        ///     invalid external symbol directive
        /// </summary>
        public static readonly Guid InvalidExternalSymbolDirective
             = new Guid(new byte[] { 0x2b, 0xba, 0xbf, 0x50, 0x5f, 0x8f, 0xa6, 0x4b, 0xb1, 0x91, 0xb5, 0x42, 0xa0, 0x9e, 0xa0, 0x43 });
        /* {50bfba2b-8f5f-4ba6-b191-b542a09ea043} */


        /// <summary>
        ///     invalid excess precision directive
        /// </summary>
        public static readonly Guid InvalidExcessPrecisionDirective
             = new Guid(new byte[] { 0xf, 0x5, 0xf7, 0xd0, 0x58, 0x79, 0xea, 0x45, 0x85, 0x2f, 0x3d, 0x39, 0xb0, 0x31, 0x45, 0xc5 });
        /* {d0f7050f-7958-45ea-852f-3d39b03145c5} */


        /// <summary>
        ///     invalid high char unicode switch
        /// </summary>
        public static readonly Guid InvalidHighCharUnicodeDirective
            = new Guid(new byte[] { 0x42, 0x87, 0xf0, 0x8c, 0x1c, 0x89, 0xc, 0x4e, 0x86, 0x50, 0x7a, 0xa0, 0x18, 0x57, 0x6a, 0xa7 });
        /* {8cf08742-891c-4e0c-8650-7aa018576aa7} */


        /// <summary>
        ///     invalid hints directive
        /// </summary>
        public static readonly Guid InvalidHintsDirective
             = new Guid(new byte[] { 0xe4, 0x15, 0x1e, 0xaf, 0x61, 0xf5, 0xa6, 0x44, 0xbf, 0x90, 0x6d, 0x61, 0x93, 0xfd, 0x18, 0x7e });
        /* {af1e15e4-f561-44a6-bf90-6d6193fd187e} */

        /// <summary>
        ///     invalid hpp emit directive
        /// </summary>
        public static readonly Guid InvalidHppEmitDirective
             = new Guid(new byte[] { 0x12, 0x26, 0x98, 0x1a, 0x70, 0x4f, 0x21, 0x46, 0xaf, 0x30, 0xc8, 0x55, 0xed, 0x5c, 0x8c, 0xfa });
        /* {1a982612-4f70-4621-af30-c855ed5c8cfa} */


        /// <summary>
        ///     invalid image base directive
        /// </summary>
        public static readonly Guid InvalidImageBaseDirective
             = new Guid(new byte[] { 0xda, 0xf5, 0xfd, 0x64, 0x6a, 0xbb, 0xb1, 0x42, 0x92, 0x23, 0x61, 0xad, 0x56, 0x72, 0xbd, 0x41 });
        /* {64fdf5da-bb6a-42b1-9223-61ad5672bd41} */


        /// <summary>
        ///     invalid implicit build directive
        /// </summary>
        public static readonly Guid InvalidImplicitBuildDirective
            = new Guid(new byte[] { 0x4a, 0x84, 0x8a, 0xb5, 0x74, 0x77, 0x51, 0x49, 0xbb, 0xcf, 0x0, 0x18, 0xea, 0x15, 0xa1, 0xa0 });
        /* {b58a844a-7774-4951-bbcf-0018ea15a1a0} */


        /// <summary>
        ///     invalid io checks directive
        /// </summary>
        public static readonly Guid InvalidIoChecksDirective
            = new Guid(new byte[] { 0x21, 0xd, 0x41, 0xaf, 0x7a, 0xae, 0x50, 0x4c, 0x8c, 0x8b, 0x8, 0xe3, 0x82, 0xb3, 0x11, 0xb4 });
        /* {af410d21-ae7a-4c50-8c8b-08e382b311b4} */


        /// <summary>
        ///     invalid local symbols directive
        /// </summary>
        public static readonly Guid InvalidLocalSymbolsDirective
            = new Guid(new byte[] { 0xd1, 0x65, 0xa0, 0xda, 0x2b, 0x59, 0x96, 0x42, 0x84, 0xd4, 0xe2, 0x6e, 0x95, 0x9f, 0xb4, 0xf1 });
        /* {daa065d1-592b-4296-84d4-e26e959fb4f1} */


        /// <summary>
        ///     invalid long string switch directive
        /// </summary>
        public static readonly Guid InvalidLongStringSwitchDirective
         = new Guid(new byte[] { 0x58, 0x94, 0xce, 0x60, 0xdf, 0x58, 0x36, 0x4b, 0x8d, 0x3d, 0x69, 0xeb, 0xf6, 0x21, 0x2, 0x4b });
        /* {60ce9458-58df-4b36-8d3d-69ebf621024b} */


        /// <summary>
        ///     invalid open strings directive
        /// </summary>
        public static readonly Guid InvalidOpenStringsDirective
             = new Guid(new byte[] { 0x91, 0x4, 0x23, 0x6b, 0x57, 0x13, 0x8a, 0x47, 0xb4, 0x59, 0x69, 0x8, 0xf4, 0x5c, 0x77, 0xb9 });
        /* {6b230491-1357-478a-b459-6908f45c77b9} */


        /// <summary>
        ///     invalid optimization directive
        /// </summary>
        public static readonly Guid InvalidOptimizationDirective
            = new Guid(new byte[] { 0xb8, 0xb, 0xb2, 0xe2, 0x45, 0x4e, 0x2, 0x41, 0xb0, 0xb8, 0x3c, 0xf5, 0x66, 0x24, 0x26, 0x1e });
        /* {e2b20bb8-4e45-4102-b0b8-3cf56624261e} */


        /// <summary>
        ///     invalid check overflow directive
        /// </summary>
        public static readonly Guid InvalidOverflowCheckDirective
             = new Guid(new byte[] { 0x36, 0xbe, 0x6a, 0x5c, 0x93, 0xc3, 0xdb, 0x40, 0x94, 0x97, 0x79, 0x37, 0xe2, 0xc7, 0xfa, 0x9a });
        /* {5c6abe36-c393-40db-9497-7937e2c7fa9a} */

        /// <summary>
        ///     invalid safe divide directive
        /// </summary>
        public static readonly Guid InvalidSafeDivide
            = new Guid(new byte[] { 0xdc, 0xde, 0x27, 0x28, 0xb0, 0x6d, 0x75, 0x48, 0xa6, 0x9f, 0x61, 0xad, 0x15, 0xea, 0x5b, 0x9e });
        /* {2827dedc-6db0-4875-a69f-61ad15ea5b9e} */


        /// <summary>
        ///     invalid range check directive
        /// </summary>
        public static readonly Guid InvalidRangeCheckDirective
            = new Guid(new byte[] { 0xf9, 0x92, 0x81, 0x3, 0x56, 0x43, 0x5d, 0x4c, 0x9d, 0xf1, 0x71, 0x6d, 0x79, 0x5d, 0xb1, 0x9f });
        /* {038192f9-4356-4c5d-9df1-716d795db19f} */

        /// <summary>
        ///    invalid stack frames directive
        /// </summary>
        public static readonly Guid InvalidStackFramesDirective
            = new Guid(new byte[] { 0x6d, 0xe5, 0xb3, 0xe, 0x45, 0xb7, 0xa2, 0x4f, 0x95, 0x85, 0x11, 0x16, 0xc4, 0x6, 0x4, 0xa0 });
        /* {0eb3e56d-b745-4fa2-9585-1116c40604a0} */


        /// <summary>
        ///     invalid zero based string directive
        /// </summary>
        public static readonly Guid InvalidZeroBasedStringsDirective
            = new Guid(new byte[] { 0xa5, 0x94, 0xc4, 0x9a, 0x8, 0xb3, 0xdf, 0x4a, 0x9e, 0xaa, 0xd2, 0x25, 0x33, 0x8b, 0x65, 0xd0 });
        /* {9ac494a5-b308-4adf-9eaa-d225338b65d0} */

        /// <summary>
        ///     invalid writeable constants directive
        /// </summary>
        public static readonly Guid InvalidWritableConstantsDirective
            = new Guid(new byte[] { 0x7c, 0xbd, 0x56, 0x8c, 0x76, 0xaf, 0x60, 0x42, 0x9a, 0x35, 0xee, 0x1f, 0x4a, 0x48, 0x9, 0xb0 });
        /* {8c56bd7c-af76-4260-9a35-ee1f4a4809b0} */


        /// <summary>
        ///     invalid weak link directive
        /// </summary>
        public static readonly Guid InvalidWeakLinkRttiDirective
            = new Guid(new byte[] { 0x2a, 0xa9, 0x19, 0xdd, 0x1a, 0x55, 0x19, 0x41, 0xa7, 0xfe, 0x1b, 0xc1, 0xfb, 0xdb, 0x9e, 0xf1 });
        /* {dd19a92a-551a-4119-a7fe-1bc1fbdb9ef1} */

        /// <summary>
        ///     invalid warnings directive
        /// </summary>
        public static readonly Guid InvalidWarningsDirective
            = new Guid(new byte[] { 0xb9, 0x1a, 0x56, 0xa0, 0x11, 0x94, 0x3c, 0x43, 0x89, 0x2c, 0x46, 0xde, 0xb9, 0x71, 0x91, 0x71 });
        /* {a0561ab9-9411-433c-892c-46deb9719171} */

        /// <summary>
        ///     invalid warn directive
        /// </summary>
        public static readonly Guid InvalidWarnDirective
            = new Guid(new byte[] { 0xa, 0xcb, 0x14, 0x65, 0xd6, 0xbb, 0xbd, 0x48, 0x88, 0x37, 0xbc, 0x7b, 0xf1, 0x81, 0xbe, 0xa8 });
        /* {6514cb0a-bbd6-48bd-8837-bc7bf181bea8} */


        /// <summary>
        ///     invalid string check directive
        /// </summary>
        public static readonly Guid InvalidStringCheckDirective
            = new Guid(new byte[] { 0x2f, 0xbe, 0x59, 0x6f, 0x17, 0x19, 0xb3, 0x44, 0x92, 0xf4, 0x67, 0x6a, 0x45, 0x96, 0x43, 0xb3 });
        /* {6f59be2f-1917-44b3-92f4-676a459643b3} */


        /// <summary>
        ///     invalid type checked pointer directive
        /// </summary>
        public static readonly Guid InvalidTypeCheckedPointersDirective
            = new Guid(new byte[] { 0x7a, 0x6e, 0x1b, 0x96, 0x6f, 0xa5, 0xf7, 0x48, 0xa7, 0x34, 0xe4, 0x4f, 0x38, 0x8b, 0xab, 0x5c });
        /* {961b6e7a-a56f-48f7-a734-e44f388bab5c} */

        /// <summary>
        ///     invlaid definition info switch
        /// </summary>
        public static readonly Guid InvalidDefinitionInfoDirective
            = new Guid(new byte[] { 0x56, 0x24, 0xc5, 0x99, 0x80, 0xb1, 0x76, 0x44, 0x95, 0xf, 0x10, 0x9, 0x6f, 0xf, 0xc8, 0x5e });
        /* {99c52456-b180-4476-950f-10096f0fc85e} */

        /// <summary>
        ///     invalid strong link types directive
        /// </summary>
        public static readonly Guid InvalidStrongLinkTypesDirective
            = new Guid(new byte[] { 0xda, 0xa6, 0xe2, 0xd7, 0x11, 0xdf, 0x8f, 0x43, 0xb8, 0x8e, 0xe6, 0xf6, 0x4e, 0xfe, 0x7a, 0xe4 });

        /* {d7e2a6da-df11-438f-b88e-e6f64efe7ae4} */


        /// <summary>
        ///     invalid scoped enums directive
        /// </summary>
        public static readonly Guid InvalidScopedEnumsDirective
         = new Guid(new byte[] { 0x49, 0x11, 0x34, 0x24, 0x4a, 0xa3, 0x4d, 0x40, 0xb0, 0x63, 0x55, 0xf2, 0x2e, 0x6f, 0x5d, 0x70 });
        /* {24341149-a34a-404d-b063-55f22e6f5d70} */


        /// <summary>
        ///     invalid published rtti directive
        /// </summary>
        public static readonly Guid InvalidPublishedRttiDirective
            = new Guid(new byte[] { 0x7f, 0xf9, 0x72, 0x63, 0xf7, 0xd4, 0xca, 0x49, 0x89, 0x84, 0x46, 0x6f, 0xe9, 0x18, 0x17, 0x4a });
        /* {6372f97f-d4f7-49ca-8984-466fe918174a} */


        /// <summary>
        ///     invalid run only directive
        /// </summary>
        public static readonly Guid InvalidRunOnlyDirective
            = new Guid(new byte[] { 0xe0, 0xb, 0xf2, 0x89, 0xb9, 0xdb, 0x35, 0x42, 0xb5, 0x9e, 0xc1, 0x4d, 0xf0, 0x13, 0x2c, 0x4 });
        /* {89f20be0-dbb9-4235-b59e-c14df0132c04} */


        /// <summary>
        ///     invalid legacy if end directive
        /// </summary>
        public static readonly Guid InvalidLegacyIfEndDirective
            = new Guid(new byte[] { 0x65, 0x66, 0x24, 0xec, 0x76, 0x15, 0x6, 0x45, 0xaf, 0x39, 0xe0, 0xf8, 0xe5, 0x87, 0x95, 0x17 });
        /* {ec246665-1576-4506-af39-e0f8e5879517} */


        /// <summary>
        ///     invalid legacy var prop setter directive
        /// </summary>
        public static readonly Guid InvalidVarPropSetterDirective
            = new Guid(new byte[] { 0x77, 0x1c, 0x53, 0x2, 0xd8, 0x2, 0x63, 0x4a, 0xb9, 0x8f, 0x3a, 0xbb, 0x9c, 0x3a, 0x87, 0xf3 });
        /* {02531c77-02d8-4a63-b98f-3abb9c3a87f3} */

        /// <summary>
        ///     invalid real48 mode
        /// </summary>
        public static readonly Guid InvalidRealCompatibilityMode
            = new Guid(new byte[] { 0xd, 0xda, 0xb7, 0xeb, 0xa7, 0xa1, 0x30, 0x42, 0xa9, 0x2, 0xb8, 0x73, 0x33, 0xfe, 0xa8, 0xde });
        /* {ebb7da0d-a1a7-4230-a902-b87333fea8de} */


        /// <summary>
        ///     invalid pointermath directive
        /// </summary>
        public static readonly Guid InvalidPointerMathDirective
            = new Guid(new byte[] { 0xe2, 0x8, 0x42, 0xda, 0xf7, 0xbe, 0xa5, 0x43, 0x94, 0x19, 0x4f, 0x4e, 0xb0, 0xd1, 0xac, 0xa1 });
        /* {da4208e2-bef7-43a5-9419-4f4eb0d1aca1} */


        /// <summary>
        ///     invalid old type layout directive
        /// </summary>
        public static readonly Guid InvalidOldTypeLayoutDirective
            = new Guid(new byte[] { 0x88, 0x13, 0x43, 0x23, 0xb8, 0xa6, 0xf1, 0x43, 0xb0, 0xe, 0x25, 0xcd, 0x24, 0xc2, 0x38, 0xed });
        /* {23431388-a6b8-43f1-b00e-25cd24c238ed} */


        /// <summary>
        ///     invalid no define directive
        /// </summary>
        public static readonly Guid InvalidNoDefineDirective
            = new Guid(new byte[] { 0x12, 0xf6, 0x95, 0x28, 0x2f, 0x28, 0xb8, 0x4d, 0xa5, 0x58, 0x68, 0xeb, 0x30, 0x74, 0x5, 0xe6 });
        /* {2895f612-282f-4db8-a558-68eb307405e6} */


        /// <summary>
        ///     invalid object type directive
        /// </summary>
        public static readonly Guid InvalidObjTypeDirective
         = new Guid(new byte[] { 0xff, 0x9, 0x86, 0x10, 0xab, 0x4f, 0xfe, 0x42, 0x97, 0x6d, 0x8b, 0xc, 0x29, 0x90, 0xf6, 0x71 });
        /* {108609ff-4fab-42fe-976d-8b0c2990f671} */


        /// <summary>
        ///     invalid no include directive
        /// </summary>
        public static readonly Guid InvalidNoIncludeDirective
             = new Guid(new byte[] { 0xde, 0xd3, 0x51, 0xe1, 0xa0, 0x5, 0x6, 0x4d, 0x99, 0xf4, 0x8b, 0x46, 0x2c, 0x24, 0x88, 0x1f });
        /* {e151d3de-05a0-4d06-99f4-8b462c24881f} */


        /// <summary>
        ///     invalid min enum size
        /// </summary>
        public static readonly Guid InvalidMinEnumSizeDirective
            = new Guid(new byte[] { 0x46, 0x5b, 0xd1, 0x47, 0x82, 0x6, 0x76, 0x40, 0x90, 0x36, 0x75, 0x69, 0xa, 0x6b, 0xd7, 0x85 });
        /* {47d15b46-0682-4076-9036-75690a6bd785} */


        /// <summary>
        ///     invalidmethod info
        /// </summary>
        public static readonly Guid InvalidMethodInfoDirective
             = new Guid(new byte[] { 0x33, 0x2e, 0x1, 0xee, 0x94, 0xe2, 0xe1, 0x45, 0x93, 0xd3, 0x0, 0xbc, 0xb4, 0xc0, 0xe2, 0x3c });
        /* {ee012e33-e294-45e1-93d3-00bcb4c0e23c} */


        /// <summary>
        ///     invalid lib directive
        /// </summary>
        public static readonly Guid InvalidLibDirective
                = new Guid(new byte[] { 0xbd, 0x26, 0xb2, 0x9c, 0x4c, 0xfd, 0xe0, 0x49, 0xbc, 0x6a, 0xce, 0x7a, 0x59, 0x6b, 0xdf, 0x37 });
        /* {9cb226bd-fd4c-49e0-bc6a-ce7a596bdf37} */


        /// <summary>
        ///     invalid pe version directive
        /// </summary>
        public static readonly Guid InvalidPEVersionDirective
            = new Guid(new byte[] { 0x58, 0x27, 0x74, 0x9b, 0x84, 0xb8, 0x1c, 0x4f, 0xb4, 0xe0, 0x7c, 0x77, 0xdb, 0xbe, 0x1d, 0x4f });
        /* {9b742758-b884-4f1c-b4e0-7c77dbbe1d4f} */


        /// <summary>
        ///     invalid region directive
        /// </summary>
        public static readonly Guid InvalidRegionDirective
            = new Guid(new byte[] { 0x83, 0xe5, 0x71, 0xfb, 0x10, 0x98, 0xc5, 0x43, 0xbe, 0xf4, 0xc5, 0xb3, 0x38, 0x59, 0x37, 0xc0 });
        /* {fb71e583-9810-43c5-bef4-c5b3385937c0} */


        /// <summary>
        ///     end region without region
        /// </summary>
        public static readonly Guid EndRegionWithoutRegion
            = new Guid(new byte[] { 0xbb, 0xc6, 0x49, 0x81, 0xa5, 0xff, 0xa1, 0x41, 0xae, 0xac, 0xe4, 0x96, 0xd3, 0xd1, 0xf6, 0xbd });
        /* {8149c6bb-ffa5-41a1-aeac-e496d3d1f6bd} */


        /// <summary>
        ///     invalid weak package unit directive
        /// </summary>
        public static readonly Guid InvalidWeakPackageUnitDirective
            = new Guid(new byte[] { 0x68, 0x75, 0xef, 0x8c, 0xe6, 0xe4, 0x95, 0x44, 0x8b, 0xa3, 0xf7, 0xc9, 0x8, 0x7e, 0xe8, 0x4a });
        /* {8cef7568-e4e6-4495-8ba3-f7c9087ee84a} */


        /// <summary>
        ///     invalid rtti directive
        /// </summary>
        public static readonly Guid InvalidRttiDirective
            = new Guid(new byte[] { 0x5a, 0x79, 0x9a, 0x34, 0x6e, 0x48, 0xee, 0x43, 0x87, 0x3, 0x85, 0x3a, 0xd8, 0x24, 0x62, 0xeb });
        /* {349a795a-486e-43ee-8703-853ad82462eb} */


        /// <summary>
        ///     invalid imported data directive
        /// </summary>
        public static readonly Guid InvalidImportedDataDirective
            = new Guid(new byte[] { 0x0, 0x65, 0x6f, 0xd0, 0xcc, 0xfc, 0x72, 0x42, 0x98, 0xfe, 0x3b, 0x22, 0xe9, 0xd9, 0xf8, 0x7b });
        /* {d06f6500-fccc-4272-98fe-3b22e9d9f87b} */


        /// <summary>
        ///     invalid message directive
        /// </summary>
        public static readonly Guid InvalidMessageDirective
            = new Guid(new byte[] { 0x8, 0x7d, 0x1e, 0xe, 0x1c, 0x29, 0x24, 0x4b, 0x9b, 0x45, 0x7a, 0x9e, 0x37, 0x72, 0xcb, 0x5f });
        /* {0e1e7d08-291c-4b24-9b45-7a9e3772cb5f} */


        /// <summary>
        ///     invalid stack mem size directive
        /// </summary>
        public static readonly Guid InvalidStackMemorySizeDirective
            = new Guid(new byte[] { 0x7e, 0xeb, 0x9, 0x0, 0x7f, 0xc3, 0xd6, 0x44, 0x92, 0xb8, 0x1a, 0xd7, 0x2d, 0x99, 0xf0, 0x2b });
        /* {0009eb7e-c37f-44d6-92b8-1ad72d99f02b} */


        /// <summary>
        ///     invalid file name
        /// </summary>
        public static readonly Guid InvalidFileName
            = new Guid(new byte[] { 0xbe, 0xcd, 0xe1, 0xf3, 0x3c, 0xef, 0x8d, 0x4a, 0x80, 0x44, 0x2a, 0xa5, 0x53, 0x56, 0x2e, 0x5 });
        /* {f3e1cdbe-ef3c-4a8d-8044-2aa553562e05} */


        /// <summary>
        ///     invalid link directive
        /// </summary>
        public static readonly Guid InvalidLinkDirective
            = new Guid(new byte[] { 0xe9, 0x77, 0x14, 0x58, 0xd6, 0x0, 0xf, 0x46, 0xbe, 0x2d, 0x79, 0x8a, 0xde, 0xc9, 0x91, 0xa5 });
        /* {581477e9-00d6-460f-be2d-798adec991a5} */


        /// <summary>
        ///     invalid resource directive
        /// </summary>
        public static readonly Guid InvalidResourceDirective
            = new Guid(new byte[] { 0xbb, 0x35, 0xb0, 0x3b, 0xa5, 0x26, 0xf, 0x43, 0xa3, 0xa7, 0xad, 0x4f, 0x2a, 0xd, 0xbc, 0xd0 });
        /* {3bb035bb-26a5-430f-a3a7-ad4f2a0dbcd0} */


        /// <summary>
        ///     invalid include directive
        /// </summary>
        public static readonly Guid InvalidIncludeDirective
            = new Guid(new byte[] { 0x63, 0x99, 0x54, 0x30, 0x2b, 0x1, 0xc5, 0x4e, 0xa5, 0xeb, 0xda, 0x51, 0xb2, 0x52, 0xb5, 0x33 });
        /* {30549963-012b-4ec5-a5eb-da51b252b533} */

    }
}