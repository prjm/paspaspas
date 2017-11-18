using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a basic method declaration
    /// </summary>
    public class MethodDeclaration : DeclaredSymbol, IParameterTarget, ITypeTarget, IDirectiveTarget {

        /// <summary>
        ///     procedure kind
        /// </summary>
        public ProcedureKind Kind { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public ParameterDefinitions Parameters { get; }

        /// <summary>
        ///     return type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     return type attributes
        /// </summary>
        public IList<SymbolAttribute> ReturnAttributes { get; set; }

        /// <summary>
        ///     user attributes
        /// </summary>
        public IList<SymbolAttribute> Attributes { get; set; }

        /// <summary>
        ///     overloaded methods
        /// </summary>
        public IList<MethodDeclaration> Overloads { get; set; }

        /// <summary>
        ///     creates a new method declaration
        /// </summary>
        public MethodDeclaration() {
            Directives = new SyntaxPartCollection<MethodDirective>(this);
            Parameters = new ParameterDefinitions() { ParentItem = this };
        }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (var parameter in Parameters.Items)
                    yield return parameter;
                if (TypeValue != null)
                    yield return TypeValue;
                if (Overloads != null)
                    foreach (var overload in Overloads)
                        yield return overload;
            }
        }

        /// <summary>
        ///     directives
        /// </summary>
        public ISyntaxPartList<MethodDirective> Directives { get; }

        /// <summary>
        ///     symbol hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     test if the method is overloaded (or not)
        /// </summary>
        public bool IsOverloaded {
            get {
                foreach (var directive in Directives)
                    if (directive.Kind == MethodDirectiveKind.Overload)
                        return true;
                return false;
            }
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
