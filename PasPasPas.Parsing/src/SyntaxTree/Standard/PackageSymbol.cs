using PasPasPas.Globals.Files;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     package definition
    /// </summary>
    public class PackageSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new package symbol
        /// </summary>
        /// <param name="packageHead"></param>
        /// <param name="path"></param>
        /// <param name="requiresClause"></param>
        /// <param name="containsClause"></param>
        /// <param name="endSymbol"></param>
        /// <param name="dotSymbol"></param>
        public PackageSymbol(PackageHeadSymbol packageHead, FileReference path, PackageRequiresSymbol requiresClause, PackageContainsSymbol containsClause, Terminal endSymbol, Terminal dotSymbol) {
            PackageHead = packageHead;
            FilePath = path;
            RequiresClause = requiresClause;
            ContainsClause = containsClause;
            EndSymbol = endSymbol;
            DotSymbol = dotSymbol;
        }

        /// <summary>
        ///     contains clause
        /// </summary>
        public PackageContainsSymbol ContainsClause { get; }

        /// <summary>
        ///     package head
        /// </summary>
        public PackageHeadSymbol PackageHead { get; }

        /// <summary>
        ///     package name
        /// </summary>
        public NamespaceNameSymbol PackageName
            => PackageHead?.PackageName;

        /// <summary>
        ///     requires clause
        /// </summary>
        public PackageRequiresSymbol RequiresClause { get; }

        /// <summary>
        ///     path
        /// </summary>
        public FileReference FilePath { get; }

        /// <summary>
        ///     dot symbol
        /// </summary>
        public Terminal DotSymbol { get; }

        /// <summary>
        ///     end symbol
        /// </summary>
        public Terminal EndSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, PackageHead, visitor);
            AcceptPart(this, RequiresClause, visitor);
            AcceptPart(this, ContainsClause, visitor);
            AcceptPart(this, EndSymbol, visitor);
            AcceptPart(this, DotSymbol, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => PackageHead.GetSymbolLength() +
                RequiresClause.GetSymbolLength() +
                ContainsClause.GetSymbolLength() +
                EndSymbol.GetSymbolLength() +
                DotSymbol.GetSymbolLength();

    }
}
