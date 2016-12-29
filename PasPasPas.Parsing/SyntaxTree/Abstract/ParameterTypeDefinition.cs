using PasPasPas.Infrastructure.Log;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     formal parameter definition
    /// </summary>
    public class ParameterTypeDefinition : SymbolTableBase<ParameterDefinition>, ITypeTarget {

        /// <summary>
        ///     parameter type
        /// </summary>
        public ITypeSpecification TypeValue { get; set; }

        /// <summary>
        ///     log duplicate parameter
        /// </summary>
        /// <param name="newDuplicate">duplicate parameter</param>
        /// <param name="logSource">log source</param>
        protected override void LogDuplicateSymbolError(ParameterDefinition newDuplicate, LogSource logSource) {
            logSource.Error(StructuralErrors.DuplicateParameterName, newDuplicate);
        }

        /// <summary>
        ///     check for duplicate parameter names
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public override bool Contains(string key) {
            if (base.Contains(key))
                return true;

            var parameterDefinition = Parent as IParameterTarget;
            foreach (ParameterTypeDefinition parameter in parameterDefinition.Parameters.Parameters)
                if (parameter != this && parameter.Contains(key))
                    return true;

            return false;
        }


        /// <summary>
        ///     enumerate all parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                foreach (ISyntaxPart parameter in base.Parts)
                    yield return parameter;
                if (TypeValue != null)
                    yield return TypeValue;
            }
        }



    }
}