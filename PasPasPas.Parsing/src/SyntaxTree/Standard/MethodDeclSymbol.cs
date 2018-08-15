using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     method declaration
    /// </summary>
    public class MethodDeclarationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new method declaration
        /// </summary>
        /// <param name="classSymbol"></param>
        /// <param name="heading"></param>
        /// <param name="semicolon"></param>
        /// <param name="directives"></param>
        /// <param name="methodBody"></param>
        /// <param name="semicolon2"></param>
        /// <param name="attribues"></param>
        public MethodDeclarationSymbol(Terminal classSymbol, UserAttributes attribues, MethodDeclarationHeadingSymbol heading, Terminal semicolon, MethodDirectivesSymbol directives, BlockSymbol methodBody, Terminal semicolon2) {
            ClassSymbol = classSymbol;
            Attributes = attribues;
            Heading = heading;
            Semicolon = semicolon;
            Directives = directives;
            MethodBody = methodBody;
            Semicolon2 = semicolon2;
        }

        /// <summary>
        ///     class symbol
        /// </summary>
        public Terminal ClassSymbol { get; }

        /// <summary>
        ///     user attributes
        /// </summary>
        public UserAttributes Attributes { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, ClassSymbol, visitor);
            AcceptPart(this, Attributes, visitor);
            AcceptPart(this, Heading, visitor);
            AcceptPart(this, Semicolon, visitor);
            AcceptPart(this, Directives, visitor);
            AcceptPart(this, MethodBody, visitor);
            AcceptPart(this, Semicolon2, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Attributes.GetSymbolLength() +
                Heading.GetSymbolLength() +
                Semicolon.GetSymbolLength() +
                Directives.GetSymbolLength() +
                MethodBody.GetSymbolLength() +
                Semicolon2.GetSymbolLength();

        /// <summary>
        ///     <c>true</c> if class method
        /// </summary>
        public Terminal Class { get; }

        /// <summary>
        ///     method directives
        /// </summary>
        public MethodDirectivesSymbol Directives { get; }

        /// <summary>
        ///     method heading
        /// </summary>
        public MethodDeclarationHeadingSymbol Heading { get; }

        /// <summary>
        ///     method implementation
        /// </summary>
        public BlockSymbol MethodBody { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon2 { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }
    }
}
