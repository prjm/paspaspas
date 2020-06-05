#nullable disable
using System.Collections.Generic;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Types;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     declared constant
    /// </summary>
    public class ConstantDeclaration : DeclaredSymbol, ITypedSyntaxPart, ISymbolWithAttributes, IExpressionTarget, ITypeTarget {

        /// <summary>
        ///     constant mode
        /// </summary>
        public DeclarationMode Mode { get; set; }

        /// <summary>
        ///     symbol hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     attributes of the constants
        /// </summary>
        public List<SymbolAttributeItem> Attributes { get; }
            = new List<SymbolAttributeItem>();

        /// <summary>
        ///     expression value
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     type specification value
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     type information
        /// </summary>
        public ITypeSymbol TypeInfo {
            get; set;
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, TypeValue, visitor);
            AcceptPart(this, Value, visitor);
            visitor.EndVisit(this);
        }

    }
}
