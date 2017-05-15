using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record declaration
    /// </summary>
    public class RecordDeclaration : StandardSyntaxTreeBase {

        /// <summary>
        ///     field list
        /// </summary>
        public RecordFieldList FieldList { get; set; }

        /// <summary>
        ///     record items
        /// </summary>
        public RecordItems Items { get; set; }

        /// <summary>
        ///     variant section
        /// </summary>
        public RecordVariantSection VariantSection { get; set; }

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