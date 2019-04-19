using System.Collections.Immutable;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;
using PasPasPas.Runtime.Values.BooleanValues;
using PasPasPas.Runtime.Values.CharValues;
using PasPasPas.Runtime.Values.FloatValues;
using PasPasPas.Runtime.Values.IntValues;
using PasPasPas.Runtime.Values.StringValues;
using PasPasPas.Runtime.Values.Structured;

namespace PasPasPas.Runtime.Values {

    public partial class RuntimeValueFactory {

        /// <summary>
        ///     create a new runtime value factory
        /// </summary>
        public RuntimeValueFactory(IListPools listPools) {
            ListPools = listPools;
            Types = new TypeOperations();
            Booleans = new BooleanOperations();
            Chars = new CharOperations();
            Structured = new StructuredTypeOperations(ListPools, Booleans);
            Integers = new IntegerOperations(Booleans, Types);
            RealNumbers = new RealNumberOperations(Booleans, Integers);
            Strings = new StringOperations(Booleans);
            formatter = new SimpleFormatter(this);
        }

        private readonly SimpleFormatter formatter;

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

        /// <summary>
        ///     string operations: value factory, concatenation
        /// </summary>
        public IStringOperations Strings { get; }

        /// <summary>
        ///     operations on characters
        /// </summary>
        public ICharOperations Chars { get; }

        /// <summary>
        ///     open type operations
        /// </summary>
        public ITypeOperations Types { get; }

        /// <summary>
        ///     structured type operations
        /// </summary>
        public IStructuredTypeOperations Structured { get; }

        /// <summary>
        ///     list pools
        /// </summary>
        public IListPools ListPools { get; }

        /// <summary>
        ///     format a simple type
        /// </summary>
        /// <param name="values">values to format</param>
        /// <returns></returns>
        public ITypeReference FormatExpression(ImmutableArray<ITypeReference> values)
            => formatter.Format(values);

    }
}
