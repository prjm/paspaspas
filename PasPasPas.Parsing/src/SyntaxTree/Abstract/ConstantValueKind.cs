﻿#nullable disable
namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     constant value kind
    /// </summary>
    public enum ConstantValueKind {

        /// <summary>
        ///     unknown kind
        /// </summary>
        Unknown = 0,

        /// <summary>
        ///     nil value
        /// </summary>
        Nil,

        /// <summary>
        ///     true value
        /// </summary>
        True,

        /// <summary>
        ///     false value
        /// </summary>
        False,

        /// <summary>
        ///     integer value
        /// </summary>
        IntegralNumber,

        /// <summary>
        ///     real value
        /// </summary>
        RealNumber,

        /// <summary>
        ///     hex number
        /// </summary>
        HexNumber,

        /// <summary>
        ///     string value
        /// </summary>
        QuotedString,
    }
}