﻿namespace PasPasPas.Globals.Types {

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
    }
}
