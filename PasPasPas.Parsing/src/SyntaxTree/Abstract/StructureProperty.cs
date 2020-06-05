#nullable disable
using System.Collections.Generic;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     property definition
    /// </summary>
    public class StructureProperty : SymbolTableEntryBase, IParameterTarget, ITypeTarget, IExpressionTarget {

        /// <summary>
        ///     assigned attributes
        /// </summary>
        public List<SymbolAttributeItem> Attributes { get; }
            = new List<SymbolAttributeItem>();

        /// <summary>
        ///     property name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     parameters (indexed property)
        /// </summary>
        public ParameterDefinitionCollection Parameters { get; }

        /// <summary>
        ///     property visibility
        /// </summary>
        public MemberVisibility Visibility { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName
            => Name?.CompleteName;

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
        public ISyntaxPartCollection<StructurePropertyAccessor> Accessors { get; }

        /// <summary>
        ///     create a new property of a structured type
        /// </summary>
        public StructureProperty() {
            Accessors = new SyntaxPartCollection<StructurePropertyAccessor>();
            Parameters = new ParameterDefinitionCollection();
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);

            foreach (var parameter in Parameters.Items)
                AcceptPart(this, parameter, visitor);

            AcceptPart(this, TypeValue, visitor);
            AcceptPart(this, Value, visitor);

            foreach (var accessor in Accessors)
                AcceptPart(this, accessor, visitor);

            visitor.EndVisit(this);
        }
    }

}
