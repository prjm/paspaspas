﻿namespace PasPasPas.Parsing.SyntaxTree.Types {

    /// <summary>
    ///     definition of type kinds
    /// </summary>
    public enum CommonTypeKind {

        /// <summary>
        ///     unknown type
        /// </summary>
        UnknownType,

        /// <summary>
        ///     Integer type
        /// </summary>
        IntegerType,

        /// <summary>
        ///     char type
        /// </summary>
        AnsiCharType,

        /// <summary>
        ///     enumeration type
        /// </summary>
        EnumerationType,

        /// <summary>
        ///     float type
        /// </summary>
        FloatType,

        /// <summary>
        ///     short string type
        /// </summary>
        ShortStringType,

        /// <summary>
        ///     set type
        /// </summary>
        SetType,

        /// <summary>
        ///     class type
        /// </summary>
        ClassType,

        /// <summary>
        ///     method type
        /// </summary>
        MethodType,

        /// <summary>
        ///     wide char type
        /// </summary>
        WideCharType,

        /// <summary>
        ///     long string type
        /// </summary>
        LongStringType,

        /// <summary>
        ///     wide string type
        /// </summary>
        WideStringType,

        /// <summary>
        ///     varian type
        /// </summary>
        VariantType,

        /// <summary>
        ///     array type
        /// </summary>
        ArrayType,

        /// <summary>
        ///     record type
        /// </summary>
        RecordType,

        /// <summary>
        ///     interface type
        /// </summary>
        InterfaceType,

        /// <summary>
        ///     int64 type
        /// </summary>
        Int64Type,

        /// <summary>
        ///     dynamic array type
        /// </summary>
        DynamicArrayType,

        /// <summary>
        ///     unicode string type
        /// </summary>
        UnicodeStringType,

        /// <summary>
        ///     class reference type
        /// </summary>
        ClassReferenceType,

        /// <summary>
        ///     pointer type
        /// </summary>
        PointerType,

        /// <summary>
        ///     procedural type
        /// </summary>
        ProcedureType,

        /// <summary>
        ///     boolean type
        /// </summary>
        /// <remarks>GetTypeKind(..) for booleans returns enumeration</remarks>
        BooleanType,

    }
}