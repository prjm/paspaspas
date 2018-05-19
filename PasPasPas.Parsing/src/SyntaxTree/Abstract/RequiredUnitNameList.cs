﻿using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     list of required units
    /// </summary>
    public class RequiredUnitNameList : SymbolTableBase<RequiredUnitName> {

        /// <summary>
        ///     log duplicated unit name
        /// </summary>
        /// <param name="newDuplicate"></param>
        /// <param name="logSource"></param>
        protected override void LogDuplicateSymbolError(RequiredUnitName newDuplicate, LogSource logSource) {
            base.LogDuplicateSymbolError(newDuplicate, logSource);
            logSource.Error(StructuralErrors.RedeclaredUnitNameInUsesList, newDuplicate);
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}