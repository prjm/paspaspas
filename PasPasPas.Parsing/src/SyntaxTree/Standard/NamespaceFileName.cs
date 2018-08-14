using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     namespace identifier with file name
    /// </summary>
    public class NamespaceFileName : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new namespaced file name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="inSymbol"></param>
        /// <param name="fileName"></param>
        /// <param name="comma"></param>
        public NamespaceFileName(NamespaceName name, Terminal inSymbol, QuotedString fileName, Terminal comma) {
            NamespaceName = name;
            InSymbol = inSymbol;
            QuotedFileName = fileName;
            Comma = comma;
        }

        /// <summary>
        ///     Namespace name
        /// </summary>
        public NamespaceName NamespaceName { get; }

        /// <summary>
        ///     filename
        /// </summary>
        public QuotedString QuotedFileName { get; }

        /// <summary>
        ///     comma symbol
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     in symbol
        /// </summary>
        public Terminal InSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, NamespaceName, visitor);
            AcceptPart(this, InSymbol, visitor);
            AcceptPart(this, QuotedFileName, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => NamespaceName.GetSymbolLength() +
                InSymbol.GetSymbolLength() +
                QuotedFileName.GetSymbolLength() +
                Comma.GetSymbolLength();

    }
}