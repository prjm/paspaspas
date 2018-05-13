using System;
using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Hidden;
using PasPasPas.Typings.Operators;
using PasPasPas.Typings.Routines;
using PasPasPas.Typings.Simple;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     common type registry - contains all system defined types
    /// </summary>
    /// <remarks>
    ///     <list type="bullet">
    ///         <item>each registered type gets an unique id</item>
    ///     </list>
    /// </remarks>
    public class RegisteredTypes : ITypeRegistry, IEnvironmentItem {

        private readonly IDictionary<int, ITypeDefinition> types
            = new Dictionary<int, ITypeDefinition>();

        private readonly IDictionary<int, IOperator> operators
            = new Dictionary<int, IOperator>();

        private readonly UnitType systemUnit;
        private readonly object idLock = new object();

        /// <summary>
        ///     first use type id
        /// </summary>
        public const int SmallestUserTypeId = 1000;
        private int userTypeIds = SmallestUserTypeId;

        /// <summary>
        ///     system unit
        /// </summary>
        public IRefSymbol SystemUnit
            => systemUnit;

        /// <summary>
        ///     number of types
        /// </summary>
        public int Count
            => types.Count;

        /// <summary>
        ///     caption
        /// </summary>
        public string Caption
            => "TypeRegistry";

        /// <summary>
        ///     registered types
        /// </summary>
        public IEnumerable<ITypeDefinition> RegisteredTypeDefinitios
            => types.Values;

        /// <summary>
        ///     runtime constant values
        /// </summary>
        public IRuntimeValueFactory Runtime { get; set; }

        /// <summary>
        ///     register a new type
        /// </summary>
        /// <param name="type">type to register</param>
        public ITypeDefinition RegisterType(ITypeDefinition type) {
            types.Add(type.TypeId, type);

            if (type is TypeBase baseType)
                baseType.TypeRegistry = this;

            return type;
        }

        /// <summary>
        ///     create a new type registry
        /// </summary>
        /// <param name="intSize">integer size</param>
        /// <param name="pool">string pool</param>
        public RegisteredTypes(StringPool pool, NativeIntSize intSize) {
            systemUnit = new UnitType(KnownTypeIds.SystemUnit);
            RegisterType(systemUnit);

            RegisterCommonTypes(intSize);
            RegisterCommonOperators();
            RegisterTObject(pool);
            RegisterCommonFunctions();
        }

        /// <summary>
        ///     register common functions
        /// </summary>
        private void RegisterCommonFunctions() {
            systemUnit.AddGlobal(new Abs(this));
            systemUnit.AddGlobal(new High(this));
        }

        /// <summary>
        ///     register common operators
        /// </summary>
        private void RegisterCommonOperators() {
            LogicalOperator.RegisterOperators(this);
            ArithmeticOperator.RegisterOperators(this);
            RelationalOperators.RegisterOperators(this);
            StringOperators.RegisterOperators(this);
        }

        /// <summary>
        ///     register an operator
        /// </summary>
        /// <param name="newOperator">operator to register</param>
        public void RegisterOperator(IOperator newOperator) {
            operators.Add(newOperator.Kind, newOperator);
            if (newOperator is OperatorBase baseOperator) {
                baseOperator.TypeRegistry = this;
            }
        }

        /// <summary>
        ///     register a system type
        /// </summary>
        /// <param name="typeDef">type definition</param>
        /// <param name="typeName">type name</param>
        private void RegisterSystemType(ITypeDefinition typeDef, string typeName) {
            RegisterType(typeDef);
            if (!string.IsNullOrWhiteSpace(typeName))
                systemUnit.RegisterSymbol(typeName, new Reference(ReferenceKind.RefToType, typeDef));
        }

        /// <summary>
        ///     register built-in types
        /// </summary>
        private void RegisterCommonTypes(NativeIntSize intSize) {
            RegisterType(new ErrorType(KnownTypeIds.ErrorType));

            RegisterIntTypes();
            RegisterBoolTypes();
            RegisterStringTypes();
            RegisterRealTypes();
            RegisterPointerTypes();
            RegisterAliasTypes();
            RegisterNativeIntTypes(intSize);
            RegisterHiddenTypes();
        }

        private void RegisterHiddenTypes() {
            RegisterSystemType(new UnspecifiedType(KnownTypeIds.UnspecifiedType), null);
        }

        /// <summary>
        ///     register type alias
        /// </summary>
        private void RegisterAliasTypes() {
            RegisterSystemType(new TypeAlias(KnownTypeIds.CharType, KnownTypeIds.WideCharType), "Char");
            RegisterSystemType(new TypeAlias(KnownTypeIds.Ucs2CharType, KnownTypeIds.WideCharType), "UCS2Char");
            RegisterSystemType(new TypeAlias(KnownTypeIds.Ucs4CharType, KnownTypeIds.CardinalType), "UCS4Char");
            RegisterSystemType(new TypeAlias(KnownTypeIds.StringType, KnownTypeIds.UnicodeStringType), "String");
            RegisterSystemType(new TypeAlias(KnownTypeIds.Real, KnownTypeIds.Double), "Real");
            RegisterSystemType(new TypeAlias(KnownTypeIds.PChar, KnownTypeIds.PAnsiChar), "PChar");
            RegisterSystemType(new TypeAlias(KnownTypeIds.PString, KnownTypeIds.PUnicodeString), "PString");
        }

        private void RegisterPointerTypes() {
            RegisterSystemType(new PointerType(KnownTypeIds.GenericPointer, KnownTypeIds.UntypedPointer), "Pointer");
            RegisterSystemType(new PointerType(KnownTypeIds.PByte, KnownTypeIds.ByteType), "PByte");
            RegisterSystemType(new PointerType(KnownTypeIds.PShortInt, KnownTypeIds.ShortInt), "PShortInt");
            RegisterSystemType(new PointerType(KnownTypeIds.PWord, KnownTypeIds.WordType), "PWord");
            RegisterSystemType(new PointerType(KnownTypeIds.PSmallInt, KnownTypeIds.SmallInt), "PSmallInt");
            RegisterSystemType(new PointerType(KnownTypeIds.PCardinal, KnownTypeIds.CardinalType), "PCardinal");
            RegisterSystemType(new PointerType(KnownTypeIds.PLongword, KnownTypeIds.LongWord), "PLongword");
            RegisterSystemType(new PointerType(KnownTypeIds.PFixedUint, KnownTypeIds.FixedUInt), "PFixedUint");
            RegisterSystemType(new PointerType(KnownTypeIds.PInteger, KnownTypeIds.IntegerType), "PInteger");
            RegisterSystemType(new PointerType(KnownTypeIds.PLongInt, KnownTypeIds.LongInt), "PLongInt");
            RegisterSystemType(new PointerType(KnownTypeIds.PFixedInt, KnownTypeIds.FixedInt), "PFixedInt");
            RegisterSystemType(new PointerType(KnownTypeIds.PUInt64, KnownTypeIds.Uint64Type), "PUInt64");
            RegisterSystemType(new PointerType(KnownTypeIds.PInt64, KnownTypeIds.CardinalType), "PInt64");
            RegisterSystemType(new PointerType(KnownTypeIds.PNativeUInt, KnownTypeIds.NativeUInt), "PNativeUInt");
            RegisterSystemType(new PointerType(KnownTypeIds.PNativeInt, KnownTypeIds.NativeInt), "PNativeInt");
            RegisterSystemType(new PointerType(KnownTypeIds.PSingle, KnownTypeIds.SingleType), "PSingle");
            RegisterSystemType(new PointerType(KnownTypeIds.PDouble, KnownTypeIds.Double), "PDouble");
            RegisterSystemType(new PointerType(KnownTypeIds.PExtended, KnownTypeIds.Extended), "PExtended");
            RegisterSystemType(new PointerType(KnownTypeIds.PAnsiChar, KnownTypeIds.AnsiCharType), "PAnsiChar");
            RegisterSystemType(new PointerType(KnownTypeIds.PWideChar, KnownTypeIds.WideCharType), "PWideChar");
            RegisterSystemType(new PointerType(KnownTypeIds.PAnsiString, KnownTypeIds.AnsiStringType), "PAnsiString");
            RegisterSystemType(new PointerType(KnownTypeIds.PRawByteString, KnownTypeIds.RawByteString), "PRawByteString");
            RegisterSystemType(new PointerType(KnownTypeIds.PUnicodeString, KnownTypeIds.UnicodeStringType), "PUnicodeString");
            RegisterSystemType(new PointerType(KnownTypeIds.PShortString, KnownTypeIds.ShortStringType), "PShortString");
            RegisterSystemType(new PointerType(KnownTypeIds.PWideString, KnownTypeIds.WideStringType), "PWideString");
            RegisterSystemType(new PointerType(KnownTypeIds.PBoolean, KnownTypeIds.BooleanType), "PBoolean");
            RegisterSystemType(new PointerType(KnownTypeIds.PLongBool, KnownTypeIds.LongBoolType), "PLongBool");
            RegisterSystemType(new PointerType(KnownTypeIds.PWordBool, KnownTypeIds.WordBoolType), "PWordBool");
            RegisterSystemType(new PointerType(KnownTypeIds.PPointer, KnownTypeIds.GenericPointer), "PPointer");
            RegisterSystemType(new PointerType(KnownTypeIds.PCurrency, KnownTypeIds.Currency), "PCurrency");
        }

        /// <summary>
        ///     register real types
        /// </summary>
        private void RegisterRealTypes() {
            RegisterSystemType(new RealType(KnownTypeIds.Real48Type), "Real48");
            RegisterSystemType(new RealType(KnownTypeIds.SingleType), "Single");
            RegisterSystemType(new RealType(KnownTypeIds.Double), "Double");
            RegisterSystemType(new RealType(KnownTypeIds.Extended), "Extended");
            RegisterSystemType(new RealType(KnownTypeIds.Comp), "Comp");
            RegisterSystemType(new RealType(KnownTypeIds.Currency), "Currency");
        }

        /// <summary>
        ///     register native integer types
        /// </summary>
        /// <param name="intSize">integer size</param>
        private void RegisterNativeIntTypes(NativeIntSize intSize) {
            RegisterSystemType(new TypeAlias(KnownTypeIds.FixedInt, KnownTypeIds.IntegerType), "FixedInt");
            RegisterSystemType(new TypeAlias(KnownTypeIds.FixedUInt, KnownTypeIds.CardinalType), "FixedUInt");

            if (intSize == NativeIntSize.Windows64bit) {
                RegisterSystemType(new TypeAlias(KnownTypeIds.NativeInt, KnownTypeIds.Int64Type), "NativeInt");
                RegisterSystemType(new TypeAlias(KnownTypeIds.NativeUInt, KnownTypeIds.Uint64Type), "NativeUInt");
                RegisterSystemType(new TypeAlias(KnownTypeIds.LongInt, KnownTypeIds.IntegerType), "LongInt");
                RegisterSystemType(new TypeAlias(KnownTypeIds.LongWord, KnownTypeIds.CardinalType), "LongWord");
            }
            else if (intSize == NativeIntSize.All64bit) {
                RegisterSystemType(new TypeAlias(KnownTypeIds.NativeInt, KnownTypeIds.Int64Type), "NativeInt");
                RegisterSystemType(new TypeAlias(KnownTypeIds.NativeUInt, KnownTypeIds.Uint64Type), "NativeUInt");
                RegisterSystemType(new TypeAlias(KnownTypeIds.LongInt, KnownTypeIds.Int64Type), "LongInt");
                RegisterSystemType(new TypeAlias(KnownTypeIds.LongWord, KnownTypeIds.Uint64Type), "LongWord");
            }
            else {
                RegisterSystemType(new TypeAlias(KnownTypeIds.NativeInt, KnownTypeIds.IntegerType), "NativeInt");
                RegisterSystemType(new TypeAlias(KnownTypeIds.NativeUInt, KnownTypeIds.CardinalType), "NativeUInt");
                RegisterSystemType(new TypeAlias(KnownTypeIds.LongInt, KnownTypeIds.IntegerType), "LongInt");
                RegisterSystemType(new TypeAlias(KnownTypeIds.LongWord, KnownTypeIds.CardinalType), "LongWord");
            }
        }

        /// <summary>
        ///     register string types
        /// </summary>
        private void RegisterStringTypes() {
            RegisterSystemType(new AnsiCharType(KnownTypeIds.AnsiCharType), "AnsiChar");
            RegisterSystemType(new WideCharType(KnownTypeIds.WideCharType), "WideChar");
            RegisterSystemType(new AnsiStringType(KnownTypeIds.AnsiStringType), "AnsiString");
            RegisterSystemType(new AnsiStringType(KnownTypeIds.RawByteString), "RawByteString");
            RegisterSystemType(new ShortStringType(KnownTypeIds.ShortStringType), "ShortString");
            RegisterSystemType(new UnicodeStringType(KnownTypeIds.UnicodeStringType), "UnicodeString");
            RegisterSystemType(new WideStringType(KnownTypeIds.WideStringType), "WideString");
        }

        /// <summary>
        ///     register boolean types
        /// </summary>
        private void RegisterBoolTypes() {
            RegisterSystemType(new BooleanType(KnownTypeIds.BooleanType, 1), "Boolean");
            RegisterSystemType(new BooleanType(KnownTypeIds.ByteBoolType, 8), "ByteBool");
            RegisterSystemType(new BooleanType(KnownTypeIds.WordBoolType, 16), "WordBool");
            RegisterSystemType(new BooleanType(KnownTypeIds.LongBoolType, 32), "LongBool");
        }

        /// <summary>
        ///     register integer types
        /// </summary>
        private void RegisterIntTypes() {
            RegisterSystemType(new IntegralType(KnownTypeIds.ByteType, false, 8), "Byte");
            RegisterSystemType(new IntegralType(KnownTypeIds.ShortInt, true, 8), "ShortInt");
            RegisterSystemType(new IntegralType(KnownTypeIds.WordType, false, 16), "Word");
            RegisterSystemType(new IntegralType(KnownTypeIds.SmallInt, true, 16), "SmallInt");
            RegisterSystemType(new IntegralType(KnownTypeIds.CardinalType, false, 32), "Cardinal");
            RegisterSystemType(new IntegralType(KnownTypeIds.IntegerType, true, 32), "Integer");
            RegisterSystemType(new Integral64BitType(KnownTypeIds.Uint64Type, false), "UInt64");
            RegisterSystemType(new Integral64BitType(KnownTypeIds.Int64Type, true), "Int64");
        }

        /// <summary>
        ///     gets an registered operator
        /// </summary>
        /// <param name="operatorKind">operator kind</param>
        /// <returns></returns>
        public IOperator GetOperator(int operatorKind) {
            if (operators.TryGetValue(operatorKind, out var result))
                return result;
            return null;
        }

        /// <summary>
        ///     get a type definition or the error fall back
        /// </summary>
        /// <param name="typeId">type id</param>
        /// <returns></returns>
        public ITypeDefinition GetTypeByIdOrUndefinedType(int typeId) {
            if (!types.TryGetValue(typeId, out var result))
                result = types[KnownTypeIds.ErrorType];

            return result;
        }

        /// <summary>
        ///     generate a new user type ids
        /// </summary>
        /// <returns></returns>
        public int RequireUserTypeId() {
            lock (idLock)
                return userTypeIds++;
        }

        /// <summary>
        ///     register the global TObject class
        /// </summary>
        /// <param name="pool"></param>
        private void RegisterTObject(StringPool pool) {
            var def = new StructuredTypeDeclaration(KnownTypeIds.TObject, StructuredTypeKind.Class);
            var meta = new MetaStructuredTypeDeclaration(KnownTypeIds.TClass, KnownTypeIds.TObject);
            RegisterSystemType(def, "TObject");
            RegisterSystemType(meta, "TClass");
            def.MetaType = meta;
            def.AddOrExtendMethod("Create", ProcedureKind.Constructor).AddParameterGroup();
            def.AddOrExtendMethod("Free", ProcedureKind.Procedure).AddParameterGroup();
            def.AddOrExtendMethod("DisposeOf", ProcedureKind.Procedure).AddParameterGroup();
            def.AddOrExtendMethod("CleanupInstance", ProcedureKind.Procedure).AddParameterGroup();
            def.AddOrExtendMethod("ClassType", ProcedureKind.Function).AddParameterGroup(KnownTypeIds.TClass);
            def.AddOrExtendMethod("FieldAddress", ProcedureKind.Function).AddParameterGroup(//
               "Name",
                KnownTypeIds.ShortStringType, //
                KnownTypeIds.GenericPointer)[0].ConstantParam = true;
        }

        /// <summary>
        ///     resolve a type kind
        /// </summary>
        /// <param name="typeId">given type id</param>
        /// <returns>common type kind</returns>
        public CommonTypeKind GetTypeKindOf(int typeId)
            => GetTypeByIdOrUndefinedType(typeId).TypeKind;

        /// <summary>
        ///     create a type reference
        /// </summary>
        /// <param name="typeId">type id</param>
        /// <returns>type reference</returns>
        public ITypeReference MakeReference(int typeId)
            => Runtime.Types.MakeReference(typeId);

        /// <summary>
        ///     get the base type of a subrange type
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public int GetBaseTypeOfSubrangeType(int typeId) {
            if (types.TryGetValue(typeId, out var typeDef) && typeDef is Simple.SubrangeType subrange)
                return subrange.BaseType.TypeId;
            return KnownTypeIds.ErrorType;
        }

        /// <summary>
        ///     cast one type to another type
        /// </summary>
        /// <param name="sourceType">source type</param>
        /// <param name="targetType">target type</param>
        /// <returns></returns>
        public int Cast(int sourceType, int targetType) {
            if (GetTypeKindOf(sourceType).IsIntegral())
                return CastIntTo(sourceType, targetType);

            return KnownTypeIds.ErrorType;
        }

        private int CastIntTo(int sourceType, int targetType) {
            var targetTypeKind = GetTypeKindOf(targetType);

            if (targetTypeKind.IsIntegral())
                return targetType;

            if (targetTypeKind.IsChar())
                return targetType;

            if (targetTypeKind == CommonTypeKind.BooleanType)
                return targetType;

            if (targetTypeKind == CommonTypeKind.EnumerationType)
                return targetType;

            if (targetTypeKind == CommonTypeKind.SubrangeType)
                return targetType;

            return KnownTypeIds.ErrorType;
        }
    }
}
