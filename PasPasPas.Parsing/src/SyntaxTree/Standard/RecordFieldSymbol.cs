using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     record field
    /// </summary>
    public class RecordFieldSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new record field definition
        /// </summary>
        /// <param name="names"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="fieldType"></param>
        /// <param name="hint"></param>
        /// <param name="semicolon"></param>
        public RecordFieldSymbol(IdentifierListSymbol names, Terminal colonSymbol, TypeSpecificationSymbol fieldType, HintingInformationListSymbol hint, Terminal semicolon) {
            Names = names;
            ColonSymbol = colonSymbol;
            FieldType = fieldType;
            Hint = hint;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     field type
        /// </summary>
        public TypeSpecificationSymbol FieldType { get; }

        /// <summary>
        ///     hinting directive
        /// </summary>
        public ISyntaxPart Hint { get; }

        /// <summary>
        ///     field names
        /// </summary>
        public IdentifierListSymbol Names { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Names, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, FieldType, visitor);
            AcceptPart(this, Hint, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Names.GetSymbolLength() +
                ColonSymbol.GetSymbolLength() +
                FieldType.GetSymbolLength() +
                Hint.GetSymbolLength() +
                Semicolon.GetSymbolLength();

    }
}