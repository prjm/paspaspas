namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     type registry provider
    /// </summary>
    public interface ITypeRegistryProvider {

        /// <summary>
        ///     type registry
        /// </summary>
        ITypeRegistry RegisteredTypes { get; }

    }

    /// <summary>
    ///     type registry provider helpers
    /// </summary>
    public static class TypeRegistryProviderHelper {

        /// <summary>
        ///     resolve the boolean type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IBooleanType GetBooleanType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.BooleanType;

        /// <summary>
        ///     resolve the boolean type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static ICharType GetWideCharType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.WideCharType;


        /// <summary>
        ///     resolve the UNICODE string type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IStringType GetUnicodeStringType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.UnicodeStringType;

        /// <summary>
        ///     ANSI string type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IStringType GetAnsiStringType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.AnsiStringType;

        /// <summary>
        ///     short string type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IStringType GetShortStringType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.ShortStringType;

        /// <summary>
        ///     short string type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IStringType GetWideStringType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.WideStringType;


        /// <summary>
        ///     short integer type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IIntegralType GetShortIntType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.ShortIntType;

        /// <summary>
        ///     extended floating point precision type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IRealType GetExtendedType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.ExtendedType;

        /// <summary>
        ///     byte type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IIntegralType GetByteType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.ByteType;


        /// <summary>
        ///     small int type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IIntegralType GetSmallIntType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.SmallIntType;

        /// <summary>
        ///     word type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IIntegralType GetWordType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.WordType;


        /// <summary>
        ///     integer type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IIntegralType GetIntegerType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.IntegerType;


        /// <summary>
        ///     cardinal type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IIntegralType GetCardinalType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.CardinalType;


        /// <summary>
        ///     int64 type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IIntegralType GetInt64Type(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.Int64Type;

        /// <summary>
        ///     uint64 type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static IIntegralType GetUInt64Type(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.UInt64Type;

        /// <summary>
        ///     uint64 type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static ITypeDefinition GetErrorType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.ErrorType;

        /// <summary>
        ///     get the nil type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static ITypeDefinition GetNilType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.NilType;

        /// <summary>
        ///     generic pointer type
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        public static ITypeDefinition GetGenericPointerType(this ITypeRegistryProvider provider)
            => provider.RegisteredTypes.SystemUnit.GenericPointerType;


    }

}
