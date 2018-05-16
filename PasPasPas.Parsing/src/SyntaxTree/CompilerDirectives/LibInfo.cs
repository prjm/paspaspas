using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     libprefix, libsuffix and libversion directive
    /// </summary>
    public class LibInfo : CompilerDirectiveBase {

        /// <summary>
        ///     prefix
        /// </summary>
        public string LibPrefix { get; set; }

        /// <summary>
        ///     suffix
        /// </summary>
        public string LibSuffix { get; set; }

        /// <summary>
        ///     version
        /// </summary>
        public string LibVersion { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}
