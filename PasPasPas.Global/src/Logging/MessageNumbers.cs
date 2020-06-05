#nullable disable
namespace PasPasPas.Globals.Log {

    /// <summary>
    ///     message numbers
    /// </summary>
    public static class MessageNumbers {

        /// <summary>
        ///     default number format
        /// </summary>
        public const string NumberFormat = "X4";

        /// <summary>
        ///     message: unexpected token message id
        /// </summary>
        public const uint UnexpectedToken
            = 0x0101;

        /// <summary>
        ///     message: user generated message id
        /// </summary>
        public const uint UserGeneratedMessage
            = 0x0102;

        /// <summary>
        ///     message: missing token error message id
        /// </summary>
        public const uint MissingToken
            = 0x0103;

        /// <summary>
        ///     missing file
        /// </summary>
        public const uint MissingFile
            = 0x104;

        /// <summary>
        ///     open an included file for parsing
        /// </summary>
        public const uint OpenFileFromUses
            = 0x105;

        /// <summary>
        ///     error code for a open <c>ifdef</c> / <c>ifndef</c>
        /// </summary>
        public const uint PendingCondition
            = 0x0301;

        /// <summary>
        ///     pending region
        /// </summary>
        public const uint PendingRegion
            = 0x0302;

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
        ///     redeclared enumeration name
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

        /// <summary>
        ///     message: unexpected token
        /// </summary>
        public const uint UnexpectedCharacter
            = 0x0390;

        /// <summary>
        ///     message: unexpected end of token
        /// </summary>
        /// <remarks>
        ///     data: expected-token-end sequence
        /// </remarks>
        public const uint UnexpectedEndOfToken
            = 0x0391;

        /// <summary>
        ///     message id: incomplete hex number
        /// </summary>
        public const uint IncompleteHexNumber
            = 0x0392;

        /// <summary>
        ///     message id: incomplete identifier
        /// </summary>
        public const uint IncompleteIdentifier
            = 0x0393;

        /// <summary>
        ///     message id: incomplete string
        /// </summary>
        public const uint IncompleteString
            = 0x0394;


        /// <summary>
        ///     invalid file format
        /// </summary>
        public const uint InvalidFileFormat
            = 0x0700;
    }
}
