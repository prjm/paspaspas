using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record field
    /// </summary>
    public class RecordField : StandardSyntaxTreeBase {
        public RecordField(IExtendableSyntaxPart parent) {
            Parent = parent;
            parent?.Add(this);
        }

        /// <summary>
        ///     field type
        /// </summary>
        public TypeSpecification FieldType { get; set; }

        /// <summary>
        ///     hinting directive
        /// </summary>
        public HintingInformationList Hint { get; set; }


        /// <summary>
        ///     field names
        /// </summary>
        public IdentifierList Names { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(this, startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }


    }
}