using System;
using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Routines;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     virtual unit type definition
    /// </summary>
    public class UnitType : TypeBase, IUnitType {

        /// <summary>
        ///     declared symbols
        /// </summary>
        private readonly IDictionary<string, Reference> symbols
            = new Dictionary<string, Reference>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     global routines
        /// </summary>
        private readonly List<IRoutine> globalRoutines
            = new List<IRoutine>();

        /// <summary>
        ///     global routines
        /// </summary>
        public List<IRoutine> GlobalRoutines
            => globalRoutines;

        /// <summary>
        ///     symbols
        /// </summary>
        public IDictionary<string, Reference> Symbols
            => symbols;

        /// <summary>
        ///     unit type
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="unitName">unit name</param>
        public UnitType(int withId, string unitName) : base(withId)
            => Name = unitName;

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.Unit;

        /// <summary>
        ///     untyped
        /// </summary>
        public override uint TypeSizeInBytes
            => 0;

        /// <summary>
        ///     unit name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     register a symbol
        /// </summary>
        /// <param name="symbolName">symbol name</param>
        /// <param name="entry">defined entry</param>
        /// <param name="numberOfTypeParameters">number of generic type parameters</param>
        public void RegisterSymbol(string symbolName, Reference entry, int numberOfTypeParameters = 0) {
            if (numberOfTypeParameters < 1)
                symbols.Add(symbolName, entry);
            else
                symbols.Add(string.Concat(symbolName, AbstractSyntaxPartBase.GenericSeparator, numberOfTypeParameters), entry);
        }

        /// <summary>
        ///     resolve a symbol in this unit
        /// </summary>
        /// <param name="symbolName">symbol to resolve</param>
        /// <param name="entry">resolved symbol</param>
        /// <returns><c>true</c> if the symbol was resolved</returns>
        public bool TryToResolve(string symbolName, out Reference entry) {
            if (string.IsNullOrWhiteSpace(symbolName)) {
                entry = default;
                return false;
            }

            return symbols.TryGetValue(symbolName, out entry);
        }

        /// <summary>
        ///     add a global routine
        /// </summary>
        /// <param name="routine"></param>
        public void AddGlobal(IRoutine routine) {

            if (routine is IntrinsicRoutine r)
                r.TypeRegistry = TypeRegistry;

            globalRoutines.Add(routine);
            if (symbols.TryGetValue(routine.Name, out var reference))
                if (reference.Kind == ReferenceKind.RefToGlobalRoutine)
                    symbols[routine.Name] = new Reference(ReferenceKind.RefToGlobalRoutine, routine);
                else
                    symbols[routine.Name] = new Reference(ReferenceKind.RefToGlobalRoutine, routine);
            else
                symbols.Add(routine.Name, new Reference(ReferenceKind.RefToGlobalRoutine, routine));
        }

        /// <summary>
        ///     test if a routine with a given name exists
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        public bool HasGlobalRoutine(string symbolName) {
            foreach (var routine in globalRoutines)
                if (string.Equals(routine.Name, symbolName, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        /// <summary>
        ///     add a routine implementation
        /// </summary>
        /// <param name="symbolName"></param>
        /// <param name="routineToImplement"></param>
        /// <returns></returns>
        public bool AddGlobalImplementation(string symbolName, out IRoutine routineToImplement) {
            foreach (var routine in globalRoutines)
                if (string.Equals(routine.Name, symbolName, StringComparison.OrdinalIgnoreCase)) {
                    routineToImplement = routine;
                    return true;
                }

            routineToImplement = default;
            return false;
        }
    }
}
