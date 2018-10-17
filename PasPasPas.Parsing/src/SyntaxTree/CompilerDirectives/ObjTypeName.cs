using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     object type name directive
    /// </summary>
    public class ObjTypeName : CompilerDirectiveBase {
        private readonly Terminal symbol;
        private readonly Terminal name;
        private readonly Terminal aliasName;

        /// <summary>
        ///     create a new object type directive
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="name"></param>
        /// <param name="aliasName"></param>
        /// <param name="typeName"></param>
        /// <param name="parsedAliasName"></param>
        public ObjTypeName(Terminal symbol, Terminal name, Terminal aliasName, string typeName, string parsedAliasName) {
            this.symbol = symbol;
            this.name = name;
            this.aliasName = aliasName;
            TypeName = typeName;
            AliasName = parsedAliasName;
        }

        /// <summary>
        ///     alias name
        /// </summary>
        public string AliasName { get; }

        /// <summary>
        ///     type name in object file
        /// </summary>
        public string TypeName { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, symbol, visitor);
            AcceptPart(this, name, visitor);
            AcceptPart(this, aliasName, visitor);
            visitor.EndVisit(this);
        }


    }
}
