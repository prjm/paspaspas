#nullable disable
using System;
using System.Collections.Generic;
using PasPasPas.Globals;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
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
        /// <param name="flags">flags</param>
        /// <returns></returns>
        public ITypeSymbol ResolveTypeByName(ITypeSymbol baseTypeValue, string name, int numberOfTypeArguments = 0, ResolverFlags flags = ResolverFlags.None) {
            var symbolReference = ResolveByName(baseTypeValue, name, numberOfTypeArguments, flags);
            return GetTypeReference(symbolReference);
        }

        /// <summary>
        ///     resolve a reference by name
        /// </summary>
        /// <param name="baseTypeValue"></param>
        /// <param name="name"></param>
        /// <param name="numberOfTypeArguments">number of generic type arguments</param>
        /// <returns></returns>
        public ITypeSymbol ResolveReferenceByName(ITypeSymbol baseTypeValue, string name, int numberOfTypeArguments = 0) {
            if (baseTypeValue == default)
                return ResolveByName(default, name, numberOfTypeArguments, ResolverFlags.None);
            else
                return ResolveByName(baseTypeValue, name, numberOfTypeArguments, ResolverFlags.None);
        }

        /// <summary>
        ///     resolve a symbol by name
        /// </summary>
        /// <param name="baseTypeValue"></param>
        /// <param name="name"></param>
        /// <param name="numberOfTypeArguments">number of generic type arguments</param>
        /// <param name="flags">flags</param>
        /// <returns></returns>
        public ITypeSymbol ResolveByName(ITypeSymbol baseTypeValue, string name, int numberOfTypeArguments, ResolverFlags flags) {

            if (numberOfTypeArguments > 0)
                name = string.Concat(name, AbstractSyntaxPartBase.GenericSeparator, numberOfTypeArguments);

            if (baseTypeValue == default || baseTypeValue.TypeDefinition.Equals(TypeRegistry.SystemUnit.UnspecifiedType)) {
                if (scope.TryToResolve(name, out var reference))
                    return reference;

                foreach (var scopeEntry in scope.AllEntriesInOrder) {

                    if (string.Equals(scopeEntry.Key, name, StringComparison.OrdinalIgnoreCase))
                        return scopeEntry.Value;

                    if (scopeEntry.Value.SymbolKind == SymbolTypeKind.Self) {
                        var importedEntry = ResolveByName(scopeEntry.Value, name, 0, flags);
                        if (importedEntry != default)
                            return importedEntry;
                    }

                    if (scopeEntry.Value.SymbolKind == SymbolTypeKind.SelfClass) {
                        var importedEntry = ResolveByName(scopeEntry.Value, name, 0, flags & ResolverFlags.RequireClassSymbols);
                        if (importedEntry != default)
                            return importedEntry;
                    }

                    if (scopeEntry.Value.GetBaseType() == BaseType.Unit) {
                        var importedEntry = ResolveByName(scopeEntry.Value, name, 0, flags | ResolverFlags.FromAnotherUnit);
                        if (importedEntry != default)
                            return importedEntry;
                    }
                }
            }

            if (baseTypeValue == default)
                return default;

            //if (baseTypeValue.TypeKind == CommonTypeKind.Type)
            //    baseTypeValue = TypeRegistry.MakeTypeInstanceReference(baseTypeValue.TypeId);

            if (baseTypeValue.TypeDefinition.BaseType == BaseType.Unit) {
                var unit = baseTypeValue.TypeDefinition as IUnitType;
                if (unit != default && unit.TryToResolve(name, out var reference)) {
                    return reference;
                }
            }

            else if (baseTypeValue.GetBaseType() == BaseType.Structured) {
                var cls = baseTypeValue.TypeDefinition as StructuredTypeDeclaration;

                if (cls != default && cls.TryToResolve(name, out var reference, flags))
                    return reference;

                while (cls != default) {
                    var baseClass = cls.BaseClass as StructuredTypeDeclaration;

                    if (baseClass != default && baseClass.TryToResolve(name, out var reference1, ResolverFlags.SkipPrivate))
                        return reference1;

                    cls = baseClass;
                }
            }

            return default;
        }

        private ITypeSymbol GetTypeReference(ITypeSymbol reference) {
            if (reference == default)
                return TypeRegistry.SystemUnit.ErrorType.Reference;

            if (reference.SymbolKind == SymbolTypeKind.Constant)
                return (reference as IValue);

            /*
            if (reference.SymbolKind == SymbolTypeKind.RefToEnumMember)
                return (reference.Symbol as EnumValue)?.Value;
                */

            if (reference.SymbolKind == SymbolTypeKind.RoutineGroup) {
                if (reference is IRoutineGroup routine) {
                    var callableRoutines = new List<IRoutineResult>();
                    var signature = TypeRegistry.Runtime.Types.MakeSignature(TypeRegistry.SystemUnit.UnspecifiedType.Reference);
                    routine.ResolveCall(callableRoutines, signature);
                    if (callableRoutines.Count == 1)
                        return callableRoutines[0];
                }
            }

            if (reference.SymbolKind == SymbolTypeKind.TypeDefinition) {
                return reference;
            }

            return reference;
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
        /// <param name="symbol">referenced symbol</param>
        /// <param name="numberOfTypeParameters">number of type parameters</param>
        public void AddToScope(string symbolName, ITypeSymbol symbol, int numberOfTypeParameters = 0) {
            if (numberOfTypeParameters == 0)
                scope.AddEntry(symbolName, symbol);
            else
                scope.AddEntry(string.Concat(symbolName, AbstractSyntaxPartBase.GenericSeparator, numberOfTypeParameters), symbol);
        }

        /// <summary>
        ///     find a unit by name
        /// </summary>
        /// <param name="completeName"></param>
        /// <returns></returns>
        public IUnitType ResolveUnit(string completeName) {
            foreach (var unit in TypeRegistry.Units) {

                if (string.Equals(unit.Name, completeName, StringComparison.OrdinalIgnoreCase))
                    return unit;
            }

            return default;
        }
    }
}