using System.Collections;
using System.Collections.Generic;

namespace PasPasPas.Api.Options {

    /// <summary>
    ///     option class for derived lists
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DerivedListOption<T> : DerivedOptionBase, IEnumerable<T> {


        /// <summary>
        ///     creates a new derived list
        /// </summary>
        /// <param name="baseOption">base option</param>
        public DerivedListOption(DerivedListOption<T> baseOption) {
            Parent = baseOption;
            IncludeParentValues = true;
        }

        /// <summary>
        ///     list of own values
        /// </summary>
        public IList<T> OwnValues { get; }
            = new List<T>();

        /// <summary>
        ///     test if the default is override    
        /// </summary>
        public override bool OverwritesDefaultValue
            => OwnValues.Count > 0 || (!IncludeParentValues);

        /// <summary>
        ///     parent liust
        /// </summary>
        public DerivedListOption<T> Parent { get; }

        /// <summary>
        ///     switch, if <c>true</c> include parent values in enumeration
        /// </summary>
        public bool IncludeParentValues { get; set; }

        /// <summary>
        ///     clear own values
        /// </summary>
        public override void ResetToDefault() {
            OwnValues.Clear();
        }

        /// <summary>
        ///     enumerate parent values then own values
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator() {
            if (IncludeParentValues && Parent != null) {
                foreach (T entry in Parent)
                    yield return entry;
            }

            foreach (T entry in OwnValues)
                yield return entry;
        }


        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
