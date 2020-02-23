using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Hidden {

    /// <summary>
    ///     generic placeholder type
    /// </summary>
    public class GenericPlaceholderType : TypeDefinitionBase, IExtensibleGenericType {

        /// <summary>
        ///     create a new generic placeholder type
        /// </summary>
        /// <param name="definingUnit"></param>
        /// <param name="name"></param>
        public GenericPlaceholderType(string name, IUnitType definingUnit) : base(definingUnit)
            => Name = name;

        /// <summary>
        ///     parameters
        /// </summary>
        public List<int> GenericParameters { get; }
        = new List<int>();

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
        public override string Name { get; }

        /// <summary>
        ///     mangled name
        /// </summary>
        public override string MangledName
            => string.Empty;

        /// <summary>
        ///     add a parameter
        /// </summary>
        /// <param name="typeId"></param>
        public void AddGenericParameter(int typeId)
            => GenericParameters.Add(typeId);

        /// <summary>
        ///     bind
        /// </summary>
        /// <param name="typeIds"></param>
        /// <returns></returns>
        public ITypeDefinition Bind(ImmutableArray<ITypeDefinition> typeIds)
            => default;

    }
}
