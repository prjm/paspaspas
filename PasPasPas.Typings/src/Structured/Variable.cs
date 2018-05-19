﻿using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     routine parameters
    /// </summary>
    public class Variable : IRefSymbol {

        /// <summary>
        ///     parameter type
        /// </summary>
        public int SymbolType { get; set; }

        /// <summary>
        ///     parameter name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     constant function parameter
        /// </summary>
        public bool ConstantParam { get; set; }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => SymbolType;
    }
}