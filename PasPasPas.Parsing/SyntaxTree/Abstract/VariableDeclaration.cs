using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     variable declaration
    /// </summary>
    public class VariableDeclaration : DeclaredSymbolGroup, ITypeTarget, IExpressionTarget {

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
        public SymbolHints Hints { get; internal set; }

        /// <summary>
        ///     variable names
        /// </summary>
        public IList<VariableName> Names { get; }
            = new List<VariableName>();

        /// <summary>
        ///     enumerate part
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (VariableName name in Names)
                    yield return name;
            }
        }

        /// <summary>
        ///     attribuetes
        /// </summary>
        public IList<SymbolAttribute> Attributes { get; internal set; }

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
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartVisitor startVisitor, IEndVisitor endVisitor) {
            startVisitor.StartVisit(this);
            AcceptParts(startVisitor, endVisitor);
            endVisitor.EndVisit(this);
        }

    }
}
