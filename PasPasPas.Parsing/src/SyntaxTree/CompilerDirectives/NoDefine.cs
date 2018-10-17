using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     no define directive
    /// </summary>
    public class NoDefine : CompilerDirectiveBase {
        private readonly Terminal symbol;
        private readonly Terminal typeName;
        private readonly Terminal hppTypeName;
        private readonly Terminal unionName;

        /// <summary>
        ///     create a new no define symbol
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="typeName"></param>
        /// <param name="hppTypeName"></param>
        /// <param name="unionName"></param>
        /// <param name="parsedHppTypeName"></param>
        /// <param name="parsedUnionName"></param>
        public NoDefine(Terminal symbol, Terminal typeName, Terminal hppTypeName, Terminal unionName, string parsedHppTypeName, string parsedUnionName) {
            this.symbol = symbol;
            this.typeName = typeName;
            this.hppTypeName = hppTypeName;
            this.unionName = unionName;
            TypeNameInHpp = parsedHppTypeName;
            TypeNameInUnion = parsedUnionName;
        }

        /// <summary>
        ///     type name
        /// </summary>
        public string TypeName
            => typeName?.Token.Value;

        /// <summary>
        ///     type names in header files
        /// </summary>
        public string TypeNameInHpp { get; }

        /// <summary>
        ///     type name in unions
        /// </summary>
        public string TypeNameInUnion { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, symbol, visitor);
            AcceptPart(this, typeName, visitor);
            AcceptPart(this, hppTypeName, visitor);
            AcceptPart(this, unionName, visitor);
            visitor.EndVisit(this);
        }


    }
}
