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

    }
}
