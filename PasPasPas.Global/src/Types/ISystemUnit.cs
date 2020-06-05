#nullable disable
using PasPasPas.Globals.Runtime;

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
        IErrorType ErrorType { get; }

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

        /// <summary>
        ///     unspecified type
        /// </summary>
        IUnspecifiedType UnspecifiedType { get; }

        /// <summary>
        ///     no type at all / void
        /// </summary>
        INoType NoType { get; }

        /// <summary>
        ///     string type
        /// </summary>
        IAliasedType StringType { get; }

        /// <summary>
        ///     TObject type
        /// </summary>
        IStructuredType TObjectType { get; }

        /// <summary>
        ///     unspecified file type
        /// </summary>
        IFileType UnspecifiedFileType { get; }

        /// <summary>
        ///     unconstrained generic type parameter
        /// </summary>
        IGenericTypeParameter UnconstrainedGenericTypeParameter { get; }

        /// <summary>
        ///     format expression helper routine
        /// </summary>
        IRoutineGroup FormatExpression { get; }


        /// <summary>
        ///     generic class constraint
        /// </summary>
        ITypeDefinition GenericClassConstraint { get; }

        /// <summary>
        ///     generic record constraint
        /// </summary>
        ITypeDefinition GenericRecordConstraint { get; }

        /// <summary>
        ///     generic constructor constraint
        /// </summary>
        ITypeDefinition GenericConstructorConstraint { get; }

        /// <summary>
        ///     char type
        /// </summary>
        IAliasedType CharType { get; }

        /// <summary>
        ///     ucs2 char type
        /// </summary>
        IAliasedType Ucs2CharType { get; }

        /// <summary>
        ///     ucs4 char type
        /// </summary>
        IAliasedType Ucs4CharType { get; }

        /// <summary>
        ///     pointer to byte type
        /// </summary>
        IPointerType PByteType { get; }

        /// <summary>
        ///     pointer to short int type
        /// </summary>
        IPointerType PShortIntType { get; }

        /// <summary>
        ///     pointer to word type
        /// </summary>
        IPointerType PWordType { get; }

        /// <summary>
        ///     pointer to small int type
        /// </summary>
        IPointerType PSmallIntType { get; }

        /// <summary>
        ///     pointer to cardinal type
        /// </summary>
        IPointerType PCardinalType { get; }

        /// <summary>
        ///     pointer longword type
        /// </summary>
        IPointerType PLongwordType { get; }

        /// <summary>
        ///       pointer to fixed uint type
        /// </summary>
        IPointerType PFixedUIntType { get; }

        /// <summary>
        ///     pointer to integer type
        /// </summary>
        IPointerType PIntegerType { get; }

        /// <summary>
        ///     pointer to long int type
        /// </summary>
        IPointerType PLongIntType { get; }

        /// <summary>
        ///     pointer to int64
        /// </summary>
        IPointerType PInt64Type { get; }

        /// <summary>
        ///     pointer to uint64
        /// </summary>
        IPointerType PUInt64Type { get; }

        /// <summary>
        ///     pointer to fixed integer type
        /// </summary>
        IPointerType PFixedIntType { get; }

        /// <summary>
        ///     pointer to native uint
        /// </summary>
        IPointerType PNativeUIntType { get; }

        /// <summary>
        ///     pointer to native int type
        /// </summary>
        IPointerType PNativeIntType { get; }


        /// <summary>
        ///     pointer to single type
        /// </summary>
        IPointerType PSingleType { get; }

        /// <summary>
        ///     pointer to double type
        /// </summary>
        IPointerType PDoubleType { get; }

        /// <summary>
        ///     pointer to extended type
        /// </summary>
        IPointerType PExtendedType { get; }

        /// <summary>
        ///     pointer to ANSI char type
        /// </summary>
        IPointerType PAnsiCharType { get; }

        /// <summary>
        ///     pointer to currency type
        /// </summary>
        IPointerType PCurrencyType { get; }

        /// <summary>
        ///     pointer to pointer type
        /// </summary>
        IPointerType PPointerType { get; }

        /// <summary>
        ///     pointer to word boolean type
        /// </summary>
        IPointerType PWordBoolType { get; }

        /// <summary>
        ///     pointer to long boolean type
        /// </summary>
        IPointerType PLongBoolType { get; }

        /// <summary>
        ///     pointer to boolean type
        /// </summary>
        IPointerType PBooleanType { get; }

        /// <summary>
        ///     pointer to byte boolean type
        /// </summary>
        IPointerType PByteBoolType { get; }

        /// <summary>
        ///     pointer to wide string type
        /// </summary>
        IPointerType PWideStringType { get; }

        /// <summary>
        ///     pointer to short string type
        /// </summary>
        IPointerType PShortStringType { get; }

        /// <summary>
        ///     pointer to ANSI string type
        /// </summary>
        IPointerType PAnsiStringType { get; }

        /// <summary>
        ///     real type
        /// </summary>
        IAliasedType RealType { get; }

        /// <summary>
        ///     pointer to string type
        /// </summary>
        IAliasedType PStringType { get; }

        /// <summary>
        ///     pointer to char type
        /// </summary>
        IAliasedType PCharType { get; }

        /// <summary>
        ///     long word type
        /// </summary>
        IAliasedType LongWordType { get; }

        /// <summary>
        ///     comp type
        /// </summary>
        IRealType CompType { get; }

        /// <summary>
        ///     real48 type
        /// </summary>
        IRealType Real48Type { get; }
        IAliasedType UInt8Type { get; }
        IAliasedType Int8Type { get; }
        IAliasedType UInt16Type { get; }
        IAliasedType Int16Type { get; }
        IAliasedType UInt32Type { get; }
        IAliasedType Int32Type { get; }
        IPointerType PRawByteStringType { get; }
    }
}
