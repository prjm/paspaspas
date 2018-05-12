using PasPasPas.Global.Runtime;
using PasPasPas.Global.Types;
using PasPasPas.Runtime.Values.BooleanValues;
using PasPasPas.Runtime.Values.CharValues;
using PasPasPas.Runtime.Values.FloatValues;
using PasPasPas.Runtime.Values.IntValues;
using PasPasPas.Runtime.Values.StringValues;
using PasPasPas.Typings.Common;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     runtime values: value creation and operations
    /// </summary>
    public class RuntimeValueFactory : IRuntimeValueFactory {

        /// <summary>
        ///     create a new runtime value factory
        /// </summary>
        /// <param name="typeKindResolver">type kind resolver</param>
        public RuntimeValueFactory(ITypeRegistry typeKindResolver) {
            Types = new TypeOperations(typeKindResolver);
            Booleans = new BooleanOperations();
            Integers = new IntegerOperations(Booleans, Types);
            RealNumbers = new RealNumberOperations(Booleans);
            Strings = new StringOperations(Booleans);
        }


        /// <summary>
        ///     integer operations: value factory and arithmetics
        /// </summary>
        public IIntegerOperations Integers { get; }

        /// <summary>
        ///     real number operations: value factory and arithmetics
        /// </summary>
        public IRealNumberOperations RealNumbers { get; }

        /// <summary>
        ///     boolean operations: constants, value factory and arithmetics
        /// </summary>
        public IBooleanOperations Booleans { get; }
            = new BooleanOperations();

        /// <summary>
        ///     string operations: value factory, concatenation
        /// </summary>
        public IStringOperations Strings { get; }

        /// <summary>
        ///     operations on characters
        /// </summary>
        public ICharOperations Chars { get; }
            = new CharOperations();

        /// <summary>
        ///     open type operations
        /// </summary>
        public ITypeOperations Types { get; }

        /// <summary>
        ///     cast values
        /// </summary>
        /// <param name="value"></param>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ITypeReference Cast(ITypeReference value, int typeId) {
            var typeKind = value.TypeKind;

            if (typeKind.IsIntegral())
                return Integers.Cast(value, typeId);

            return Types.MakeReference(KnownTypeIds.ErrorType);
        }

    }
}