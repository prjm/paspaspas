using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     except handler
    /// </summary>
    public class ExceptHandlerSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new exception handler
        /// </summary>
        /// <param name="on"></param>
        /// <param name="name"></param>
        /// <param name="colon"></param>
        /// <param name="type"></param>
        /// <param name="doSymbol"></param>
        /// <param name="statement"></param>
        /// <param name="semicolon"></param>
        public ExceptHandlerSymbol(Terminal on, IdentifierSymbol name, Terminal colon, TypeNameSymbol type, Terminal doSymbol, StatementSymbol statement, Terminal semicolon) {
            On = on;
            Name = name;
            Colon = colon;
            HandlerType = type;
            DoSymbol = doSymbol;
            Statement = statement;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     handler type
        /// </summary>
        public TypeNameSymbol HandlerType { get; }

        /// <summary>
        ///     handler name
        /// </summary>
        public IdentifierSymbol Name { get; }

        /// <summary>
        ///     statement
        /// </summary>
        public StatementSymbol Statement { get; }

        /// <summary>
        ///     on terminal
        /// </summary>
        public Terminal On { get; }

        /// <summary>
        ///      colon symbol
        /// </summary>
        public Terminal Colon { get; }

        /// <summary>
        ///     do symbol
        /// </summary>
        public Terminal DoSymbol { get; }

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
            AcceptPart(this, On, visitor);
            AcceptPart(this, Name, visitor);
            AcceptPart(this, Colon, visitor);
            AcceptPart(this, Statement, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => On.GetSymbolLength() +
                Name.GetSymbolLength() +
                Colon.GetSymbolLength() +
                HandlerType.GetSymbolLength() +
                DoSymbol.GetSymbolLength() +
                Statement.GetSymbolLength() +
                Semicolon.GetSymbolLength();

    }
}