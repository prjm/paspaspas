using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     reference to symbol
    /// </summary>
    public class Reference {
        private readonly ReferenceKind refKind;
        private readonly IRefSymbol refSymbol;

        /// <summary>
        ///     create a new reference
        /// </summary>
        /// <param name="kind">reference kind</param>
        /// <param name="symbol">referenced symbol</param>
        public Reference(ReferenceKind kind, IRefSymbol symbol) {
            refKind = kind;
            refSymbol = symbol;
        }

        /// <summary>
        ///     reference kind
        /// </summary>
        public ReferenceKind Kind
            => refKind;

        /// <summary>
        ///     referebced symbol
        /// </summary>
        public IRefSymbol Symbol
            => refSymbol;

    }
}
