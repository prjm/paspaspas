using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     namespaced name
    /// </summary>
    public class NamespaceName : SyntaxPartBase {

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
                    ISyntaxPart part = PartList[i];
                    if (!(part is Identifier)) continue;
                    yield return IdentifierValue(part);
                }
            }
        }
    }

}