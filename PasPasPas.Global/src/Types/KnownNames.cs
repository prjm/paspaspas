namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     well-known type names
    /// </summary>
    public static class KnownNames {

        #region Types

        /// <summary>
        ///    short int type name
        /// </summary>
        public const string ShortInt = "ShortInt";

        /// <summary>
        ///     byte type name
        /// </summary>
        public const string Byte = "Byte";

        /// <summary>
        ///     system type
        /// </summary>
        public const string System = "System";

        /// <summary>
        ///     internal main method name
        /// </summary>
        public const string MainMethod = "<>main";

        /// <summary>
        ///     WriteLn method name
        /// </summary>
        public const string WriteLn = "WriteLn";

        /// <summary>
        ///     small int type
        /// </summary>
        public const string SmallInt = "SmallInt";

        /// <summary>
        ///     word type
        /// </summary>
        public const string Word = "Word";

        /// <summary>
        ///     cardinal type
        /// </summary>
        public const string Cardinal = "Cardinal";

        /// <summary>
        ///     integer type
        /// </summary>
        public const string Integer = "Integer";

        /// <summary>
        ///     int64 type name
        /// </summary>
        public const string Int64 = "Int64";

        /// <summary>
        ///     uint64 type name
        /// </summary>
        public const string UInt64 = "UInt64";

        /// <summary>
        ///     type alias for byte
        /// </summary>
        public const string UInt8 = "UInt8";

        /// <summary>
        ///     type alias for small int
        /// </summary>
        public const string Int8 = "Int8";

        /// <summary>
        ///     type alias for words
        /// </summary>
        public const string UInt16 = "UInt16";

        /// <summary>
        ///     type alias for short int
        /// </summary>
        public const string Int16 = "Int16";

        /// <summary>
        ///  type alias for cardinal
        /// </summary>
        public const string UInt32 = "UInt32";

        /// <summary>
        ///     type alias for integer
        /// </summary>
        public const string Int32 = "Int32";

        /// <summary>
        ///     boolean type
        /// </summary>
        public const string Boolean = "Boolean";

        /// <summary>
        ///     byte-sized boolean
        /// </summary>
        public const string ByteBool = "ByteBool";

        /// <summary>
        ///     word-sized boolean
        /// </summary>
        public const string WordBool = "WordBool";

        /// <summary>
        ///     double word size boolean
        /// </summary>
        public const string LongBool = "LongBool";

        /// <summary>
        ///     fixed integer type
        /// </summary>
        public const string FixedUInt = "FixedUInt";

        /// <summary>
        ///     fixed unsigned integer type
        /// </summary>
        public const string FixedInt = "FixedInt";

        /// <summary>
        ///     native integers
        /// </summary>
        public const string NativeInt = "NativeInt";

        /// <summary>
        ///     native unsigned integers
        /// </summary>
        public const string NativeUInt = "NativeUInt";

        /// <summary>
        ///     long int
        /// </summary>
        public const string LongInt = "LongInt";

        /// <summary>
        ///     long word
        /// </summary>
        public const string LongWord = "LongWord";

        /// <summary>
        ///     real 48 type
        /// </summary>
        public const string Real48 = "Real48";

        /// <summary>
        ///     single type
        /// </summary>
        public const string Single = "Single";

        /// <summary>
        ///     double type
        /// </summary>
        public const string Double = "Double";

        /// <summary>
        ///     comp type
        /// </summary>
        public const string Comp = "Comp";

        /// <summary>
        ///     currency type
        /// </summary>
        public const string Currency = "Currency";

        /// <summary>
        ///     extended type
        /// </summary>
        public const string Extended = "Extended";

        /// <summary>
        ///     ANSI char type
        /// </summary>
        public const string AnsiChar = "AnsiChar";

        /// <summary>
        ///     wide char type
        /// </summary>
        public const string WideChar = "WideChar";

        /// <summary>
        ///     ANSI string type
        /// </summary>
        public const string AnsiString = "AnsiString";

        /// <summary>
        ///     raw byte string
        /// </summary>
        public const string RawByteString = "RawByteString";

        /// <summary>
        ///     short string
        /// </summary>
        public const string ShortString = "ShortString";

        /// <summary>
        ///     UNICODE string
        /// </summary>
        public const string UnicodeString = "UnicodeString";

        /// <summary>
        ///     wide string
        /// </summary>
        public const string WideString = "WideString";

        /// <summary>
        ///     pointer type
        /// </summary>
        public const string Pointer = "Pointer";

        /// <summary>
        ///     pointer to a byte value
        /// </summary>
        public const string PByte = "PByte";

        /// <summary>
        ///     pointer to short int type
        /// </summary>
        public const string PShortInt = "PShortInt";

        /// <summary>
        ///     pointer to word type
        /// </summary>
        public const string PWord = "PWord";

        /// <summary>
        ///     pointer to small int type
        /// </summary>
        public const string PSmallInt = "PSmallInt";

        /// <summary>
        ///     pointer to cardinal type
        /// </summary>
        public const string PCardinal = "PCardinal";

        /// <summary>
        ///     pointer to longword
        /// </summary>
        public const string PLongword = "PLongword";

        /// <summary>
        ///     pointer to fixed unsigned integer
        /// </summary>
        public const string PFixedUint = "PFixedUInt";

        /// <summary>
        ///     pointer to an integer
        /// </summary>
        public const string PInteger = "PInteger";

        /// <summary>
        ///     pointer to a long integer
        /// </summary>
        public const string PLongInt = "PLongInt";

        /// <summary>
        ///     pointer to a fixed integer
        /// </summary>
        public const string PFixedInt = "PFixedInt";

        /// <summary>
        ///     pointer to 64 bit unsigned integer
        /// </summary>
        public const string PUInt64 = "PUInt64";

        /// <summary>
        ///     pointer to 64 bit integer
        /// </summary>
        public const string PInt64 = "PInt64";

        /// <summary>
        ///     pointer to a native integer
        /// </summary>
        public const string PNativeInt = "PNativeInt";

        /// <summary>
        ///     pointer to a native unsigned integer
        /// </summary>
        public const string PNativeUInt = "PNativeUInt";

        /// <summary>
        ///     pointer to a single floating point value
        /// </summary>
        public const string PSingle = "PSingle";

        /// <summary>
        ///     pointer to a double floating point value
        /// </summary>
        public const string PDouble = "PDouble";

        /// <summary>
        ///     pointer to a extended floating point value
        /// </summary>
        public const string PExtended = "PExtended";

        /// <summary>
        ///     pointer to an ANSI char
        /// </summary>
        public const string PAnsiChar = "PAnsiChar";

        /// <summary>
        ///     pointer to a wide char
        /// </summary>
        public const string PWideChar = "PWideChar";

        /// <summary>
        ///     pointer to a boolean
        /// </summary>
        public const string PBoolean = "PBoolean";

        /// <summary>
        ///     pointer to a long boolean
        /// </summary>
        public const string PLongBool = "PLongBool";

        /// <summary>
        ///     pointer to a byte boolean
        /// </summary>
        public const string PByteBool = "PByteBool";

        /// <summary>
        ///     pointer to a word boolean
        /// </summary>
        public const string PWordBool = "PWordBool";

        /// <summary>
        ///     generic pointer to a pointer
        /// </summary>
        public const string PPointer = "PPointer";

        /// <summary>
        ///     pointer to a currency value
        /// </summary>
        public const string PCurrency = "PCurrency";

        /// <summary>
        ///     pointer to a wide string
        /// </summary>
        public const string PWideString = "PWideString";

        /// <summary>
        ///     pointer to a raw byte string
        /// </summary>
        public const string PRawByteString = "PRawByteString";

        /// <summary>
        ///     pointer to a UNICODE string
        /// </summary>
        public const string PUnicodeString = "PUnicodeString";

        /// <summary>
        ///     pointer to short string
        /// </summary>
        public const string PShortString = "PShortString";

        /// <summary>
        ///     pointer to ANIS string
        /// </summary>
        public const string PAnsiString = "PAnsiString";

        /// <summary>
        ///     error type name
        /// </summary>
        public const string Error = "Error";

        /// <summary>
        ///     char type
        /// </summary>
        public const string Char = "Char";

        /// <summary>
        ///     ucs2 char type
        /// </summary>
        public const string Ucs2Char = "UCS2Char";

        /// <summary>
        ///     ucs4 char type
        /// </summary>
        public const string Ucs4Char = "UCS4Char";

        /// <summary>
        ///     string type
        /// </summary>
        public const string String = "String";

        /// <summary>
        ///     pointer to a char type
        /// </summary>
        public const string PChar = "PChar";

        /// <summary>
        ///     pointer to a string type
        /// </summary>
        public const string PString = "PString";

        /// <summary>
        ///     real type
        /// </summary>
        public const string Real = "Real";

        /// <summary>
        ///     generic array type
        /// </summary>
        public const string TArray = "TArray";

        /// <summary>
        ///     generic file type
        /// </summary>
        public const string File = "File";

        #endregion
        #region Short Type names

        /// <summary>
        ///     shorthand name for short ints
        /// </summary>
        public const string ZC = "zc";

        /// <summary>
        ///     shorthand name for bytes
        /// </summary>
        public const string UC = "uc";

        /// <summary>
        ///     shorthand for word type
        /// </summary>
        public const string US = "us";

        /// <summary>
        ///     shorthand for small int type
        /// </summary>
        public const string S = "s";

        /// <summary>
        ///     shorthand for integer type
        /// </summary>
        public const string I = "i";

        /// <summary>
        ///     shorthand for cardinal type
        /// </summary>
        public const string UI = "ui";

        /// <summary>
        ///     shorthand for int64 type
        /// </summary>
        public const string J = "j";

        /// <summary>
        ///     shorthand for uint64 type
        /// </summary>
        public const string UJ = "uj";

        /// <summary>
        ///     shorthand for boolean type
        /// </summary>
        public const string O = "o";

        /// <summary>
        ///     shorthand for real48 type
        /// </summary>
        public const string SReal48 = "6real48";

        /// <summary>
        ///     short hand for single precision floating point type
        /// </summary>
        public const string F = "f";

        /// <summary>
        ///     short hand for double precision floating point type
        /// </summary>
        public const string D = "d";

        /// <summary>
        ///     shorthand for comp data type
        /// </summary>
        public const string SystemAtComp = "System@Comp";

        /// <summary>
        ///     shorthand for currency data type
        /// </summary>
        public const string SystemAtCurrency = "System@Currency";

        /// <summary>
        ///     shorthand for extended
        /// </summary>
        public const string G = "g";

        /// <summary>
        ///     shorthand for ANSI char
        /// </summary>
        public const string C = "c";

        /// <summary>
        ///     shorthand for wide char
        /// </summary>
        public const string B = "b";

        /// <summary>
        ///     shorthand for wide string
        /// </summary>
        public const string SystemAtWideString = "System@WideString";

        /// <summary>
        ///     shorthand for UNICODE string
        /// </summary>
        public const string SystemAtUnicodeString = "System@UnicodeString";

        /// <summary>
        ///     shorthand for generic pointer type
        /// </summary>
        public const string PV = "pv";

        /// <summary>
        ///     shorthand for pointer type
        /// </summary>
        public const string P = "p";

        #endregion

        /// <summary>
        ///     at symbol
        /// </summary>
        public const string AtSymbol = "@";

    }
}
