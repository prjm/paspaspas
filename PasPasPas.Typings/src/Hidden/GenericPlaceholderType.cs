using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Hidden {

    /// <summary>
    ///     generic placeholder type
    /// </summary>
    internal class GenericPlaceholderType : TypeDefinitionBase, IExtensibleGenericType {

        /// <summary>
        ///     create a new generic placeholder type
        /// </summary>
        /// <param name="definingUnit"></param>
        /// <param name="name"></param>
        internal GenericPlaceholderType(string name, IUnitType definingUnit) : base(definingUnit)
            => Name = name;

        /// <summary>
        ///     parameters
        /// </summary>
        public List<ITypeDefinition> GenericParameters { get; }
        = new List<ITypeDefinition>();

        /// <summary>
        ///     number of generic parameters
        /// </summary>
        public int NumberOfTypeParameters
            => GenericParameters.Count;

        /// <summary>
        ///     undefined type size
        /// </summary>
        public override uint TypeSizeInBytes
            => 0;

        /// <summary>
        ///     type kind
        /// </summary>
        public override BaseType BaseType
            => BaseType.Error;

        /// <summary>
        ///     type name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     add a parameter
        /// </summary>
        /// <param name="typeId"></param>
        public void AddGenericParameter(ITypeDefinition typeId)
            => GenericParameters.Add(typeId);

        /// <summary>
        ///     bind
        /// </summary>
        /// <param name="typeIds"></param>
        /// <param name="typeCreator"></param>
        /// <returns></returns>
        public ITypeDefinition Bind(ImmutableArray<ITypeDefinition> typeIds, ITypeCreator typeCreator)
            => default!;

        public override bool Equals(ITypeDefinition? other)
            => other is GenericPlaceholderType o && string.Equals(Name, o.Name, System.StringComparison.OrdinalIgnoreCase);
    }
}
