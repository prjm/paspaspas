using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

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
                foreach (var parameter in Parameters.Items)
                    yield return parameter;
                if (TypeValue != null)
                    yield return TypeValue;
                if (Value != null)
                    yield return Value;
                foreach (var accessor in Accessors)
                    yield return accessor;
            }
        }

        /// <summary>
        ///     property type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     expression value (default value)
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     property accessors
        /// </summary>
        public ISyntaxPartList<StructurePropertyAccessor> Accessors { get; }

        /// <summary>
        ///     create a new property of a structured type
        /// </summary>
        public StructureProperty() {
            Accessors = new SyntaxPartCollection<StructurePropertyAccessor>(this);
            Parameters = new ParameterDefinitions() { ParentItem = this };
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }

}
