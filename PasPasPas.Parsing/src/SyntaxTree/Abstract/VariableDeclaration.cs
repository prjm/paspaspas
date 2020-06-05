#nullable disable
using System.Collections.Generic;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     variable declaration
    /// </summary>
    public class VariableDeclaration : DeclaredSymbolGroup, ITypeTarget, IExpressionTarget, ITypedSyntaxPart {

        /// <summary>
        ///     declaration mode
        /// </summary>
        public DeclarationMode Mode { get; set; }

        /// <summary>
        ///     optional variable type specification
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     symbol hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     variable names
        /// </summary>
        public ISyntaxPartCollection<VariableName> Names { get; }

        /// <summary>
        ///     create a new variable declaration
        /// </summary>
        public VariableDeclaration()
            => Names = new SyntaxPartCollection<VariableName>();

        /// <summary>
        ///     attributes
        /// </summary>
        public List<SymbolAttributeItem> Attributes { get; }
            = new List<SymbolAttributeItem>();

        /// <summary>
        ///     value kind
        /// </summary>
        public VariableValueKind ValueKind { get; set; }
            = VariableValueKind.Unknown;

        /// <summary>
        ///     variable initialization value
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     type definition
        /// </summary>
        public ITypeSymbol TypeInfo { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Names, visitor);
            AcceptPart(this, TypeValue, visitor);
            visitor.EndVisit(this);
        }

    }
}
