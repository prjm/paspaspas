using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     scope for identifier visiblity
    /// </summary>
    public class Scope {

        private IOrderedDictionary<string, ScopeEntry> entries
            = new OrderedDictionary<string, ScopeEntry>(StringComparer.OrdinalIgnoreCase);

        private readonly Scope parent;
        private readonly ITypeRegistry typeRegistry;

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
        ///     open a new child scope
        /// </summary>
        /// <param name="scopeName"></param>
        /// <returns>new child scope</returns>
        public Scope Open()
            => new Scope(this);

        /// <summary>
        ///     close this scope
        /// </summary>
        /// <param name="completeName"></param>
        /// <returns></returns>
        public Scope Close() {
            if (parent == null)
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

                if (entries.TryGetValue(name.FirstPart, out var entry)) {
                    if (name.Length == 1)
                        return entry;

                    return ResolveNameByEntry(name.RemoveFirstPart(), entry.TypeId);
                }

                for (var i = scope.entries.Count - 1; i >= 0; i--) {
                    entry = scope.entries[i];
                    if (entry.Kind == ScopeEntryKind.UnitReference) {
                        var importedEntry = ResolveNameByEntry(name, entry.TypeId);
                        if (importedEntry != null)
                            return importedEntry;
                    }
                }

                scope = scope.parent;
            }

            return null;
        }

        /// <summary>
        ///     resolve a name by entry
        /// </summary>
        /// <param name="scopedName"></param>
        /// <param name="typeId">given type id</param>
        /// <returns></returns>
        private ScopeEntry ResolveNameByEntry(ScopedName scopedName, int typeId) {
            var type = typeRegistry.GetTypeByIdOrUndefinedType(typeId);
            var kind = type.TypeKind;

            if (kind == CommonTypeKind.Unit && type is UnitType unit)
                return ResolveNameInUnit(unit, scopedName);

            return null;
        }

        private ScopeEntry ResolveNameInUnit(UnitType unit, ScopedName scopedName) {

            if (unit.TryToResolve(scopedName.FirstPart, out var entry)) {
                if (scopedName.Length == 1)
                    return entry;
            }

            return null;
        }

        /// <summary>
        ///     a a new entry to this scope
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scopeEntry">scope entry</param>
        public void AddEntry(string name, ScopeEntry scopeEntry)
            => entries[name] = scopeEntry;

    }
}
