using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     libprefix, libsuffix and libversion directive
    /// </summary>
    public class LibInfo : CompilerDirectiveBase {

        /// <summary>
        ///     create a new lib directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="libParam"></param>
        /// <param name="libPrefix"></param>
        /// <param name="libSuffix"></param>
        /// <param name="libVersion"></param>
        public LibInfo(Terminal symbol, Terminal libParam, string libPrefix, string libSuffix, string libVersion) {
            Symbol = symbol;
            LibParam = libParam;
            LibPrefix = libPrefix;
            LibSuffix = libSuffix;
            LibVersion = libVersion;
        }

        /// <summary>
        ///     prefix
        /// </summary>
        public string LibPrefix { get; }

        /// <summary>
        ///     suffix
        /// </summary>
        public string LibSuffix { get; }

        /// <summary>
        ///     version
        /// </summary>
        public string LibVersion { get; }

        /// <summary>
        ///     symbol
        /// </summary>
        public Terminal Symbol { get; }

        /// <summary>
        ///     parameter
        /// </summary>
        public Terminal LibParam { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Symbol, visitor);
            AcceptPart(this, LibParam, visitor);
            visitor.EndVisit(this);
        }


    }
}
