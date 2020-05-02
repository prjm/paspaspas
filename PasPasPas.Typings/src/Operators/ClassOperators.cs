using System;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Operators {

    /// <summary>
    ///     class operators
    /// </summary>
    public class ClassOperators : OperatorBase {

        /// <summary>
        ///     helper function: register an relational operator
        /// </summary>
        /// <param name="registry">type registry</param>
        /// <param name="kind">operator kind to register</param>
        private static void Register(ITypeRegistry registry, OperatorKind kind)
            => registry.RegisterOperator(new ClassOperators(kind));

        /// <summary>
        ///     register operators
        /// </summary>
        /// <param name="registry"></param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, OperatorKind.AsOperator);
            Register(registry, OperatorKind.IsOperator);
        }

        /// <summary>
        ///     create a new class operator
        /// </summary>
        /// <param name="withKind"></param>
        public ClassOperators(OperatorKind withKind) : base(withKind, 2) { }

        /// <summary>
        ///     get the operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case OperatorKind.AsOperator:
                        return KnownNames.AsSymbol;
                    case OperatorKind.IsOperator:
                        return KnownNames.IsSymbol;
                }
                throw new InvalidOperationException();
            }
        }


        /// <summary>
        ///     evaluate a binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <param name="currentUnit"></param>
        /// <returns></returns>
        protected override ITypeSymbol EvaluateBinaryOperator(ISignature input, IUnitType currentUnit) {

            if (Kind == OperatorKind.IsOperator)
                return EvaluateIsOperator(input);

            if (Kind == OperatorKind.AsOperator)
                return EvaluateAsOperator(input);

            return Invalid;
        }

        private ITypeSymbol EvaluateIsOperator(ISignature input) {
            var classObject = input[0].TypeDefinition as IStructuredType;
            var classType = input[1].TypeDefinition as IStructuredType;

            if (classObject == default || classType == default)
                return Invalid;

            if (classObject.StructTypeKind == StructuredTypeKind.Interface)
                return SystemUnit.BooleanType.Reference;

            if (!TypeRegistry.AreCommonBaseClasses(classObject, classType))
                return Invalid;

            return Invalid;
        }

        private ITypeSymbol EvaluateAsOperator(ISignature input) {
            var classObject = input[0].TypeDefinition as IStructuredType;
            var classType = input[1].TypeDefinition as IStructuredType;

            if (classObject == default || classType == default)
                return Invalid;

            if (!TypeRegistry.AreCommonBaseClasses(classObject, classType))
                return Invalid;

            return classType.Reference;
        }

    }
}
