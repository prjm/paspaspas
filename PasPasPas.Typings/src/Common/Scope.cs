using System;
using System.Collections.Generic;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     scope for identifier visibility
    /// </summary>
    public class Scope {

        private readonly IOrderedDictionary<string, Reference> entries
            = new OrderedDictionary<string, Reference>(StringComparer.OrdinalIgnoreCase);

        private readonly Scope parent;
        private readonly ITypeRegistry typeRegistry;

        /// <summary>
        ///     defined types
        /// </summary>
        public ITypeRegistry TypeRegistry
            => typeRegistry;

        /// <summary>
        ///     create a new scope
        /// </summary>
        public Scope(ITypeRegistry types) {
            typeRegistry = types;
            parent = null;
        }


        /// <summary>
        ///     create a new scope
        /// </summary>
        /// <param name="parentScope">parent scope</param>
        public Scope(Scope parentScope) {
            parent = parentScope;
            typeRegistry = parentScope.typeRegistry;
        }

        /// <summary>
        ///     gets the root of the scope
        /// </summary>
        public Scope Root {
            get {
                var result = this;
                while (result.parent != null)
                    result = result.parent;
                return result;
            }
        }

        /// <summary>
        ///     get the parent scope
        /// </summary>
        public Scope Parent
            => parent;

        /// <summary>
        ///     list of entries
        /// </summary>
        public IEnumerable<KeyValuePair<string, Reference>> AllEntriesInOrder {
            get {
                var scope = this;
                while (scope != default) {
                    foreach (var entry in scope.entries)
                        yield return entry;
                    scope = scope.Parent;
                }
            }
        }

        /// <summary>
        ///     open a new child scope
        /// </summary>
        /// <returns>new child scope</returns>
        public Scope Open()
            => new Scope(this);

        /// <summary>
        ///     close this scope
        /// </summary>
        /// <returns>parent scope</returns>
        public Scope Close() {
            if (parent == null)
                throw new InvalidOperationException();
            return parent;
        }

        /// <summary>
        ///     try to resolve a name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="reference"></param>
        /// <returns></returns>
        public bool TryToResolve(string name, out Reference reference)
            => entries.TryGetValue(name, out reference);


        /// <summary>
        ///     a a new entry to this scope
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scopeEntry">scope entry</param>
        public void AddEntry(string name, Reference scopeEntry)
            => entries[name] = scopeEntry;

    }
}
