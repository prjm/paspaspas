using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     namespace name
    /// </summary>
    public class NamespaceName : VariableLengthSyntaxTreeBase<SyntaxPartBase> {

        /// <summary>
        ///     create a new namespace name
        /// </summary>
        /// <param name="items"></param>
        public NamespaceName(ImmutableArray<SyntaxPartBase> items) : base(items) {
        }

        /// <summary>
        ///     unit name
        /// </summary>
        public string Name {
            get {
                if (Parts == null || PartList == null || PartList.Count < 1)
                    return null;

                return IdentifierValue(PartList[PartList.Count - 1]);
            }
        }

        /// <summary>
        ///     namespace name
        /// </summary>
        public IEnumerable<string> Namespace {
            get {
                if (Parts == null || PartList == null || PartList.Count < 2)
                    yield break;

                for (var i = 0; i <= PartList.Count - 2; i++) {
                    var part = PartList[i];
                    if (!(part is Identifier))
                        continue;
                    yield return IdentifierValue(part);
                }
            }
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ItemLength;

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

    }

}