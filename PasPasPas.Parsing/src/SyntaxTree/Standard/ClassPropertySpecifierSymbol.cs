using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     class property specifier
    /// </summary>
    public class ClassPropertySpecifierSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new property specifier
        /// </summary>
        /// <param name="classPropertyReadWriteSymbol"></param>
        public ClassPropertySpecifierSymbol(ClassPropertyReadWriteSymbol classPropertyReadWriteSymbol) {
            PropertyReadWrite = classPropertyReadWriteSymbol;
        }

        /// <summary>
        ///     create a new property specifier
        /// </summary>
        /// <param name="classPropertyDispInterfaceSymbols"></param>
        public ClassPropertySpecifierSymbol(ClassPropertyDispInterfaceSymbols classPropertyDispInterfaceSymbols) {
            PropertyDispInterface = classPropertyDispInterfaceSymbols;
        }

        /// <summary>
        ///     create a new class property specifier symbol
        /// </summary>
        /// <param name="terminal"></param>
        /// <param name="expression"></param>
        public ClassPropertySpecifierSymbol(Terminal terminal, ExpressionSymbol expression) {
            StoredSymbol = terminal;
            StoredProperty = expression;
        }

        /// <summary>
        ///     create a new class property specifier symbol
        /// </summary>
        /// <param name="defaultProperty"></param>
        /// <param name="defaultSymbol"></param>
        public ClassPropertySpecifierSymbol(ExpressionSymbol defaultProperty, Terminal defaultSymbol) {
            DefaultProperty = defaultProperty;
            DefaultSymbol = defaultSymbol;
        }

        /// <summary>
        ///     no default symbol
        /// </summary>
        /// <param name="noDefaultSymbol"></param>
        public ClassPropertySpecifierSymbol(Terminal noDefaultSymbol) {
            NoDefaultSymbol = noDefaultSymbol;
        }

        /// <summary>
        ///     create a new class property specifier symbol
        /// </summary>
        /// <param name="implementsSymbol"></param>
        /// <param name="namespaceName"></param>
        public ClassPropertySpecifierSymbol(Terminal implementsSymbol, NamespaceNameSymbol namespaceName) {
            ImplentsSymbol = implementsSymbol;
            ImplementsTypeId = namespaceName;
        }

        /// <summary>
        ///     default property
        /// </summary>
        public bool IsDefault
            => DefaultSymbol != default;

        /// <summary>
        ///     default property expression
        /// </summary>
        public SyntaxPartBase DefaultProperty { get; }

        /// <summary>
        ///     disp interface
        /// </summary>
        public ClassPropertyDispInterfaceSymbols PropertyDispInterface { get; }

        /// <summary>
        ///     read write accessors
        /// </summary>
        public ClassPropertyReadWriteSymbol PropertyReadWrite { get; }

        /// <summary>
        ///     stored property
        /// </summary>
        public bool IsStored
            => StoredSymbol != default;

        /// <summary>
        ///     stored property expression
        /// </summary>
        public ExpressionSymbol StoredProperty { get; }

        /// <summary>
        ///     no default
        /// </summary>
        public bool NoDefault
            => NoDefaultSymbol != default;

        /// <summary>
        ///     implementing type name
        /// </summary>
        public NamespaceNameSymbol ImplementsTypeId { get; }

        /// <summary>
        ///     stored symbol
        /// </summary>
        public Terminal StoredSymbol { get; }

        /// <summary>
        ///     default symbol
        /// </summary>
        public Terminal DefaultSymbol { get; }

        /// <summary>
        ///     no default
        /// </summary>
        public Terminal NoDefaultSymbol { get; }

        /// <summary>
        ///     implements
        /// </summary>
        public Terminal ImplentsSymbol { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, PropertyReadWrite, visitor);
            AcceptPart(this, PropertyDispInterface, visitor);
            AcceptPart(this, StoredSymbol, visitor);
            AcceptPart(this, StoredProperty, visitor);
            AcceptPart(this, DefaultSymbol, visitor);
            AcceptPart(this, DefaultProperty, visitor);
            AcceptPart(this, NoDefaultSymbol, visitor);
            AcceptPart(this, ImplentsSymbol, visitor);
            AcceptPart(this, ImplementsTypeId, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
               => PropertyReadWrite.GetSymbolLength()
                + PropertyDispInterface.GetSymbolLength()
                + StoredSymbol.GetSymbolLength()
                + StoredProperty.GetSymbolLength()
                + DefaultSymbol.GetSymbolLength()
                + DefaultProperty.GetSymbolLength()
                + NoDefaultSymbol.GetSymbolLength()
                + ImplentsSymbol.GetSymbolLength()
                + ImplementsTypeId.GetSymbolLength();

    }
}