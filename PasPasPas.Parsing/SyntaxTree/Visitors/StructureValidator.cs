using PasPasPas.Parsing.SyntaxTree.Abstract;
using System;
using System.IO;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     validate structure elements
    /// </summary>
    public class StructureValidator : SyntaxPartVisitorBase<StructureValidatorOptions> {

        /// <summary>
        ///     visit an element to validate
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool BeginVisit(ISyntaxPart syntaxPart, StructureValidatorOptions parameter) {
            dynamic part = syntaxPart;
            BeginVisitItem(part, parameter);
            return true;
        }

        private void BeginVisitItem(CompilationUnit unit, StructureValidatorOptions parameter) {

            if (!string.Equals(Path.GetFileNameWithoutExtension(unit.FilePath.Path), unit.SymbolName, StringComparison.OrdinalIgnoreCase)) {
                parameter.Log.Error(StructuralErrors.UnitNameDoesNotMatchFileName, unit.FilePath.Path, unit.SymbolName);
            }

        }

        private void Error(StructureValidatorOptions parameter, object data, Guid messageId) {
            parameter.Log.Error(messageId, data);
        }

        private void BeginVisitItem(ISyntaxPart part, StructureValidatorOptions parameter) {
            //..
        }

    }
}
