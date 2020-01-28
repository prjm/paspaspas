namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     reference to symbol
    /// </summary>
    public class Reference {

        /// <summary>
        ///     create a new reference
        /// </summary>
        /// <param name="kind">reference kind</param>
        /// <param name="symbol">referenced symbol</param>
        public Reference(ReferenceKind kind, ITypeSymbol symbol) {
            Kind = kind;
            Symbol = symbol;
        }

        /// <summary>
        ///     reference kind
        /// </summary>
        public ReferenceKind Kind { get; }

        /// <summary>
        ///     referenced symbol
        /// </summary>
        public ITypeSymbol Symbol { get; }

    }
}
