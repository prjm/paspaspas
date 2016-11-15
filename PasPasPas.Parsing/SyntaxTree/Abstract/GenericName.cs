using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     generic name
    /// </summary>
    public class GenericName : AbstractSyntaxPart {

        private IList<GenericNameFragment> fragments
                = new List<GenericNameFragment>();

        /// <summary>
        ///     add a afragment
        /// </summary>
        /// <param name="fragment"></param>
        public void AddFragment(GenericNameFragment fragment) {
            fragments.Add(fragment);
        }

        /// <summary>
        ///     enumerate all parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                foreach (var part in fragments)
                    yield return part;
            }
        }
    }
}
