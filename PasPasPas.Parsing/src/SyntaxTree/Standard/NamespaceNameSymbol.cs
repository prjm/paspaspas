﻿using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     namespace name
    /// </summary>
    public class NamespaceNameSymbol : VariableLengthSyntaxTreeBase<SyntaxPartBase> {

        /// <summary>
        ///     create a new namespace name
        /// </summary>
        /// <param name="items"></param>
        public NamespaceNameSymbol(ImmutableArray<SyntaxPartBase> items, Terminal comma) : base(items)
            => Comma = comma;

        /// <summary>
        ///     unit name
        /// </summary>
        public string Name => Parts == null || PartList == null || PartList.Count < 1 ? null : IdentifierValue(PartList[PartList.Count - 1]);

        /// <summary>
        ///     namespace name
        /// </summary>
        public IEnumerable<string> Namespace {
            get {
                if (Parts == null || PartList == null || PartList.Count < 2)
                    yield break;

                for (var i = 0; i <= PartList.Count - 2; i++) {
                    var part = PartList[i];
                    if (!(part is IdentifierSymbol))
                        continue;
                    yield return IdentifierValue(part);
                }
            }
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ItemLength + Comma.GetSymbolLength();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

    }

}