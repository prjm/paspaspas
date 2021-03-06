﻿#nullable disable
namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     interface for structural type members
    /// </summary>
    public interface IStructuredTypeMember {

        /// <summary>
        ///     static member
        /// </summary>
        bool ClassItem { get; }

        /// <summary>
        ///     member attributes
        /// </summary>
        UserAttributesSymbol Attributes1 { get; }

        /// <summary>
        ///     member attributes
        /// </summary>
        UserAttributesSymbol Attributes2 { get; }

    }
}