﻿using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     enum type value
    /// </summary>
    public class EnumTypeValue : SymbolTableEntryBase, IExpressionTarget, IRefSymbol, ITypedSyntaxNode {

        /// <summary>
        ///     enum name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     enum expression value
        /// </summary>
        public IExpression Value { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     enum value
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                if (Value != null)
                    yield return Value;
            }
        }


        /// <summary>
        ///     type information
        /// </summary>
        public ITypeReference TypeInfo { get; set; }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => TypeInfo != null ? TypeInfo.TypeId : KnownTypeIds.ErrorType;

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