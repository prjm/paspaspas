using System;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     <c>not</c> operation
    /// </summary>
    public class LogicalOperators : OperatorBase {

        /// <summary>
        ///     create a new logical operation
        /// </summary>
        /// <param name="withKind">operation kind</param>
        public LogicalOperators(int withKind) : base(withKind) { }

        /// <summary>
        ///     operation name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case DefinedOperators.AndOperation:
                        return "and";
                    case DefinedOperators.OrOperation:
                        return "or";
                    case DefinedOperators.XorOperation:
                        return "xor";
                    case DefinedOperators.NotOperation:
                        return "not";
                }
                throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     compute the output type of this operation
        /// </summary>
        /// <param name="input">input signature</param>
        /// <returns>output type</returns>
        public override int GetOutputTypeForOperation(Signature input) {
            if (Kind == DefinedOperators.NotOperation && input.EqualsType(TypeRegistry, CommonTypeKind.BooleanType))
                return TypeIds.BooleanType;

            if (Kind == DefinedOperators.AndOperation && input.EqualsType(TypeRegistry, CommonTypeKind.BooleanType, CommonTypeKind.BooleanType))
                return TypeIds.BooleanType;

            if (Kind == DefinedOperators.OrOperation && input.EqualsType(TypeRegistry, CommonTypeKind.BooleanType, CommonTypeKind.BooleanType))
                return TypeIds.BooleanType;

            if (Kind == DefinedOperators.XorOperation && input.EqualsType(TypeRegistry, CommonTypeKind.BooleanType, CommonTypeKind.BooleanType))
                return TypeIds.BooleanType;


            return TypeIds.ErrorType;
        }
    }
}
