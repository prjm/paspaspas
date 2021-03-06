﻿#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     part of complex symbol references
    /// </summary>
    public class SymbolReferencePart : AbstractSyntaxPartBase, IExpressionTarget, ITypedSyntaxPart {

        /// <summary>
        ///     reference kind
        /// </summary>
        public SymbolReferencePartKind Kind { get; set; }

        /// <summary>
        ///     generic part
        /// </summary>
        public GenericTypeCollection GenericType { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     expressions
        /// </summary>
        public ISyntaxPartCollection<IExpression> Expressions { get; }

        /// <summary>
        ///     create a new symbol reference part
        /// </summary>
        public SymbolReferencePart()
            => Expressions = new SyntaxPartCollection<IExpression>();

        /// <summary>
        ///     expression value
        /// </summary>
        public IExpression Value {
            get => Expressions.LastOrDefault();
            set => Expressions.Add(value);
        }

        /// <summary>
        ///     type info
        /// </summary>
        public ITypeSymbol TypeInfo { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, GenericType, visitor);
            AcceptPart(this, Expressions, visitor);
            visitor.EndVisit(this);
        }
    }
}
