namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     helpers for type kind
    /// </summary>
    public static class TypeKindHelpers {

        /// <summary>
        ///     match all type kinds
        /// </summary>
        /// <param name="kind">kind</param>
        /// <param name="type1">first type kind to be compared</param>
        /// <param name="type2">second type kind to be compared</param>
        /// <returns></returns>
        public static bool All(this CommonTypeKind kind, CommonTypeKind type1, CommonTypeKind type2)
            => kind == type1 && kind == type2;

        /// <summary>
        ///     match one type kind
        /// </summary>
        /// <param name="kind">kind</param>
        /// <param name="type1">first type kind to be compared</param>
        /// <param name="type2">second type kind to be compared</param>
        /// <returns></returns>
        public static bool One(this CommonTypeKind kind, CommonTypeKind type1, CommonTypeKind type2)
            => kind == type1 || kind == type2;


        /// <summary>
        ///     test if the type kind is numerical
        /// </summary>
        /// <param name="kind">kind</param>
        /// <returns><c>true</c> if the type is integer, int64 or float</returns>
        public static bool IsNumerical(this CommonTypeKind kind)
            => kind == CommonTypeKind.IntegerType || kind == CommonTypeKind.RealType || kind == CommonTypeKind.Int64Type;

        /// <summary>
        ///     test if the type kind is integral
        /// </summary>
        /// <param name="kind">kind</param>
        /// <returns><c>true</c> if the type is integer, int64 or float</returns>
        public static bool IsIntegral(this CommonTypeKind kind)
            => kind == CommonTypeKind.IntegerType || kind == CommonTypeKind.Int64Type;

        /// <summary>
        ///     test if the type kind is a char type
        /// </summary>
        /// <param name="kind">kind</param>
        /// <returns><c>true</c> if the type is ANSI char or wide char</returns>
        public static bool IsChar(this CommonTypeKind kind)
            => kind == CommonTypeKind.AnsiCharType || kind == CommonTypeKind.WideCharType;

        /// <summary>
        ///     test if the type kind is an array type
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsArray(this CommonTypeKind kind)
            => kind == CommonTypeKind.ArrayType || kind == CommonTypeKind.ConstantArrayType || kind == CommonTypeKind.DynamicArrayType;

        /// <summary>
        ///     test if the type kind is an array type
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsEnum(this CommonTypeKind kind)
            => kind == CommonTypeKind.EnumerationType;


        /// <summary>
        ///     test if the type kind is a set type
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsSet(this CommonTypeKind kind)
            => kind == CommonTypeKind.SetType;


        /// <summary>
        ///     test if the type kind is a unicode char type
        /// </summary>
        /// <param name="kind">kind</param>
        /// <returns><c>true</c> if the type is ANSI char or wide char</returns>
        public static bool IsUnicodeChar(this CommonTypeKind kind)
            => kind == CommonTypeKind.WideCharType;


        /// <summary>
        ///     test if the type kind is a string type
        /// </summary>
        /// <param name="kind">kind</param>
        /// <returns><c>true</c> if the type is a string type</returns>
        public static bool IsString(this CommonTypeKind kind)
            => kind == CommonTypeKind.ShortStringType ||
                kind == CommonTypeKind.LongStringType ||
                kind == CommonTypeKind.UnicodeStringType ||
                kind == CommonTypeKind.WideStringType;

        /// <summary>
        ///     test if the type kind is a ANSI string type
        /// </summary>
        /// <param name="kind">kind</param>
        /// <returns><c>true</c> if the type is a string type</returns>
        public static bool IsAnsiString(this CommonTypeKind kind)
            => kind == CommonTypeKind.ShortStringType ||
                kind == CommonTypeKind.LongStringType;

        /// <summary>
        ///     test if the type kind is a string type
        /// </summary>
        /// <param name="kind">kind</param>
        /// <returns><c>true</c> if the type is a string type</returns>
        public static bool IsUnicodeString(this CommonTypeKind kind)
            => kind == CommonTypeKind.UnicodeStringType ||
               kind == CommonTypeKind.WideStringType;


        /// <summary>
        ///     test if the type kind is textual
        /// </summary>
        /// <param name="kind">type kind</param>
        /// <returns><c>true</c> if the type kind is a char or string type kind</returns>
        public static bool IsTextual(this CommonTypeKind kind)
            => kind.IsChar() || kind.IsString();

        /// <summary>
        ///     test if the type kind is unicodetext
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsUnicodeText(this CommonTypeKind kind)
            => kind.IsUnicodeChar() || kind.IsUnicodeString();

        /// <summary>
        ///     test if this type kind is a short string
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsShortString(this CommonTypeKind kind)
            => kind == CommonTypeKind.ShortStringType;

        /// <summary>
        ///     test if this type kind is a subrange type
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsSubrange(this CommonTypeKind kind)
            => kind == CommonTypeKind.SubrangeType;

        /// <summary>
        ///     check if this type kind is a type reference
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool IsType(this CommonTypeKind kind)
            => kind == CommonTypeKind.Type;

        /// <summary>
        ///     test if the type kind is an ordinal type
        /// </summary>
        /// <param name="kind">type kind</param>
        /// <returns><c>true if this type kind is an ordinal type kind</c></returns>
        public static bool IsOrdinal(this CommonTypeKind kind)
            => kind == CommonTypeKind.AnsiCharType ||
            kind == CommonTypeKind.WideCharType ||
            kind == CommonTypeKind.BooleanType ||
            kind == CommonTypeKind.EnumerationType ||
            kind == CommonTypeKind.Int64Type ||
            kind == CommonTypeKind.IntegerType ||
            kind == CommonTypeKind.SubrangeType;

        /// <summary>
        ///     test if the type kind is an ordinal type
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool Ordinal(this CommonTypeKind? kind)
            => kind.HasValue && kind.Value.IsOrdinal();

        /// <summary>
        ///     test if the type kind is an integral type
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static bool Integral(this CommonTypeKind? kind)
            => kind.HasValue && kind.Value.IsIntegral();
    }
}
