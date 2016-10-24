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




    }
}
