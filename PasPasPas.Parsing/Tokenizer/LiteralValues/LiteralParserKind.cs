using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     integer literal parser kind
    /// </summary>
    public enum LiteralParserKind {

        /// <summary>
        ///     undefined parser
        /// </summary>
        Undefined,

        /// <summary>
        ///     parse ints
        /// </summary>
        IntegerNumbers,

        /// <summary>
        ///     parse hex numbers
        /// </summary>
        HexNumbers

    }
}
