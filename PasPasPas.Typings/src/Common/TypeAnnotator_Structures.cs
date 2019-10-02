using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;

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

            if (element.FileType == CompilationUnitType.Program) {
                var mainRoutine = TypeCreator.CreateGlobalRoutine(KnownTypeNames.MainMethod, ProcedureKind.Procedure);
                unitType.Symbols.Add(mainRoutine.Name, new Reference(ReferenceKind.RefToGlobalRoutine, mainRoutine));
            }
        }

        /// <summary>
        ///     end visiting a unit
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(CompilationUnit element) {
            resolver.CloseScope();
            CurrentUnit = null;
        }

    }
}
