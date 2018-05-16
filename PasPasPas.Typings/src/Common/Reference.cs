using PasPasPas.Globals.Types;

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
        ///     referenced symbol
        /// </summary>
        public IRefSymbol Symbol
            => refSymbol;

    }
}
