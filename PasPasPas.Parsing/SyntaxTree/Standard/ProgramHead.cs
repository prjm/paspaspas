using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     program header
    /// </summary>
    public class ProgramHead : StandardSyntaxTreeBase {

        /// <summary>
        ///     name of the program
        /// </summary>
        public NamespaceName Name { get; set; }

        /// <summary>
        ///     program parameters
        /// </summary>
        public ProgramParameterList Parameters { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}