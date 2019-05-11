namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     structural error messages
    /// </summary>
    public static class StructuralErrors {

        /// <summary>
        ///     duplicate unit name
        /// </summary>
        public const uint DuplicateUnitName
            = 0x0350;

        /// <summary>
        ///     unit name does not match filename
        /// </summary>
        public const uint UnitNameDoesNotMatchFileName
            = 0x0351;


        /// <summary>
        ///     duplicated unit name
        /// </summary>
        public const uint RedeclaredUnitNameInUsesList
            = 0x0352;


        /// <summary>
        ///     redeclared symbol
        /// </summary>
        public const uint RedeclaredSymbol
             = 0x0353;

        /// <summary>
        ///     redeclared enum name
        /// </summary>
        public const uint RedeclaredEnumName
            = 0x0354;


        /// <summary>
        ///     invalid type of construct
        /// </summary>
        public const uint UnsupportedTypeOfConstruct
            = 0x0355;

        /// <summary>
        ///     duplicate parameter name
        /// </summary>
        public const uint DuplicateParameterName
             = 0x0356;

        /// <summary>
        ///     duplicate field name
        /// </summary>
        public const uint DuplicateFieldName
            = 0x0357;


    }
}
