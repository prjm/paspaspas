using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     property definition
    /// </summary>
    public class StructureProperty : SymbolTableEntryBase, IParameterTarget, ITypeTarget, IExpressionTarget {

        /// <summary>
        ///     assigned attributes
        /// </summary>
        public IList<SymbolAttribute> Attributes { get; set; }

        /// <summary>
        ///     property name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     parameters (indexed property)
        /// </summary>
        public ParameterDefinitions Parameters { get; }
            = new ParameterDefinitions();

        /// <summary>
        ///     property visiblity
        /// </summary>
        public MemberVisibility Visibility { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     enumerate syntax parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (ParameterTypeDefinition parameter in Parameters.Parameters)
                    yield return parameter;
                if (TypeValue != null)
                    yield return TypeValue;
                if (Value != null)
                    yield return Value;
            }
        }

        /// <summary>
        ///     property type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     expression value (default value)
        /// </summary>
        public ExpressionBase Value { get; set; }

    }

}
