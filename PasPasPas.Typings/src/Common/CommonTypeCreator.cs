using System.Collections.Immutable;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Hidden;
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
        /// <param name="definingUnit"></param>
        public CommonTypeCreator(RegisteredTypes registeredTypes, IUnitType definingUnit) {
            RegisteredTypes = registeredTypes;
            DefiningUnit = definingUnit;
        }

        /// <summary>
        ///     registered types
        /// </summary>
        public RegisteredTypes RegisteredTypes { get; }

        /// <summary>
        ///     defining unit
        /// </summary>
        public IUnitType DefiningUnit { get; }

        /// <summary>
        ///     create a new array type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="isPacked"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public IArrayType CreateDynamicArrayType(ITypeDefinition baseType, string typeName, bool isPacked) {
            var result = new DynamicArrayType(DefiningUnit, typeName) {
                BaseTypeDefinition = baseType,
                Packed = isPacked
            };
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new enumerated type
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IEnumeratedType CreateEnumType(string name) {
            var result = new EnumeratedType(DefiningUnit, name);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new file type
        /// </summary>
        /// <param name="baseTypeDefinition"></param>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public IFileType CreateFileType(string typeName, ITypeDefinition baseTypeDefinition) {
            var result = new FileType(DefiningUnit, typeName, baseTypeDefinition);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new hidden placeholder type
        /// </summary>
        /// <param name="name">type name</param>
        /// <returns></returns>
        public IExtensibleGenericType CreateGenericPlaceholder(string name) {
            var result = new GenericPlaceholderType(name, DefiningUnit);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     manually create a global routine
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IRoutineGroup CreateGlobalRoutine(string name) {
            var routine = new RoutineGroup(DefiningUnit, name);
            return routine;
        }

        /// <summary>
        ///     create a new meta class type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="name">type name</param>
        /// <returns></returns>
        public IMetaClassType CreateMetaClassType(string name, ITypeDefinition baseType) {
            var result = new MetaClassType(DefiningUnit, name, baseType);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a routine type
        /// </summary>
        /// <returns></returns>
        public IRoutineType CreateRoutineType(string name) {
            var result = new RoutineType(DefiningUnit, name);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new set type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="name">type name</param>
        /// <returns></returns>
        public ISetType CreateSetType(IOrdinalType baseType, string name) {
            var result = new SetType(DefiningUnit, name, baseType);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new short string type
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public IShortStringType CreateShortStringType(byte length) {
            var result = new ShortStringType(DefiningUnit, length);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new static array type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="indexType"></param>
        /// <param name="isPacked"></param>
        /// <param name="name">type name</param>
        /// <returns></returns>
        public IArrayType CreateStaticArrayType(ITypeDefinition baseType, string name, IOrdinalType indexType, bool isPacked) {
            var result = new StaticArrayType(name, DefiningUnit, indexType) {
                BaseTypeDefinition = baseType,
                Packed = isPacked
            };
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new structured type
        /// </summary>
        /// <param name="typeKind"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public IStructuredType CreateStructuredType(string name, StructuredTypeKind typeKind) {
            var result = new StructuredTypeDeclaration(DefiningUnit, name, typeKind);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new subrange type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        /// <param name="name"></param>
        /// <returns>new subrange type</returns>
        public ISubrangeType CreateSubrangeType(string name, IOrdinalType baseType, IValue lowerBound, IValue upperBound) {
            var result = new SubrangeType(DefiningUnit, name, baseType, lowerBound, upperBound);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new alias type
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="newType"></param>
        /// <param name="aliasName">alias name</param>
        /// <returns></returns>
        public IAliasedType CreateTypeAlias(ITypeDefinition baseType, string aliasName, bool newType) {
            var result = new TypeAlias(DefiningUnit, baseType, aliasName, newType);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new generic type parameter
        /// </summary>
        /// <param name="constraints"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public IGenericTypeParameter CreateUnboundGenericTypeParameter(string name, ImmutableArray<int> constraints) {
            var result = new GenericTypeParameter(DefiningUnit, name, constraints);
            RegisteredTypes.RegisterType(result);
            return result;
        }

        /// <summary>
        ///     create a new unit type
        /// </summary>
        /// <returns></returns>
        public IUnitType CreateUnitType(string name) {
            var result = new UnitType(name, RegisteredTypes);
            RegisteredTypes.RegisterType(result);
            return result;
        }
    }
}