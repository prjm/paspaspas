﻿using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Simple;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     common type creator
    /// </summary>
    public class CommonTypeCreator : ITypeCreator {

        /// <summary>
        ///     create a new type creator
        /// </summary>
        /// <param name="registeredTypes"></param>
        public CommonTypeCreator(RegisteredTypes registeredTypes)
            => RegisteredTypes = registeredTypes;

        private RegisteredTypes RegisteredTypes { get; }

        /// <summary>
        ///     create a new array type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="isPacked"></param>
        /// <returns></returns>
        public IArrayType CreateDynamicArrayType(int baseType, bool isPacked) {
            var typeId = RegisteredTypes.RequireUserTypeId();
            var result = new DynamicArrayType(typeId) {
                BaseTypeId = baseType,
                Packed = isPacked
            };
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new enum type
        /// </summary>
        /// <returns></returns>
        public IEnumeratedType CreateEnumType() {
            var typeId = RegisteredTypes.RequireUserTypeId();
            var result = new EnumeratedType(typeId);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new meta type
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public IMetaStructuredType CreateMetaType(int baseType) {
            var typeId = RegisteredTypes.RequireUserTypeId();
            var result = new MetaStructuredTypeDeclaration(typeId, baseType);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new set type
        /// </summary>
        /// <param name="baseType"></param>
        /// <returns></returns>
        public ISetType CreateSetType(int baseType) {
            var typeId = RegisteredTypes.RequireUserTypeId();
            var result = new SetType(typeId, baseType);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new short string type
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public IShortStringType CreateShortStringType(ITypeReference length) {
            var typeId = RegisteredTypes.RequireUserTypeId();
            var result = new ShortStringType(typeId, length);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new static array type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="indexType"></param>
        /// <param name="isPacked"></param>
        /// <returns></returns>
        public IArrayType CreateStaticArrayType(int baseType, int indexType, bool isPacked) {
            var typeId = RegisteredTypes.RequireUserTypeId();
            var result = new StaticArrayType(typeId, indexType) {
                BaseTypeId = baseType,
                Packed = isPacked
            };
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new structured type
        /// </summary>
        /// <param name="typeKind"></param>
        /// <returns></returns>
        public IStructuredType CreateStructuredType(StructuredTypeKind typeKind) {
            var typeId = RegisteredTypes.RequireUserTypeId();
            var result = new StructuredTypeDeclaration(typeId, typeKind);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new subrange type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        /// <returns></returns>
        public ISubrangeType CreateSubrangeType(int baseType, ITypeReference lowerBound, ITypeReference upperBound) {
            var typeId = RegisteredTypes.RequireUserTypeId();
            var result = new SubrangeType(typeId, baseType, lowerBound, upperBound);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new alias type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="newType"></param>
        /// <returns></returns>
        public IAliasedType CreateTypeAlias(int baseType, bool newType) {
            var typeId = RegisteredTypes.RequireUserTypeId();
            var result = new TypeAlias(typeId, baseType, newType);
            RegisteredTypes.RegisterType(result);
            return result;
        }

    }
}