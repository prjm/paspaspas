using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     helper class to resolve symbol names
    /// </summary>
    public class Resolver {

        private Scope scope;

        /// <summary>
        ///     create a new resolver
        /// </summary>
        /// <param name="currentScope"></param>
        public Resolver(Scope currentScope)
            => scope = currentScope;

        /// <summary>
        ///     resolve a reference by name
        /// </summary>
        /// <param name="typeName">given name</param>
        /// <param name="signature">signature</param>
        /// <returns>resolved reference</returns>
        public Reference ResolveByName(ScopedName typeName, Signature signature = default) {
            var scope = this.scope;

            while (scope != null) {

                if (scope.TryToResolve(typeName.FirstPart, out var entry)) {
                    if (typeName.Length == 1)
                        return entry;

                    return ResolveNameByEntry(typeName.RemoveFirstPart(), entry);
                }

                foreach (var scopeEntry in scope.Entries) {
                    if (scopeEntry.Kind == ReferenceKind.RefToUnit) {
                        var importedEntry = ResolveNameByEntry(typeName, scopeEntry);
                        if (importedEntry != null)
                            return importedEntry;
                    }
                }

                scope = scope.Parent;
            }

            return null;
        }

        /// <summary>
        ///     resolve a name by entry
        /// </summary>
        /// <param name="scopedName">name to resolve</param>
        /// <param name="entry">scope entry</param>
        /// <returns></returns>
        private Reference ResolveNameByEntry(ScopedName scopedName, Reference entry) {
            var type = scope.TypeRegistry.GetTypeByIdOrUndefinedType(entry.Symbol.TypeId);
            var kind = type.TypeKind;

            if (kind == CommonTypeKind.Unit && type is UnitType unit)
                return ResolveNameInUnit(unit, scopedName);
            else if (kind == CommonTypeKind.ClassType && type is StructuredTypeDeclaration structType)
                return ResolveNameInStructuredType(structType, scopedName);
            else if (kind == CommonTypeKind.ClassReferenceType && type is MetaStructuredTypeDeclaration metaType)
                return ResolveNameInMetaType(metaType, scopedName);

            return null;
        }

        private Reference ResolveNameInMetaType(MetaStructuredTypeDeclaration metaType, ScopedName scopedName) {

            if (metaType.TryToResolve(scopedName.FirstPart, out var entry)) {
                if (scopedName.Length == 1)
                    return entry;
            }

            return null;
        }

        private Reference ResolveNameInStructuredType(StructuredTypeDeclaration structType, ScopedName scopedName) {

            if (structType.TryToResolve(scopedName.FirstPart, out var entry)) {
                if (scopedName.Length == 1)
                    return entry;
            }

            return null;
        }

        private Reference ResolveNameInUnit(UnitType unit, ScopedName scopedName) {

            if (unit.TryToResolve(scopedName.FirstPart, out var entry)) {
                if (scopedName.Length == 1)
                    return entry;
            }

            return null;
        }



        /// <summary>
        ///     open a scope
        /// </summary>
        /// <returns>opened scope</returns>
        public void OpenScope()
            => scope = scope.Open();

        /// <summary>
        ///     sclope the current scope
        /// </summary>
        public void CloseScope()
            => scope = scope.Close();

        /// <summary>
        ///     add a symbol to the current scope
        /// </summary>
        /// <param name="symbolName">name of the symbol</param>
        /// <param name="kind">scope kind</param>
        /// <param name="symbol">referenced symbol</param>
        public void AddToScope(string symbolName, ReferenceKind kind, IRefSymbol symbol)
            => scope.AddEntry(symbolName, new Reference(kind, symbol));
    }
}