namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     system unit
    /// </summary>
    public interface ISystemUnit : IUnitType {

        /// <summary>
        ///     byte type
        /// </summary>
        IIntegralType ByteType { get; }

        /// <summary>
        ///     short int type
        /// </summary>
        IIntegralType ShortIntType { get; }

        /// <summary>
        ///     word type
        /// </summary>
        IIntegralType WordType { get; }

        /// <summary>
        ///     small int type
        /// </summary>
        IIntegralType SmallIntType { get; }

        /// <summary>
        ///     cardinal type
        /// </summary>
        IIntegralType CardinalType { get; }

        /// <summary>
        ///     4-byte integer type
        /// </summary>
        IIntegralType IntegerType { get; }

        /// <summary>
        ///     error type
        /// </summary>
        ITypeDefinition ErrorType { get; }

        /// <summary>
        /// s   boolean type definition
        /// </summary>
        IBooleanType BooleanType { get; }

        /// <summary>
        ///     wide char type
        /// </summary>
        ICharType WideCharType { get; }

        /// <summary>
        ///     ANSI char type
        /// </summary>
        ICharType AnsiCharType { get; }

        /// <summary>
        ///     UNICODE string type
        /// </summary>
        IStringType UnicodeStringType { get; }

        /// <summary>
        ///     ANSI string type
        /// </summary>
        IStringType AnsiStringType { get; }

        /// <summary>
        ///     short string type
        /// </summary>
        IStringType ShortStringType { get; }

        /// <summary>
        ///     extended type
        /// </summary>
        IRealType ExtendedType { get; }

        /// <summary>
        ///     int64 type definition
        /// </summary>
        IIntegralType Int64Type { get; }

        /// <summary>
        ///     uint64 type definition
        /// </summary>
        IIntegralType UInt64Type { get; }

        /// <summary>
        ///     native integer type
        /// </summary>
        IAliasedType NativeIntType { get; }

        /// <summary>
        ///     generic pointer type
        /// </summary>
        ITypeDefinition GenericPointerType { get; }

        /// <summary>
        ///     nil type
        /// </summary>
        ITypeDefinition NilType { get; }

        /// <summary>
        ///     wide string type
        /// </summary>
        IStringType WideStringType { get; }

        /// <summary>
        ///     fixed integer type
        /// </summary>
        IAliasedType FixedIntType { get; }

        /// <summary>
        ///     fixed unsigned integer type
        /// </summary>
        IAliasedType FixedUIntType { get; }

        /// <summary>
        ///     native unsigned integer type
        /// </summary>
        IAliasedType NativeUIntType { get; }

        /// <summary>
        ///     long integer types
        /// </summary>
        IAliasedType LongIntType { get; }

        /// <summary>
        ///     single type
        /// </summary>
        IRealType SingleType { get; }

        /// <summary>
        ///     double type
        /// </summary>
        IRealType DoubleType { get; }

        /// <summary>
        ///     byte boolean type
        /// </summary>
        IBooleanType ByteBoolType { get; }

        /// <summary>
        ///     word boolean type
        /// </summary>
        IBooleanType WordBoolType { get; }

        /// <summary>
        ///     long boolean type
        /// </summary>
        IBooleanType LongBoolType { get; }

        /// <summary>
        ///     currency type
        /// </summary>
        IRealType CurrencyType { get; }

        /// <summary>
        ///     raw byte string type
        /// </summary>
        IStringType RawByteStringType { get; }

        /// <summary>
        ///     pointer to Unicode string type
        /// </summary>
        IPointerType PUnicodeStringType { get; }

        /// <summary>
        ///     pointer to wide char type
        /// </summary>
        IPointerType PWideCharType { get; }
    }
}
