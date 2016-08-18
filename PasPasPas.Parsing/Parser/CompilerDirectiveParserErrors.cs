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

    }
}
