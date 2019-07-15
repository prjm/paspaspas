using System;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.AssemblyBuilder.Builder.Net {

    /// <summary>
    ///     type mappings
    /// </summary>
    public class TypeMapper {

        /// <summary>
        ///     create a new type mapper
        /// </summary>
        /// <param name="typeRegistry"></param>
        public TypeMapper(ITypeRegistry typeRegistry)
            => Types = typeRegistry;

        /// <summary>
        ///     type registry
        /// </summary>
        public ITypeRegistry Types { get; }

        /// <summary>
        ///     map  a type to the target
        /// </summary>
        /// <param name="typeRef"></param>
        /// <returns></returns>
        internal Type Map(ITypeReference typeRef) {

            var type = Types.GetTypeByIdOrUndefinedType(typeRef.TypeId);
            var baseType = TypeBase.ResolveAlias(type);
            var value = baseType.TypeId;

            if (value == KnownTypeIds.NoType)
                return typeof(void);

            if (value == KnownTypeIds.ShortInt)
                return typeof(sbyte);

            if (value == KnownTypeIds.SmallInt)
                return typeof(short);

            if (value == KnownTypeIds.IntegerType)
                return typeof(int);

            if (value == KnownTypeIds.Int64Type)
                return typeof(long);

            if (value == KnownTypeIds.ByteType)
                return typeof(byte);

            if (value == KnownTypeIds.WordType)
                return typeof(ushort);

            if (value == KnownTypeIds.CardinalType)
                return typeof(uint);

            if (value == KnownTypeIds.Uint64Type)
                return typeof(ulong);

            if (value == KnownTypeIds.WideCharType)
                return typeof(char);

            if (value == KnownTypeIds.AnsiCharType)
                return typeof(byte);

            if (value == KnownTypeIds.BooleanType)
                return typeof(bool);

            throw new InvalidOperationException();
        }

        /// <summary>
        ///     map a type from the target
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        internal int Map(Type returnType) {
            if (returnType == typeof(void))
                return KnownTypeIds.NoType;

            throw new InvalidOperationException();
        }
    }
}
