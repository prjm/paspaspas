using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     enum type value
    /// </summary>
    public class EnumTypeValue : SymbolTableEntryBase, IExpressionTarget {

        /// <summary>
        ///     enum name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     enum expression value
        /// </summary>
        public ExpressionBase Value { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     enum value
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                if (Value != null)
                    yield return Value;
            }
        }

    }
}
