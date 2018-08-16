using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     package head
    /// </summary>
    public class PackageHeadSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new package head
        /// </summary>
        /// <param name="packageSymbol"></param>
        /// <param name="packageName"></param>
        /// <param name="semicolon"></param>
        public PackageHeadSymbol(Terminal packageSymbol, NamespaceNameSymbol packageName, Terminal semicolon) {
            PackageSymbol = packageSymbol;
            PackageName = packageName;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     package name
        /// </summary>
        public NamespaceNameSymbol PackageName { get; }

        /// <summary>
        ///     package symbol
        /// </summary>
        public Terminal PackageSymbol { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, PackageSymbol, visitor);
            AcceptPart(this, PackageName, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => PackageSymbol.GetSymbolLength() + PackageName.GetSymbolLength() + Semicolon.GetSymbolLength();
    }
}