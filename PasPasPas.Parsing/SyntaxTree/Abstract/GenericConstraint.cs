using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     generic constraint
    /// </summary>
    public class GenericConstraint : SymbolTableEntryBase {

        /// <summary>
        ///     constraint kind
        /// </summary>
        public GenericConstraintKind Kind { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        public SymbolName Name { get; set; }


        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName {
            get {
                switch (Kind) {
                    case GenericConstraintKind.Class:
                        return "class";
                    case GenericConstraintKind.Record:
                        return "record";
                    case GenericConstraintKind.Constructor:
                        return "constructor";
                }

                return Name?.CompleteName;
            }
        }

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