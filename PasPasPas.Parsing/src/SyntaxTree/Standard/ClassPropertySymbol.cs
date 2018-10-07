using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     property declaration
    /// </summary>
    public class ClassPropertySymbol : VariableLengthSyntaxTreeBase<ClassPropertySpecifierSymbol> {

        /// <summary>
        ///     class property symbol
        /// </summary>
        /// <param name="propertySymbol"></param>
        /// <param name="propertyName"></param>
        /// <param name="openBraces"></param>
        /// <param name="arrayIndex"></param>
        /// <param name="closeBraces"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="typeName"></param>
        /// <param name="indexSymbol"></param>
        /// <param name="propertyIndex"></param>
        /// <param name="items"></param>
        /// <param name="semicolon"></param>
        /// <param name="defaultSymbol"></param>
        /// <param name="semicolon2"></param>
        public ClassPropertySymbol(Terminal propertySymbol, IdentifierSymbol propertyName, Terminal openBraces, FormalParametersSymbol arrayIndex, Terminal closeBraces, Terminal colonSymbol, TypeNameSymbol typeName, Terminal indexSymbol, ExpressionSymbol propertyIndex, ImmutableArray<ClassPropertySpecifierSymbol> items, Terminal semicolon, Terminal defaultSymbol, Terminal semicolon2) : base(items) {
            PropertySymbol = propertySymbol;
            PropertyName = propertyName;
            OpenBraces = openBraces;
            ArrayIndex = arrayIndex;
            CloseBraces = closeBraces;
            ColonSymbol = colonSymbol;
            TypeName = typeName;
            IndexSymbol = indexSymbol;
            PropertyIndex = propertyIndex;
            Semicolon = semicolon;
            DefaultSymbol = defaultSymbol;
            Semicolon2 = semicolon2;
        }

        /// <summary>
        ///     property access index
        /// </summary>
        public FormalParametersSymbol ArrayIndex { get; }

        /// <summary>
        ///     default flag (for disp interface)
        /// </summary>
        public bool IsDefault { get; }

        /// <summary>
        ///     index of the property
        /// </summary>
        public ExpressionSymbol PropertyIndex { get; }

        /// <summary>
        ///     property name
        /// </summary>
        public IdentifierSymbol PropertyName { get; }

        /// <summary>
        ///     property type
        /// </summary>
        public TypeNameSymbol TypeName { get; }

        /// <summary>
        ///     property symbol
        /// </summary>
        public Terminal PropertySymbol { get; }

        /// <summary>
        ///     open braces
        /// </summary>
        public Terminal OpenBraces { get; }

        /// <summary>
        ///     close braces
        /// </summary>
        public Terminal CloseBraces { get; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; }

        /// <summary>
        ///     index symbol
        /// </summary>
        public Terminal IndexSymbol { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     default symbol
        /// </summary>
        public Terminal DefaultSymbol { get; }

        /// <summary>
        ///     semicolon2
        /// </summary>
        public Terminal Semicolon2 { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, PropertySymbol, visitor);
            AcceptPart(this, PropertyName, visitor);
            AcceptPart(this, OpenBraces, visitor);
            AcceptPart(this, ArrayIndex, visitor);
            AcceptPart(this, CloseBraces, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, TypeName, visitor);
            AcceptPart(this, IndexSymbol, visitor);
            AcceptPart(this, PropertyIndex, visitor);
            AcceptPart(this, visitor);
            AcceptPart(this, Semicolon, visitor);
            AcceptPart(this, DefaultSymbol, visitor);
            AcceptPart(this, Semicolon2, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => PropertySymbol.GetSymbolLength()
            + PropertyName.GetSymbolLength()
            + OpenBraces.GetSymbolLength()
            + ArrayIndex.GetSymbolLength()
            + CloseBraces.GetSymbolLength()
            + ColonSymbol.GetSymbolLength()
            + TypeName.GetSymbolLength()
            + IndexSymbol.GetSymbolLength()
            + PropertyIndex.GetSymbolLength()
            + ItemLength
            + Semicolon.GetSymbolLength()
            + DefaultSymbol.GetSymbolLength()
            + Semicolon2.GetSymbolLength();

    }
}
