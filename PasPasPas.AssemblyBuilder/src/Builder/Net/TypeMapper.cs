using System;
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
        internal Type Map(ITypeSymbol typeRef) {

            var type = typeRef.TypeDefinition;
            var baseType = type.ResolveAlias();
            var value = baseType.BaseType;

            switch (baseType) {
                case INoType _:
                    return typeof(void);

                case IIntegralType integralType:
                    switch (integralType.Kind) {
                        case IntegralTypeKind.ShortInt:
                            return typeof(sbyte);
                        case IntegralTypeKind.SmallInt:
                            return typeof(short);
                        case IntegralTypeKind.Integer:
                            return typeof(int);
                        case IntegralTypeKind.Int64:
                            return typeof(long);
                        case IntegralTypeKind.Byte:
                            return typeof(byte);
                        case IntegralTypeKind.Word:
                            return typeof(ushort);
                        case IntegralTypeKind.Cardinal:
                            return typeof(uint);
                        case IntegralTypeKind.UInt64:
                            return typeof(ulong);
                    }
                    break;

                case ICharType charType:
                    switch (charType.Kind) {
                        case CharTypeKind.AnsiChar:
                            return typeof(byte);
                        case CharTypeKind.WideChar:
                            return typeof(char);
                    }
                    break;

                case IBooleanType booleanType:
                    switch (booleanType.Kind) {
                        case BooleanTypeKind.Boolean:
                            return typeof(bool);
                        case BooleanTypeKind.ByteBool:
                            return typeof(byte);
                        case BooleanTypeKind.WordBool:
                            return typeof(ushort);
                        case BooleanTypeKind.LongBool:
                            return typeof(uint);
                    }
                    break;

            }

            throw new InvalidOperationException();
        }

        /// <summary>
        ///     map a type from the target
        /// </summary>
        /// <param name="returnType"></param>
        /// <returns></returns>
        internal ITypeDefinition Map(Type returnType) {
            if (returnType == typeof(void))
                return Types.SystemUnit.NoType;

            throw new InvalidOperationException();
        }
    }
}
