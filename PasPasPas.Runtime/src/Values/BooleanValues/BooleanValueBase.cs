﻿using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.BooleanValues {

    /// <summary>
    ///     base class for boolean values
    /// </summary>
    internal abstract class BooleanValueBase : RuntimeValueBase, IBooleanValue, IOrdinalValue {

        /// <summary>
        ///     create a new boolean value
        /// </summary>
        /// <param name="typeDef"></param>
        /// <param name="kind"></param>
        protected BooleanValueBase(ITypeDefinition typeDef, BooleanTypeKind kind) : base(typeDef) {
            if (typeDef.BaseType != BaseType.Boolean)
                throw new ArgumentException(string.Empty, nameof(typeDef));

            if (!(typeDef is IBooleanType booleanType))
                throw new ArgumentException(string.Empty, nameof(typeDef));

            if (booleanType.Kind != kind)
                throw new ArgumentException(string.Empty, nameof(typeDef));
        }

        /// <summary>
        ///     get the boolean value
        /// </summary>
        public abstract bool AsBoolean { get; }

        /// <summary>
        ///     format this value as unsigned integer
        /// </summary>
        public abstract uint AsUint { get; }

        /// <summary>
        ///     boolean value kind
        /// </summary>
        public BooleanTypeKind Kind
            => (TypeDefinition as IBooleanType ?? throw new InvalidOperationException()).Kind;

        internal static bool And(IBooleanValue boolean1, IBooleanValue boolean2)
            => boolean1.AsBoolean && boolean2.AsBoolean;

        internal static bool Not(IBooleanValue boolean)
            => !boolean.AsBoolean;

        internal static bool Or(IBooleanValue boolean1, IBooleanValue boolean2)
            => boolean1.AsBoolean || boolean2.AsBoolean;

        internal static bool Xor(IBooleanValue boolean1, IBooleanValue boolean2)
            => boolean1.AsBoolean ^ boolean2.AsBoolean;

        internal static bool Equal(IBooleanValue boolean1, IBooleanValue boolean2)
            => boolean1.AsBoolean == boolean2.AsBoolean;

        internal static bool GreaterThen(IBooleanValue boolean1, IBooleanValue boolean2)
            => boolean1.AsBoolean && !boolean2.AsBoolean;

        internal static bool LessThenOrEqual(IBooleanValue boolean1, IBooleanValue boolean2)
            => !boolean1.AsBoolean && boolean2.AsBoolean || boolean1.AsBoolean == boolean2.AsBoolean;

        internal static bool NotEquals(IBooleanValue boolean1, IBooleanValue boolean2)
            => boolean1.AsBoolean != boolean2.AsBoolean;

        internal static bool GreaterThenEqual(IBooleanValue boolean1, IBooleanValue boolean2)
            => boolean1.AsBoolean && !boolean2.AsBoolean || boolean1.AsBoolean == boolean2.AsBoolean;

        internal static bool LessThen(IBooleanValue boolean1, IBooleanValue boolean2)
            => !boolean1.AsBoolean && boolean2.AsBoolean;

        /// <summary>
        ///     ordinal value
        /// </summary>
        /// <param name="types"></param>
        /// <returns></returns>
        public abstract IValue GetOrdinalValue(ITypeRegistry types);
    }
}
