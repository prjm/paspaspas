﻿using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     field definitions
    /// </summary>
    public class StructureFieldDefinition : CombinedSymbolTableBase<StructureFields, StructureField> {

        /// <summary>
        ///     log duplicate field
        /// </summary>
        /// <param name="newDuplicate">duplicate parameter</param>
        /// <param name="logSource">log source</param>
        protected override void LogDuplicateSymbolError(StructureField newDuplicate, LogSource logSource) {
            logSource.Error(StructuralErrors.DuplicateFieldName, newDuplicate);
        }

    }
}