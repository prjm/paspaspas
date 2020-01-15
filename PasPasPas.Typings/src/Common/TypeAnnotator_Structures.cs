using System.Collections.Generic;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Common {

    public partial class TypeAnnotator {

        /// <summary>
        ///     registered routines
        /// </summary>
        public List<(Routine, BlockOfStatements)> Routines
            => routines;

        /// <summary>
        ///     begin visit a unit
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(CompilationUnit element) {
            var unitType = TypeRegistry.TypeCreator.CreateUnitType(element.SymbolName);
            CurrentUnit = element;
            CurrentUnit.TypeInfo = GetTypeReferenceById(unitType.TypeId);
            resolver.OpenScope();
            resolver.AddToScope(KnownNames.System, ReferenceKind.RefToUnit, environment.TypeRegistry.SystemUnit);
        }

        /// <summary>
        ///     end visiting a unit
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(CompilationUnit element) {
            resolver.CloseScope();
            CurrentUnit = null;
        }

        /// <summary>
        ///     begin visit a unit
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(BlockOfStatements element) {
            resolver.OpenScope();
            if (currentMethodImplementation.Count < 1) {
                var mainRoutineGroup = TypeCreator.CreateGlobalRoutine(KnownNames.MainMethod);
                var mainRoutine = new Routine(mainRoutineGroup, RoutineKind.Procedure, GetInstanceTypeById(KnownTypeIds.NoType));
                mainRoutineGroup.Items.Add(mainRoutine);
                var unitType = GetTypeByIdOrUndefinedType(CurrentUnit.TypeInfo.TypeId) as IUnitType;
                unitType.Symbols.Add(mainRoutineGroup.Name, new Reference(ReferenceKind.RefToGlobalRoutine, mainRoutineGroup));
                currentMethodImplementation.Push(new RoutineIndex(mainRoutineGroup, 0));
                RegisterRoutine(mainRoutine, element);
            }
        }

        private void RegisterRoutine(Routine routine, BlockOfStatements block) {
            var entry = (routine, block);
            routines.Add(entry);
        }

        /// <summary>
        ///     end visiting a unit
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(BlockOfStatements element) {
            resolver.CloseScope();

            var cmi = currentMethodImplementation.Count > 0 ? currentMethodImplementation.Peek() : default;

            if (cmi != default && string.Equals(cmi.Name, KnownNames.MainMethod, System.StringComparison.OrdinalIgnoreCase)) {
                var mi = currentMethodImplementation.Pop();
            }
        }

    }
}
