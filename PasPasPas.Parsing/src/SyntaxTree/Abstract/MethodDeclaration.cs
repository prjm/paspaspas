using System.Collections.Generic;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     a basic method declaration
    /// </summary>
    public abstract class MethodDeclaration : DeclaredSymbol, IParameterTarget, ITypeTarget, IDirectiveTarget {

        /// <summary>
        ///     procedure kind
        /// </summary>
        public ProcedureKind Kind { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public ParameterDefinitionCollection Parameters { get; }

        /// <summary>
        ///     return type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     return type attributes
        /// </summary>
        public IList<SymbolAttributeItem> ReturnAttributes { get; }
            = new List<SymbolAttributeItem>();

        /// <summary>
        ///     user attributes
        /// </summary>
        public List<SymbolAttributeItem> Attributes { get; }
            = new List<SymbolAttributeItem>();

        /// <summary>
        ///     creates a new method declaration
        /// </summary>
        protected MethodDeclaration() {
            Directives = new SyntaxPartCollection<MethodDirective>();
            Parameters = new ParameterDefinitionCollection();
        }

        /// <summary>
        ///     directives
        /// </summary>
        public ISyntaxPartCollection<MethodDirective> Directives { get; }

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
            AcceptBaseParts(visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     accepts parts of base class
        /// </summary>
        /// <param name="visitor"></param>
        protected virtual void AcceptBaseParts(IStartEndVisitor visitor) {
            AcceptPart(this, Parameters.Items, visitor);
            AcceptPart(this, TypeValue, visitor);
        }

    }
}
