namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     definition of available type kinds
    /// </summary>
    public enum CommonTypeKind {

        /// <summary>
        ///     unknown type
        /// </summary>
        UnknownType = 0,

        /// <summary>
        ///     integer type (of 8, 16 or 32 bit size)
        /// </summary>
        IntegerType = 1,

        /// <summary>
        ///     byte char type (8 bit)
        /// </summary>
        AnsiCharType = 2,

        /// <summary>
        ///     enumeration type
        /// </summary>
        EnumerationType = 3,

        /// <summary>
        ///     floating point number type
        /// </summary>
        RealType = 4,

        /// <summary>
        ///     short string type
        /// </summary>
        ShortStringType = 5,

        /// <summary>
        ///     set type
        /// </summary>
        SetType = 6,

        /// <summary>
        ///     class type
        /// </summary>
        ClassType = 7,

        /// <summary>
        ///     method type
        /// </summary>
        MethodType = 8,

        /// <summary>
        ///     wide char type
        /// </summary>
        WideCharType = 9,

        /// <summary>
        ///     long string type
        /// </summary>
        LongStringType = 10,

        /// <summary>
        ///     wide string type
        /// </summary>
        WideStringType = 11,

        /// <summary>
        ///     variant record type
        /// </summary>
        VariantType = 12,

        /// <summary>
        ///     array type
        /// </summary>
        StaticArrayType = 13,

        /// <summary>
        ///     record type
        /// </summary>
        RecordType = 14,

        /// <summary>
        ///     interface type
        /// </summary>
        InterfaceType = 15,

        /// <summary>
        ///     int64 type
        /// </summary>
        Int64Type = 16,

        /// <summary>
        ///     dynamic array type
        /// </summary>
        DynamicArrayType = 17,

        /// <summary>
        ///     Unicode string type
        /// </summary>
        UnicodeStringType = 18,

        /// <summary>
        ///     class reference type
        /// </summary>
        MetaClassType = 19,

        /// <summary>
        ///     pointer type
        /// </summary>
        PointerType = 20,

        /// <summary>
        ///     procedural type
        /// </summary>
        ProcedureType = 21,

        /// <summary>
        ///     boolean type
        /// </summary>
        /// <remarks>GetTypeKind(..) for booleans returns enumeration</remarks>
        BooleanType = 22,

        /// <summary>
        ///     class helper
        /// </summary>
        /// <remarks>GetTypeKind(..) for class helpers return class</remarks>
        ClassHelperType = 23,

        /// <summary>
        ///     record helper
        /// </summary>
        /// <remarks>GetTypeKind(..) for class helpers return class</remarks>
        RecordHelperType = 24,

        /// <summary>
        ///     unit type
        /// </summary>
        Unit = 25,

        /// <summary>
        ///     subrange type
        /// </summary>
        SubrangeType = 26,

        /// <summary>
        ///     hidden / internal type
        /// </summary>
        HiddenType = 27,

        /// <summary>
        ///     constant array type
        /// </summary>
        ConstantArrayType = 28,

        /// <summary>
        ///     declared type name
        /// </summary>
        Type = 29,

        /// <summary>
        ///     file type
        /// </summary>
        FileType = 30,
    }
}
