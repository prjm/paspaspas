using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
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
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     variable names
        /// </summary>
        public ISyntaxPartList<VariableName> Names { get; }

        /// <summary>
        ///     create a new variable declaration
        /// </summary>
        public VariableDeclaration()
            => Names = new SyntaxPartCollection<VariableName>(this);

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
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
