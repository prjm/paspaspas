namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     abstract symbol name
    /// </summary>
    public class SymbolName {

        /// <summary>
        ///     get the complete unit name
        /// </summary>
        public string CompleteName
        {
            get
            {
                if (!string.IsNullOrWhiteSpace(Namespace))
                    return string.Concat(Namespace, ".", Name);
                else
                    return Name;
            }
        }


        /// <summary>
        ///     symbol name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     symbol namespace
        /// </summary>
        public string Namespace { get; set; }

    }
}
