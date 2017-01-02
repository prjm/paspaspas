using System.Collections.Generic;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     fields
    /// </summary>
    public class StructureFields : SymbolTableBase<StructureField>, ITypeTarget {

        /// <summary>
        ///     log duplicate field
        /// </summary>
        /// <param name="newDuplicate">duplicate parameter</param>
        /// <param name="logSource">log source</param>
        protected override void LogDuplicateSymbolError(StructureField newDuplicate, LogSource logSource) {
            logSource.Error(StructuralErrors.DuplicateFieldName, newDuplicate);
        }

        /// <summary>
        ///     check for duplicate parameter names
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool Contains(string key) {
            if (base.Contains(key))
                return true;

            var structType = Parent as StructuredType;
            foreach (StructureFields fields in structType.Fields.Fields)
                if (fields != this && fields.Contains(key))
                    return true;

            return false;
        }


        /// <summary>
        ///     enumerate all parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (ISyntaxPart parameter in base.Parts)
                    yield return parameter;
                if (TypeValue != null)
                    yield return TypeValue;
            }
        }

        /// <summary>
        ///     field type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     visibility
        /// </summary>
        public MemberVisibility Visibility { get; set; }
            = MemberVisibility.Public;

        /// <summary>
        ///     hints
        /// </summary>
        public SymbolHints Hints { get; set; }
    }
}
