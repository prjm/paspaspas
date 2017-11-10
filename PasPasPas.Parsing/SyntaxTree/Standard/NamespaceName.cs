using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     namespaced name
    /// </summary>
    public class NamespaceName : StandardSyntaxTreeBase {

        /// <summary>
        ///     unit name
        /// </summary>
        public string Name {
            get {
                if (Parts == null || PartList.Count < 1)
                    return null;


                return IdentifierValue(PartList[PartList.Count - 1]);
            }
        }

        /// <summary>
        ///     namespace name
        /// </summary>
        public IEnumerable<string> Namespace {
            get {
                if (Parts == null || PartList.Count < 2)
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
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }

}