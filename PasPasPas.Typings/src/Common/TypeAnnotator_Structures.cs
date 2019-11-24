using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Common {

    public partial class TypeAnnotator {

        /// <summary>
        ///     begin visit a unit
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(CompilationUnit element) {
            var unitType = TypeRegistry.TypeCreator.CreateUnitType(element.SymbolName);
            CurrentUnit = element;
            CurrentUnit.TypeInfo = GetTypeReferenceById(unitType.TypeId);
            resolver.OpenScope();
            resolver.AddToScope(KnownTypeNames.System, ReferenceKind.RefToUnit, environment.TypeRegistry.SystemUnit);
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
                var mainRoutine = TypeCreator.CreateGlobalRoutine(KnownTypeNames.MainMethod);
                mainRoutine.Parameters.Add(new ParameterGroup(mainRoutine, ProcedureKind.Procedure, GetInstanceTypeById(KnownTypeIds.NoType)));
                var unitType = GetTypeByIdOrUndefinedType(CurrentUnit.TypeInfo.TypeId) as IUnitType;
                unitType.Symbols.Add(mainRoutine.Name, new Reference(ReferenceKind.RefToGlobalRoutine, mainRoutine));
                currentMethodImplementation.Push(new RoutineIndex(mainRoutine, 0));
                currentCodeBlock.Push(new CodeBlockBuilder(environment.ListPools));
            }
        }

        /// <summary>
        ///     end visiting a unit
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(BlockOfStatements element) {
            resolver.CloseScope();

            var cmi = currentMethodImplementation.Count > 0 ? currentMethodImplementation.Peek() : default;

            if (cmi != default && string.Equals(cmi.Name, KnownTypeNames.MainMethod, System.StringComparison.OrdinalIgnoreCase)) {
                var mi = currentMethodImplementation.Pop();
                var cb = currentCodeBlock.Pop();
                (mi.Parameters as ParameterGroup).Code = cb.CreateCodeArray();
                cb.Dispose();
            }
        }

    }
}
