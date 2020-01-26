using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     procedural type
    /// </summary>
    public class RoutineType : TypeDefinitionBase, IRoutineType {

        /// <summary>
        ///     create a new routine type
        /// </summary>
        /// <param name="definingType"></param>
        /// <param name="name"></param>
        public RoutineType(IUnitType definingType, string name) : base(definingType)
            => Name = name;

        /// <summary>
        ///     base type kind
        /// </summary>
        public override BaseType BaseType
            => BaseType.Routine;

        /// <summary>
        ///     undefined type size
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.GetPointerSize();

        /// <summary>
        ///     generic parameters
        /// </summary>
        public List<int> GenericParameters
            => new List<int>();

        /// <summary>
        ///     number of generic type parameters
        /// </summary>
        public int NumberOfTypeParameters
            => GenericParameters.Count;

        /// <summary>
        ///     type name
        /// </summary>
        public override string Name { get; }

        /// <summary>
        ///     magled type name
        /// </summary>
        public override string MangledName
            => string.Empty;

        /// <summary>
        ///     add a generic parameter
        /// </summary>
        /// <param name="typeId"></param>
        public void AddGenericParameter(int typeId)
            => GenericParameters.Add(typeId);

        /// <summary>
        ///     bind the generic type parameter
        /// </summary>
        /// <param name="typeIds"></param>
        /// <returns></returns>
        public Reference Bind(ImmutableArray<int> typeIds)
            => default;
    }
}
