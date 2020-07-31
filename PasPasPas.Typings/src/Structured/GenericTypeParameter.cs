using System;
using System.Collections.Immutable;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     generic type parameter
    /// </summary>
    internal class GenericTypeParameter : StructuredTypeBase, IGenericTypeParameter {

        /// <summary>
        ///     create a new generic type parameter
        /// </summary>
        /// <param name="definingUnit"></param>
        /// <param name="name"></param>
        /// <param name="constraints"></param>
        public GenericTypeParameter(IUnitType definingUnit, string name, ImmutableArray<ITypeDefinition> constraints) : base(definingUnit) {
            Name = name;
            Constraints = constraints;
        }

        /// <summary>
        ///     base type kind
        /// </summary>
        public override BaseType BaseType
            => BaseType.GenericTypeParameter;

        /// <summary>
        ///     type size
        /// </summary>
        public override uint TypeSizeInBytes
            => 0;

        /// <summary>
        ///     parameter name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     type constraints
        /// </summary>
        public ImmutableArray<ITypeDefinition> Constraints { get; }

        public override bool Equals(ITypeDefinition? other)
            => other is GenericTypeParameter p && KnownNames.SameIdentifier(p.Name, Name);


        public override int GetHashCode()
            => StringComparer.OrdinalIgnoreCase.GetHashCode(Name);
    }
}
