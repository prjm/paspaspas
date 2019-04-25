using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Hidden {

    /// <summary>
    ///     generic placeholder type
    /// </summary>
    public class GenericPlaceholderType : TypeBase, IExtensibleGenericType {

        /// <summary>
        ///     create a new generic placeholder type
        /// </summary>
        /// <param name="typeId"></param>
        public GenericPlaceholderType(int typeId) : base(typeId) { }

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
        ///     common type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.HiddenType;

        /// <summary>
        ///     undefined type size
        /// </summary>
        public override uint TypeSizeInBytes
            => 0;

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
        public Reference Bind(ImmutableArray<int> typeIds)
            => default;

    }
}
