using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     common type ids
    /// </summary>
    public static class TypeIds {

        /// <summary>
        ///     no type
        /// </summary>
        public const int NoType = -4;

        /// <summary>
        ///     system unit
        /// </summary>
        public const int SystemUnit = -3;

        /// <summary>
        ///     unspecified type
        /// </summary>
        public const int UnspecifiedType = -2;

        /// <summary>
        ///     errornous type
        /// </summary>
        public const int ErrorType = Signature.ErrorType;

        /// <summary>
        ///     byte type (1 byte unsigned integer)
        /// </summary>
        public const int ByteType = 1;

        /// <summary>
        ///     word type (2 byte unsigned integer)
        /// </summary>
        public const int WordType = 2;

        /// <summary>
        ///     cardinal type (4 byte unsigned integer)
        /// </summary>
        public const int CardinalType = 3;

        /// <summary>
        ///     unit64 type (8 byte unsigned integer)
        /// </summary>
        public const int Uint64Type = 4;

        /// <summary>
        ///     boolean type
        /// </summary>
        public const int BooleanType = 5;

        /// <summary>
        ///     wide char type
        /// </summary>
        public const int WideCharType = 6;

        /// <summary>
        ///     string type
        /// </summary>
        public const int StringType = 7;

        /// <summary>
        ///     extended real type
        /// </summary>
        public const int Extended = 8;

        /// <summary>
        ///     integer type
        /// </summary>
        public const int IntegerType = 9;

        /// <summary>
        ///     unicode string type
        /// </summary>
        public const int UnicodeStringType = 10;

        /// <summary>
        ///     ansi char type
        /// </summary>
        public const int AnsiCharType = 11;

        /// <summary>
        ///     signed int64 type
        /// </summary>
        public const int Int64Type = 12;

        /// <summary>
        ///     short integer type (1 byte signed)
        /// </summary>
        public const int ShortInt = 13;

        /// <summary>
        ///     small int type (2 byte signed)
        /// </summary>
        public const int SmallInt = 14;

        /// <summary>
        ///     char type
        /// </summary>
        public const int CharType = 15;

        /// <summary>
        ///     ucs2 char type (type alias for widechar)
        /// </summary>
        public const int Ucs2CharType = 16;

        /// <summary>
        ///     ucs4 char type (type alias for integer)
        /// </summary>
        public const int Ucs4CharType = 17;

        /// <summary>
        ///     1 byte boolean
        /// </summary>
        public const int ByteBoolType = 18;

        /// <summary>
        ///     2 byte boolean
        /// </summary>
        public const int WordBoolType = 19;

        /// <summary>
        ///     4 byte boolean
        /// </summary>
        public const int LongBoolType = 20;

        /// <summary>
        ///     native int
        /// </summary>
        public const int NativeInt = 21;

        /// <summary>
        ///     native uint
        /// </summary>
        public const int NativeUInt = 22;

        /// <summary>
        ///     native long int
        /// </summary>
        public const int LongInt = 23;

        /// <summary>
        ///     native long word
        /// </summary>
        public const int LongWord = 24;

        /// <summary>
        ///     wide string type
        /// </summary>
        public const int WideStringType = 25;

        /// <summary>
        ///     short string type
        /// </summary>
        public const int ShortStringType = 26;

        /// <summary>
        ///     ansi string type
        /// </summary>
        public const int AnsiStringType = 27;

        /// <summary>
        ///     real46 type
        /// </summary>
        public const int Real48Type = 28;

        /// <summary>
        ///     single precision type
        /// </summary>
        public const int SingleType = 29;

        /// <summary>
        ///     double precisision type
        /// </summary>
        public const int Double = 30;

        /// <summary>
        ///     real type
        /// </summary>
        public const int Real = 31;

        /// <summary>
        ///     comp type
        /// </summary>
        public const int Comp = 32;

        /// <summary>
        ///     current type
        /// </summary>
        public const int Currency = 33;

        /// <summary>
        ///     fixed int
        /// </summary>
        public const int FixedInt = 34;

        /// <summary>
        ///     fixed uint
        /// </summary>
        public const int FixedUInt = 35;

        /// <summary>
        ///     generic pointer
        /// </summary>
        public const int GenericPointer = 36;

        /// <summary>
        ///     pointer to byte
        /// </summary>
        public const int PByte = 37;

        /// <summary>
        ///     pointer to short int
        /// </summary>
        public const int PShortInt = 38;

        /// <summary>
        ///     pointer to word
        /// </summary>
        public const int PWord = 39;

        /// <summary>
        ///     pointer to small int
        /// </summary>
        public const int PSmallInt = 40;

        /// <summary>
        ///     pointer to cardinal
        /// </summary>
        public const int PCardinal = 41;

        /// <summary>
        ///     pointer to longword
        /// </summary>
        public const int PLongword = 42;

        /// <summary>
        ///     pointer to fixed uint
        /// </summary>
        public const int PFixedUint = 43;

        /// <summary>
        ///     pointer to integer
        /// </summary>
        public const int PInteger = 44;

        /// <summary>
        ///     pointer to long int
        /// </summary>
        public const int PLongInt = 45;

        /// <summary>
        ///     pointer to fixed size int
        /// </summary>
        public const int PFixedInt = 46;

        /// <summary>
        ///     pointer to unsigned int64
        /// </summary>
        public const int PUInt64 = 47;

        /// <summary>
        ///     pointer to int64
        /// </summary>
        public const int PInt64 = 48;

        /// <summary>
        ///     pointer to native uint
        /// </summary>
        public const int PNativeUInt = 49;

        /// <summary>
        ///     pointer to native int
        /// </summary>
        public const int PNativeInt = 50;

        /// <summary>
        ///     pointer to single
        /// </summary>
        public const int PSingle = 51;

        /// <summary>
        ///     pointer to double
        /// </summary>
        public const int PDouble = 52;

        /// <summary>
        ///     pointer to extended
        /// </summary>
        public const int PExtended = 53;

        /// <summary>
        ///     raw byte string
        /// </summary>
        public const int RawByteString = 54;

        /// <summary>
        ///     pointer to ansi char
        /// </summary>
        public const int PAnsiChar = 55;

        /// <summary>
        ///     ponter to wide char
        /// </summary>
        public const int PWideChar = 56;

        /// <summary>
        ///     pointer to ansi string
        /// </summary>
        public const int PAnsiString = 57;

        /// <summary>
        ///     pointer to raw byte string
        /// </summary>
        public const int PRawByteString = 58;

        /// <summary>
        ///     pointer to unicode string
        /// </summary>
        public const int PUnicodeString = 59;

        /// <summary>
        ///     pointer to short string
        /// </summary>
        public const int PShortString = 60;

        /// <summary>
        ///     pointer to wide string
        /// </summary>
        public const int PWideString = 61;

        /// <summary>
        ///     pointer to char
        /// </summary>
        public const int PChar = 62;

        /// <summary>
        ///     pointer to string
        /// </summary>
        public const int PString = 63;

        /// <summary>
        ///     pointer to boolean
        /// </summary>
        public const int PBoolean = 64;

        /// <summary>
        ///     pointer to long boolean
        /// </summary>
        public const int PLongBool = 65;

        /// <summary>
        ///     pointer to word boolean
        /// </summary>
        public const int PWordBool = 66;

        /// <summary>
        ///     pointer to a pointer
        /// </summary>
        public const int PPointer = 67;

        /// <summary>
        ///     pointer to a currency value
        /// </summary>
        public const int PCurrency = 68;

        /// <summary>
        ///     root object definition
        /// </summary>
        public const int TObject = 100;

        /// <summary>
        ///     root object metaclass
        /// </summary>
        public const int TClass = 101;

        /// <summary>
        ///     untyped pointer
        /// </summary>
        public const int UntypedPointer = 103;
    }
}
