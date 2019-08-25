﻿using System;
using System.Collections.Generic;
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
        /// <param name="flags">flags</param>
        /// <returns></returns>
        public ITypeReference ResolveTypeByName(ITypeReference baseTypeValue, string name, int numberOfTypeArguments = 0, ResolverFlags flags = ResolverFlags.None) {
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
        public Reference ResolveReferenceByName(Reference baseTypeValue, string name, int numberOfTypeArguments = 0) {
            if (baseTypeValue == default)
                return ResolveByName(default, name, numberOfTypeArguments, ResolverFlags.None);
            else
                return ResolveByName(TypeRegistry.MakeTypeInstanceReference(baseTypeValue.Symbol.TypeId), name, numberOfTypeArguments, ResolverFlags.None);
        }

        /// <summary>
        ///     resolve a symbol by name
        /// </summary>
        /// <param name="baseTypeValue"></param>
        /// <param name="name"></param>
        /// <param name="numberOfTypeArguments">number of generic type arguments</param>
        /// <param name="flags">flags</param>
        /// <returns></returns>
        public Reference ResolveByName(ITypeReference baseTypeValue, string name, int numberOfTypeArguments, ResolverFlags flags) {

            if (numberOfTypeArguments > 0)
                name = string.Concat(name, AbstractSyntaxPartBase.GenericSeparator, numberOfTypeArguments);

            if (baseTypeValue == default || baseTypeValue.TypeId == KnownTypeIds.UnspecifiedType) {
                if (scope.TryToResolve(name, out var reference))
                    return reference;

                foreach (var scopeEntry in scope.AllEntriesInOrder) {

                    if (string.Equals(scopeEntry.Key, name, StringComparison.OrdinalIgnoreCase))
                        return scopeEntry.Value;

                    if (scopeEntry.Value.Kind == ReferenceKind.RefToSelf) {
                        var importedEntry = ResolveByName(TypeRegistry.MakeTypeInstanceReference(scopeEntry.Value.Symbol.TypeId), name, 0, flags);
                        if (importedEntry != default)
                            return importedEntry;
                    }

                    if (scopeEntry.Value.Kind == ReferenceKind.RefToUnit) {
                        var importedEntry = ResolveByName(TypeRegistry.MakeTypeInstanceReference(scopeEntry.Value.Symbol.TypeId), name, 0, flags | ResolverFlags.FromAnotherUnit);
                        if (importedEntry != default)
                            return importedEntry;
                    }
                }
            }

            if (baseTypeValue == default)
                return default;

            if (baseTypeValue.TypeKind == CommonTypeKind.Type)
                baseTypeValue = TypeRegistry.MakeTypeInstanceReference(baseTypeValue.TypeId);

            if (baseTypeValue.TypeKind == CommonTypeKind.Unit) {
                var unit = TypeRegistry.GetTypeByIdOrUndefinedType(baseTypeValue.TypeId) as UnitType;
                if (unit != default && unit.TryToResolve(name, out var reference)) {
                    return reference;
                }
            }

            else if (baseTypeValue.TypeKind == CommonTypeKind.ClassType) {
                var cls = TypeRegistry.GetTypeByIdOrUndefinedType(baseTypeValue.TypeId) as StructuredTypeDeclaration;

                if (cls != default && cls.TryToResolve(name, out var reference, flags))
                    return reference;

                while (cls != default && cls.BaseClass != default) {
                    var metaBaseClass = TypeRegistry.GetTypeByIdOrUndefinedType(cls.BaseClass.TypeId) as MetaStructuredTypeDeclaration;
                    if (metaBaseClass == default)
                        return default;

                    var baseClass = TypeRegistry.GetTypeByIdOrUndefinedType(metaBaseClass.BaseType) as StructuredTypeDeclaration;

                    if (baseClass != default && baseClass.TryToResolve(name, out var reference1, ResolverFlags.SkipPrivate))
                        return reference1;

                    cls = baseClass;
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
                return TypeRegistry.MakeTypeInstanceReference(KnownTypeIds.ErrorType);

            var baseTypeValue = TypeRegistry.GetTypeByIdOrUndefinedType(reference.Symbol.TypeId);

            if (reference.Kind == ReferenceKind.RefToConstant)
                return (reference.Symbol as ITypedSyntaxNode)?.TypeInfo;

            if (reference.Kind == ReferenceKind.RefToEnumMember) {
                return (reference.Symbol as EnumValue)?.Value;
            }

            if (reference.Kind == ReferenceKind.RefToGlobalRoutine) {
                if (reference.Symbol is IRoutine routine) {
                    var callableRoutines = new List<ParameterGroup>();
                    routine.ResolveCall(callableRoutines, new Signature());
                    if (callableRoutines.Count == 1)
                        return callableRoutines[0].ResultType;
                }
            }

            if (reference.Kind == ReferenceKind.RefToType) {
                return TypeRegistry.MakeTypeReference(reference.Symbol.TypeId);
            }

            return TypeRegistry.MakeTypeInstanceReference(reference.Symbol.TypeId);
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

        /// <summary>
        ///     find a unit by name
        /// </summary>
        /// <param name="completeName"></param>
        /// <returns></returns>
        public UnitType ResolveUnit(string completeName) {
            foreach (var type in TypeRegistry.RegisteredTypeDefinitios) {
                if (!(type is UnitType unit))
                    continue;

                if (string.Equals(unit.Name, completeName, StringComparison.OrdinalIgnoreCase))
                    return unit;
            }

            return default;
        }
    }
}