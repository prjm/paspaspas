using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record field
    /// </summary>
    public class RecordField : StandardSyntaxTreeBase {

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
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}