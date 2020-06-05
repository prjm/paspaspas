#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     interface item
    /// </summary>
    public class InterfaceItemSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new interface item
        /// </summary>
        /// <param name="classMethodSymbol"></param>
        public InterfaceItemSymbol(ClassMethodSymbol classMethodSymbol) => Method = classMethodSymbol;

        /// <summary>
        ///     create a new interface item
        /// </summary>
        /// <param name="classPropertySymbol"></param>
        public InterfaceItemSymbol(ClassPropertySymbol classPropertySymbol)
            => Property = classPropertySymbol;

        /// <summary>
        ///     method declaration
        /// </summary>
        public ClassMethodSymbol Method { get; }

        /// <summary>
        ///     property declaration
        /// </summary>
        public ClassPropertySymbol Property { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Method, visitor);
            AcceptPart(this, Property, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Method.GetSymbolLength() +
               Property.GetSymbolLength();

    }
}