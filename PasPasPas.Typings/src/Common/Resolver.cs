using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Simple;
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
        ///     type registry
        /// </summary>
        public ITypeRegistry TypeRegistry
            => scope.TypeRegistry;

        /// <summary>
        ///     resolve a type a given name
        /// </summary>
        /// <param name="baseTypeValue">base type</param>
        /// <param name="name">type name</param>
        /// <param name="numberOfTypeArguments">number of generic type arguments</param>
        /// <returns></returns>
        public ITypeReference ResolveTypeByName(ITypeReference baseTypeValue, string name, int numberOfTypeArguments = 0) {
            var symbolReference = ResolveByName(baseTypeValue, name, numberOfTypeArguments);
            return GetTypeReference(symbolReference);
        }

        /// <summary>
        ///     resolve a reference by name
        /// </summary>
        /// <param name="baseTypeValue"></param>
        /// <param name="name"></param>
        /// <param name="numberOfTypeArguments">number of generic type arguments</param>
        /// <returns></returns>
        public Reference ResolveReferenceByName(Reference baseTypeValue, string name, int numberOfTypeArguments = 0) {
            if (baseTypeValue == default)
                return ResolveByName(default, name, numberOfTypeArguments);
            else
                return ResolveByName(TypeRegistry.MakeReference(baseTypeValue.Symbol.TypeId), name, numberOfTypeArguments);
        }

        /// <summary>
        ///     resolve a symbol by name
        /// </summary>
        /// <param name="baseTypeValue"></param>
        /// <param name="name"></param>
        /// <param name="numberOfTypeArguments">number of generic type arguments</param>
        /// <returns></returns>
        public Reference ResolveByName(ITypeReference baseTypeValue, string name, int numberOfTypeArguments) {

            if (numberOfTypeArguments > 0)
                name = string.Concat(name, AbstractSyntaxPartBase.GenericSeparator, numberOfTypeArguments);

            if (baseTypeValue == default || baseTypeValue.TypeId == KnownTypeIds.UnspecifiedType) {
                if (scope.TryToResolve(name, out var reference))
                    return reference;

                foreach (var scopeEntry in scope.AllEntriesInOrder) {
                    if (scopeEntry.Value.Kind == ReferenceKind.RefToUnit) {
                        var importedEntry = ResolveByName(TypeRegistry.MakeReference(scopeEntry.Value.Symbol.TypeId), name, 0);
                        if (importedEntry != null)
                            return importedEntry;
                    }
                }
            }

            if (baseTypeValue == default)
                return default;

            if (baseTypeValue.TypeKind == CommonTypeKind.Type)
                baseTypeValue = TypeRegistry.MakeReference(baseTypeValue.TypeId);

            if (baseTypeValue.TypeKind == CommonTypeKind.Unit) {
                var unit = TypeRegistry.GetTypeByIdOrUndefinedType(baseTypeValue.TypeId) as UnitType;
                if (unit != default && unit.TryToResolve(name, out var reference)) {
                    return reference;
                }
            }

            else if (baseTypeValue.TypeKind == CommonTypeKind.ClassType) {
                var cls = TypeRegistry.GetTypeByIdOrUndefinedType(baseTypeValue.TypeId) as StructuredTypeDeclaration;

                if (cls != default && cls.TryToResolve(name, out var reference)) {
                    return reference;
                }
            }

            else if (baseTypeValue.TypeKind == CommonTypeKind.MetaClassType) {
                var cls = TypeRegistry.GetTypeByIdOrUndefinedType(baseTypeValue.TypeId) as MetaStructuredTypeDeclaration;
                if (cls != default && cls.TryToResolve(name, out var reference)) {
                    return reference;
                }
            }

            return default;
        }

        private ITypeReference GetTypeReference(Reference reference) {
            if (reference == default || reference.Symbol == default)
                return TypeRegistry.MakeReference(KnownTypeIds.ErrorType);

            var baseTypeValue = TypeRegistry.GetTypeByIdOrUndefinedType(reference.Symbol.TypeId);

            if (reference.Kind == ReferenceKind.RefToConstant)
                return (reference.Symbol as ITypedSyntaxNode)?.TypeInfo;

            if (reference.Kind == ReferenceKind.RefToEnumMember) {
                return (reference.Symbol as EnumValue)?.Value;
            }

            if (reference.Kind == ReferenceKind.RefToType) {
                return TypeRegistry.MakeTypeReference(reference.Symbol.TypeId);
            }

            return TypeRegistry.MakeReference(reference.Symbol.TypeId);
        }

        private Reference ResolveNameInMetaType(MetaStructuredTypeDeclaration metaType, ScopedName scopedName) {

            if (metaType.TryToResolve(scopedName.FirstPart, out var entry)) {
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
        ///     close the current scope
        /// </summary>
        public void CloseScope()
            => scope = scope.Close();

        /// <summary>
        ///     add a symbol to the current scope
        /// </summary>
        /// <param name="symbolName">name of the symbol</param>
        /// <param name="kind">scope kind</param>
        /// <param name="symbol">referenced symbol</param>
        /// <param name="numberOfTypeParameters">number of type parameters</param>
        public void AddToScope(string symbolName, ReferenceKind kind, IRefSymbol symbol, int numberOfTypeParameters = 0) {
            if (numberOfTypeParameters == 0)
                scope.AddEntry(symbolName, new Reference(kind, symbol));
            else
                scope.AddEntry(string.Concat(symbolName, AbstractSyntaxPartBase.GenericSeparator, numberOfTypeParameters), new Reference(kind, symbol));
        }
    }
}