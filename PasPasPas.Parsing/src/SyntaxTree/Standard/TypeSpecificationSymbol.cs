using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     a type specification
    /// </summary>
    public class TypeSpecificationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new type specification
        /// </summary>
        /// <param name="structuredType"></param>
        /// <param name="comma"></param>
        public TypeSpecificationSymbol(StructTypeSymbol structuredType, Terminal comma) {
            StructuredType = structuredType;
            Comma = comma;
        }

        /// <summary>
        ///     create a new type specification
        /// </summary>
        /// <param name="pointerType"></param>
        /// <param name="comma"></param>
        public TypeSpecificationSymbol(PointerTypeSymbol pointerType, Terminal comma) {
            PointerType = pointerType;
            Comma = comma;
        }

        /// <summary>
        ///     create a new type specification
        /// </summary>
        /// <param name="stringType"></param>
        /// <param name="comma"></param>
        public TypeSpecificationSymbol(StringTypeSymbol stringType, Terminal comma) {
            StringType = stringType;
            Comma = comma;
        }

        /// <summary>
        ///     create a new type specification
        /// </summary>
        /// <param name="procedureType"></param>
        /// <param name="comma"></param>
        public TypeSpecificationSymbol(ProcedureTypeSymbol procedureType, Terminal comma) {
            ProcedureType = procedureType;
            Comma = comma;
        }

        /// <summary>
        ///     create a new type specification
        /// </summary>
        /// <param name="simpleType"></param>
        /// <param name="comma"></param>
        public TypeSpecificationSymbol(SimpleTypeSymbol simpleType, Terminal comma) {
            SimpleType = simpleType;
            Comma = comma;
        }

        /// <summary>
        ///     pointer type
        /// </summary>
        public PointerTypeSymbol PointerType { get; }

        /// <summary>
        ///     procedure type
        /// </summary>
        public ProcedureTypeSymbol ProcedureType { get; }

        /// <summary>
        ///     simple type
        /// </summary>
        public SimpleTypeSymbol SimpleType { get; }

        /// <summary>
        ///     string type
        /// </summary>
        public StringTypeSymbol StringType { get; }

        /// <summary>
        ///     structured type
        /// </summary>
        public StructTypeSymbol StructuredType { get; }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, PointerType, visitor);
            AcceptPart(this, ProcedureType, visitor);
            AcceptPart(this, SimpleType, visitor);
            AcceptPart(this, StringType, visitor);
            AcceptPart(this, StructuredType, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length =>
            PointerType.GetSymbolLength() +
            ProcedureType.GetSymbolLength() +
            SimpleType.GetSymbolLength() +
            StringType.GetSymbolLength() +
            StructuredType.GetSymbolLength() +
            Comma.GetSymbolLength();

    }
}