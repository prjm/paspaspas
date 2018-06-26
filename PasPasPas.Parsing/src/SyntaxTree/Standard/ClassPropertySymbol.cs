using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     property declaration
    /// </summary>
    public class ClassPropertySymbol : VariableLengthSyntaxTreeBase<ClassPropertySpecifierSymbol> {

        /// <summary>
        ///     create a new class property symbol
        /// </summary>
        /// <param name="items"></param>
        public ClassPropertySymbol(ImmutableArray<ClassPropertySpecifierSymbol> items) : base(items) {

        }

        /// <summary>
        ///     property access index
        /// </summary>
        public SyntaxPartBase ArrayIndex { get; set; }

        /// <summary>
        ///     default flag (for disp interface)
        /// </summary>
        public bool IsDefault { get; internal set; }

        /// <summary>
        ///     index of the property
        /// </summary>
        public SyntaxPartBase PropertyIndex { get; set; }

        /// <summary>
        ///     property name
        /// </summary>
        public Identifier PropertyName { get; set; }

        /// <summary>
        ///     property type
        /// </summary>
        public SyntaxPartBase TypeName { get; set; }

        /// <summary>
        ///     property symbol
        /// </summary>
        public Terminal PropertySymbol { get; set; }

        /// <summary>
        ///     open braces
        /// </summary>
        public Terminal OpenBraces { get; set; }

        /// <summary>
        ///     close braces
        /// </summary>
        public Terminal CloseBraces { get; set; }

        /// <summary>
        ///     colon symbol
        /// </summary>
        public Terminal ColonSymbol { get; set; }

        /// <summary>
        ///     index symbol
        /// </summary>
        public Terminal IndexSymbol { get; set; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; set; }

        /// <summary>
        ///     default symbol
        /// </summary>
        public Terminal DefaultSymbol { get; set; }

        /// <summary>
        ///     semicolon2
        /// </summary>
        public Terminal Semicolon2 { get; set; }

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
        public int Length
            => PropertySymbol.Length
            + PropertyName.Length
            + OpenBraces.Length
            + ArrayIndex.Length
            + CloseBraces.Length
            + ColonSymbol.Length
            + TypeName.Length
            + IndexSymbol.Length
            + PropertyIndex.Length
            + ItemLength
            + Semicolon.Length
            + DefaultSymbol.Length
            + Semicolon2.Length;


    }
}
