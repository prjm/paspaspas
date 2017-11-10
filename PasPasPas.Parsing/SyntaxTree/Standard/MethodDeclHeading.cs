using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     method declaration heading
    /// </summary>
    public class MethodDeclarationHeading : StandardSyntaxTreeBase {

        /// <summary>
        ///     method kind
        /// </summary>
        public int Kind { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        public FormalParameterSection Parameters { get; set; }

        /// <summary>
        ///     result type
        /// </summary>
        public TypeSpecification ResultType { get; set; }

        /// <summary>
        ///     result type attributes
        /// </summary>
        public UserAttributes ResultTypeAttributes { get; set; }

        /// <summary>
        ///     method qualifier
        /// </summary>
        public IList<MethodDeclarationName> Qualifiers { get; }
            = new List<MethodDeclarationName>();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}