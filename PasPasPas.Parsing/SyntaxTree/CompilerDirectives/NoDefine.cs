using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     no define directive
    /// </summary>
    public class NoDefine : CompilerDirectiveBase {

        /// <summary>
        ///     type name
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        ///     tpye names in header files
        /// </summary>
        public string TypeNameInHpp { get; set; }

        /// <summary>
        ///     type name in unions
        /// </summary>
        public string TypeNameInUnion { get; set; }

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
