﻿using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     declaration of a simple value type
    /// </summary>
    public class EnumType : SymbolTableBase<EnumTypeValue>, ITypeSpecification {

        /// <summary>
        ///     log duplicate enum
        /// </summary>
        /// <param name="newDuplicate"></param>
        /// <param name="logSource"></param>
        protected override void LogDuplicateSymbolError(EnumTypeValue newDuplicate, LogSource logSource) {
            base.LogDuplicateSymbolError(newDuplicate, logSource);
            logSource.Error(StructuralErrors.RedeclaredEnumName, newDuplicate);
        }

    }
}