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
        private static void Register(ITypeRegistry registry, int kind)
            => registry.RegisterOperator(new ClassOperators(kind));

        /// <summary>
        ///     register operators
        /// </summary>
        /// <param name="registry"></param>
        public static void RegisterOperators(ITypeRegistry registry) {
            Register(registry, DefinedOperators.AsOperator);
            Register(registry, DefinedOperators.IsOperator);
        }

        /// <summary>
        ///     create a new class operator
        /// </summary>
        /// <param name="withKind"></param>
        public ClassOperators(int withKind) : base(withKind, 2) { }

        /// <summary>
        ///     get the operator name
        /// </summary>
        public override string Name {
            get {
                switch (Kind) {
                    case DefinedOperators.AsOperator:
                        return "as";
                    case DefinedOperators.IsOperator:
                        return "is";
                }
                throw new InvalidOperationException();
            }
        }


        /// <summary>
        ///     evaluate a binary operator
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected override ITypeSymbol EvaluateBinaryOperator(Signature input) {

            if (Kind == DefinedOperators.IsOperator)
                return EvaluateIsOperator(input);

            if (Kind == DefinedOperators.AsOperator)
                return EvaluateAsOperator(input);

            return Invalid;
        }

        private ITypeSymbol EvaluateIsOperator(Signature input) {
            var classObject = input[0].TypeDefinition as IStructuredType;
            var classType = input[1].TypeDefinition as IStructuredType;

            if (classObject == default || classType == default)
                return Invalid;

            if (classObject.StructTypeKind == StructuredTypeKind.Interface)
                return SystemUnit.BooleanType;

            if (!TypeRegistry.AreCommonBaseClasses(classObject, classType))
                return Invalid;

            return Invalid;
        }

        private ITypeSymbol EvaluateAsOperator(Signature input) {
            var classObject = input[0].TypeDefinition as IStructuredType;
            var classType = input[1].TypeDefinition as IStructuredType;

            if (classObject == default || classType == default)
                return Invalid;

            if (!TypeRegistry.AreCommonBaseClasses(classObject, classType))
                return Invalid;

            return classType;
        }

    }
}
