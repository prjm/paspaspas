using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     scope for identifier validity
    /// </summary>
    public class Scope : IScope {

        private IDictionary<string, Scope> children
            = new Dictionary<string, Scope>(StringComparer.OrdinalIgnoreCase);

        private IDictionary<string, ScopeEntry> entries
            = new Dictionary<string, ScopeEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly Scope parent;

        /// <summary>
        ///     create a new scope
        /// </summary>
        /// <param name="parentScope">parent scope</param>
        public Scope(Scope parentScope)
            => parent = parentScope;

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
        ///     scope name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     open a new child scope
        /// </summary>
        /// <param name="scopeName"></param>
        /// <returns>new child scope</returns>
        public Scope Open(string scopeName)
            => new Scope(this) { Name = scopeName };

        /// <summary>
        ///     close this scope
        /// </summary>
        /// <param name="completeName"></param>
        /// <returns></returns>
        public Scope Close(string completeName) {
            if (parent == null)
                throw new InvalidOperationException();

            if (!string.Equals(completeName, Name, StringComparison.OrdinalIgnoreCase))
                throw new InvalidOperationException();

            return parent;
        }

        /// <summary>
        ///     resolve a scoped name
        /// </summary>
        /// <param name="name">name to resolve</param>
        /// <returns></returns>
        public ScopeEntry ResolveName(ScopedName name) {
            var scope = this;

            while (scope != null) {
                var part = name[0];

                if (name.Length > 1) {
                    var child = this;
                    for (var i = 0; i < name.Length; i++) {
                        part = name[i];
                        if (i + 1 == name.Length && entries.TryGetValue(part, out var entry))
                            return entry;
                        if (scope.children.TryGetValue(part, out var nextChild))
                            child = nextChild;
                        else
                            break;
                    }
                }
                else if (entries.TryGetValue(part, out var entry))
                    return entry;

                scope = scope.parent;
            }

            return null;
        }

        /// <summary>
        ///     a a new entry to this scope
        /// </summary>
        /// <param name="path">named path</param>
        /// <param name="scopeEntry">scope entry</param>
        public void AddEntry(ScopedName path, ScopeEntry scopeEntry) {
            var scope = this;
            for (var i = 0; i < path.Length - 1; i++) {
                if (!scope.children.TryGetValue(path[i], out var childScope)) {
                    childScope = new Scope(scope);
                    scope.children.Add(path[i], childScope);
                }
                scope = childScope;
            }

            var name = path[path.Length - 1];
            scope.entries[name] = scopeEntry;
        }

        /// <summary>
        ///     open a new scope
        /// </summary>
        /// <param name="completeName"></param>
        /// <param name="scope"></param>
        public void Open(string completeName, IScope scope)
            => Open(completeName);
    }
}
