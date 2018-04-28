﻿using PasPasPas.Global.Runtime;
using PasPasPas.Runtime.Values.Boolean;
using PasPasPas.Runtime.Values.Char;
using PasPasPas.Runtime.Values.Float;
using PasPasPas.Runtime.Values.Int;
using PasPasPas.Runtime.Values.String;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     runtime values: value creation and operations
    /// </summary>
    public class RuntimeValueFactory : IRuntimeValueFactory {

        /// <summary>
        ///     create a new runtime value factory
        /// </summary>
        /// <param name="typeKindResolver">type kind resolver</param>
        public RuntimeValueFactory(ITypeKindResolver typeKindResolver)
            => Types = new TypeOperations(typeKindResolver);


        /// <summary>
        ///     integer operations: value factory and arithmetics
        /// </summary>
        public IIntegerOperations Integers { get; }
            = new IntegerOperations();

        /// <summary>
        ///     real number operations: value factory and arithmetics
        /// </summary>
        public IRealNumberOperations RealNumbers { get; }
            = new RealNumberOperations();

        /// <summary>
        ///     boolean operations: constants, value factory and arithmetics
        /// </summary>
        public IBooleanOperations Booleans { get; }
            = new BooleanOperations();

        /// <summary>
        ///     string operations: value factory, concatenation
        /// </summary>
        public IStringOperations Strings { get; }
            = new StringOperations();

        /// <summary>
        ///     operations on characters
        /// </summary>
        public ICharOperations Chars { get; }
            = new CharOperations();

        /// <summary>
        ///     open type operations
        /// </summary>
        public ITypeOperations Types { get; }

    }
}