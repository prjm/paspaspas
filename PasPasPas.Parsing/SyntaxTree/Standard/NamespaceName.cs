namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     namespaced name
    /// </summary>
    public class NamespaceName : SyntaxPartBase {

        /// <summary>
        ///     unit name
        /// </summary>
        public string Name
        {
            get
            {
                if (Parts == null || PartList.Count < 1)
                    return null;


                return IdentifierValue(PartList[PartList.Count - 1]);
            }
        }

        /// <summary>
        ///     namespace name
        /// </summary>
        public string Namespace
        {
            get
            {
                if (Parts == null || PartList.Count < 2)
                    return null;

                var result = string.Empty;

                for (int i = 0; i <= PartList.Count - 2; i++) {
                    var part = PartList[i];
                    if (!(part is Identifier)) continue;
                    if (i > 0)
                        result = string.Concat(result, ".", IdentifierValue(part));
                    else
                        result = IdentifierValue(part);
                }

                return result;
            }
        }
    }

}