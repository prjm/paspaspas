#nullable disable
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     application type parameter
    /// </summary>
    public class AppTypeParameter : CompilerDirectiveBase {

        /// <summary>
        ///     create a new application type parameter
        /// </summary>
        /// <param name="appTypeSymbol"></param>
        /// <param name="appTypeInfo"></param>
        /// <param name="appType"></param>
        public AppTypeParameter(Terminal appTypeSymbol, Terminal appTypeInfo, AppType appType) {
            AppTypeSymbol = appTypeSymbol;
            AppTypeInfo = appTypeInfo;
            ApplicationType = appType;
        }

        /// <summary>
        ///     application type
        /// </summary>
        public AppType ApplicationType { get; }

        /// <summary>
        ///     app type constant
        /// </summary>
        public Terminal AppTypeInfo { get; }

        /// <summary>
        ///     apptype symbol
        /// </summary>
        public Terminal AppTypeSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, AppTypeSymbol, visitor);
            AcceptPart(this, AppTypeInfo, visitor);
            visitor.EndVisit(this);
        }

    }
}
