using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Typings.Common {
    /// <summary>
    ///     scope entry
    /// </summary>
    public class ScopeEntry {

        /// <summary>
        ///     type kind
        /// </summary>
        private ScopeEntryKind typeKind;

        /// <summary>
        ///     create a new scope kind
        /// </summary>
        /// <param name="kind"></param>
        public ScopeEntry(ScopeEntryKind kind)
            => typeKind = kind;

        /// <summary>
        ///     scope kind
        /// </summary>
        public ScopeEntryKind Kind
            => typeKind;

        /// <summary>
        ///     referenced type id
        /// </summary>
        public int TypeId { get; set; }
    }
}
