using System;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     structural error messages
    /// </summary>
    public static class StructuralErrors {

        /// <summary>
        ///     duplicate unit name
        /// </summary>
        public static readonly Guid DuplicateUnitName
            = new Guid(new byte[] { 0xd6, 0x79, 0x68, 0xf3, 0xe5, 0x75, 0xfe, 0x45, 0x9a, 0xee, 0xc2, 0x77, 0x34, 0xdd, 0xce, 0x53 });
        /* {f36879d6-75e5-45fe-9aee-c27734ddce53} */


        /// <summary>
        ///     unit name does not match filename
        /// </summary>
        public static readonly Guid UnitNameDoesNotMatchFileName
            = new Guid(new byte[] { 0xb7, 0x9e, 0x4f, 0x1a, 0xbf, 0x3b, 0xa6, 0x40, 0x89, 0xc1, 0x6a, 0x78, 0x31, 0x89, 0x53, 0xac });
        /* {1a4f9eb7-3bbf-40a6-89c1-6a78318953ac} */


        /// <summary>
        ///     duplicated unit name
        /// </summary>
        public static readonly Guid RedeclaredUnitNameInUsesList
            = new Guid(new byte[] { 0x72, 0x6a, 0x2c, 0x2b, 0x16, 0x5e, 0xef, 0x44, 0xb9, 0xd4, 0x53, 0x62, 0x5a, 0x63, 0x89, 0xac });
        /* {2b2c6a72-5e16-44ef-b9d4-53625a6389ac} */


        /// <summary>
        ///     redeclared symbol
        /// </summary>
        public static readonly Guid RedeclaredSymbol
             = new Guid(new byte[] { 0x2c, 0xc9, 0x36, 0xbd, 0xcf, 0xba, 0x2c, 0x49, 0x8b, 0xf8, 0x17, 0xe8, 0x90, 0xcb, 0xaa, 0xfa });
        /* {bd36c92c-bacf-492c-8bf8-17e890cbaafa} */

        /// <summary>
        ///     redeclared enum name
        /// </summary>
        public static readonly Guid RedeclaredEnumName
            = new Guid(new byte[] { 0x2e, 0x96, 0x44, 0x96, 0x9c, 0xcb, 0x35, 0x44, 0x9e, 0x9, 0x8f, 0x54, 0x6f, 0xfc, 0xab, 0xc6 });
        /* {9644962e-cb9c-4435-9e09-8f546ffcabc6} */


        /// <summary>
        ///     invalid type of construct
        /// </summary>
        public static readonly Guid UnsupportedTypeOfConstruct
            = new Guid(new byte[] { 0x94, 0xc4, 0xb9, 0xeb, 0xcb, 0x1c, 0xc, 0x4e, 0xb7, 0x21, 0x51, 0xd9, 0x37, 0xf3, 0xd2, 0xe3 });

        /// <summary>
        ///     duplacate parameter name
        /// </summary>
        internal static readonly Guid DuplicateParameterName
             = new Guid(new byte[] { 0x60, 0xd6, 0x95, 0xc7, 0xa4, 0xc7, 0xce, 0x4b, 0x8a, 0xf4, 0x26, 0x23, 0x52, 0x95, 0xbf, 0x3d });
        /* {c795d660-c7a4-4bce-8af4-26235295bf3d} */

    }
}
