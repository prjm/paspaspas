using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Hidden;
using PasPasPas.Typings.Operators;
using PasPasPas.Typings.Routines;
using PasPasPas.Typings.Routines.Runtime;
using PasPasPas.Typings.Simple;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Common {

    using Ids = KnownTypeIds;
    using Names = KnownTypeNames;

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
        ///     first used type id
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
        ///     registered types
        /// </summary>
        public IEnumerable<ITypeDefinition> RegisteredTypeDefinitions
            => types.Values;

        /// <summary>
        ///     runtime constant values
        /// </summary>
        public IRuntimeValueFactory Runtime { get; }

        /// <summary>
        ///     list pools
        /// </summary>
        public IListPools ListPools { get; }

        /// <summary>
        ///     type creator
        /// </summary>
        public ITypeCreator TypeCreator { get; }

        /// <summary>
        ///     register a new type
        /// </summary>
        /// <param name="typeDef">type to register</param>
        public ITypeDefinition RegisterType(ITypeDefinition typeDef) {

            if (!types.ContainsKey(typeDef.TypeId))
                types.Add(typeDef.TypeId, typeDef);

            if (typeDef is TypeBase baseType)
                baseType.TypeRegistry = this;

            return typeDef;
        }

        /// <summary>
        ///     create a new type registry
        /// </summary>
        /// <param name="intSize">integer size</param>
        /// <param name="runtime">runtime values</param>
        /// <param name="listPools">list pools</param>
        public RegisteredTypes(IRuntimeValueFactory runtime, IListPools listPools, NativeIntSize intSize) {
            Runtime = runtime;
            ListPools = listPools;
            TypeCreator = new CommonTypeCreator(this);
            systemUnit = new UnitType(Ids.SystemUnit, "System");
            RegisterType(systemUnit);

            RegisterCommonTypes(runtime, intSize);
            RegisterCommonOperators();
            RegisterTObject();
            RegisterCommonFunctions();
        }

        /// <summary>
        ///     register common functions
        /// </summary>
        private void RegisterCommonFunctions() {
            systemUnit.AddGlobal(new Abs());
            systemUnit.AddGlobal(new Chr());
            systemUnit.AddGlobal(new Concat());
            systemUnit.AddGlobal(new HiOrLo(HiLoMode.Hi));
            systemUnit.AddGlobal(new HighOrLow(HighOrLowMode.High));
            systemUnit.AddGlobal(new Length());
            systemUnit.AddGlobal(new HiOrLo(HiLoMode.Lo));
            systemUnit.AddGlobal(new HighOrLow(HighOrLowMode.Low));
            systemUnit.AddGlobal(new MulDivInt64());
            systemUnit.AddGlobal(new Odd());
            systemUnit.AddGlobal(new Ord());
            systemUnit.AddGlobal(new Pi());
            systemUnit.AddGlobal(new PredOrSucc(PredSuccMode.Pred));
            systemUnit.AddGlobal(new PtrRoutine());
            systemUnit.AddGlobal(new Round());
            systemUnit.AddGlobal(new PredOrSucc(PredSuccMode.Succ));
            systemUnit.AddGlobal(new SizeOf());
            systemUnit.AddGlobal(new Sqr());
            systemUnit.AddGlobal(new Swap());
            systemUnit.AddGlobal(new Trunc());

            // dynamic procedures
            systemUnit.AddGlobal(new WriteLn());
        }

        /// <summary>
        ///     register common operators
        /// </summary>
        private void RegisterCommonOperators() {
            LogicalOperator.RegisterOperators(this);
            ArithmeticOperator.RegisterOperators(this);
            RelationalOperator.RegisterOperators(this);
            StringOperator.RegisterOperators(this);
            SetOperator.RegisterOperators(this);
            ClassOperators.RegisterOperators(this);
            OtherOperator.RegisterOperators(this);
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
        /// <param name="numberOfTypeParameters">number of generic type parameters</param>
        private void RegisterSystemType(ITypeDefinition typeDef, string typeName, int numberOfTypeParameters = 0) {
            RegisterType(typeDef);
            if (!string.IsNullOrWhiteSpace(typeName))
                systemUnit.RegisterSymbol(typeName, new Reference(ReferenceKind.RefToType, typeDef), numberOfTypeParameters);
        }

        /// <summary>
        ///     register a type alias name
        /// </summary>
        /// <param name="baseTypeId">base type id</param>
        /// <param name="typeName">alias name</param>
        /// <param name="withId">alias type id</param>
        private void RegisterSystemAlias(int withId, int baseTypeId, string typeName) {
            var alias = new TypeAlias(withId, typeName, baseTypeId);
            RegisterSystemType(alias, alias.AliasName);
        }

        /// <summary>
        ///     register built-in types
        /// </summary>
        private void RegisterCommonTypes(IRuntimeValueFactory runtime, NativeIntSize intSize) {
            RegisterType(new ErrorType(Ids.ErrorType));

            RegisterIntTypes();
            RegisterBoolTypes();
            RegisterStringTypes(runtime);
            RegisterRealTypes();
            RegisterPointerTypes();
            RegisterAliasTypes();
            RegisterNativeIntTypes(intSize);
            RegisterHiddenTypes();
            RegisterSystemType(new GenericArrayType(Ids.GenericArrayType), "TArray", 1);
            RegisterSystemType(new FileType(Ids.UntypedFile, Ids.GenericPointer), "");
            RegisterSystemType(new GenericConstraintType(Ids.GenericClassConstraint), "");
            RegisterSystemType(new GenericConstraintType(Ids.GenericRecordConstraint), "");
            RegisterSystemType(new GenericConstraintType(Ids.GenericConstructorConstraint), "");
        }

        private void RegisterHiddenTypes() {
            RegisterSystemType(new UnspecifiedType(Ids.UnspecifiedType), null);
            RegisterSystemType(new VoidType(Ids.NoType), null);
            RegisterSystemType(new GenericTypeParameter(Ids.UnconstrainedGenericTypeParameter, ImmutableArray<int>.Empty), null);
        }

        /// <summary>
        ///     register type alias
        /// </summary>
        private void RegisterAliasTypes() {
            RegisterSystemAlias(Ids.CharType, Ids.WideCharType, "Char");
            RegisterSystemAlias(Ids.Ucs2CharType, Ids.WideCharType, "UCS2Char");
            RegisterSystemAlias(Ids.Ucs4CharType, Ids.CardinalType, "UCS4Char");
            RegisterSystemAlias(Ids.StringType, Ids.UnicodeStringType, "String");
            RegisterSystemAlias(Ids.Real, Ids.DoubleType, "Real");
            RegisterSystemAlias(Ids.PChar, Ids.PAnsiChar, "PChar");
            RegisterSystemAlias(Ids.PString, Ids.PUnicodeString, "PString");
        }

        private void RegisterPointerTypes() {
            RegisterSystemType(new PointerType(Ids.GenericPointer, Ids.UntypedPointer), "Pointer");
            RegisterSystemType(new PointerType(Ids.PByte, Ids.ByteType), "PByte");
            RegisterSystemType(new PointerType(Ids.PShortInt, Ids.ShortInt), "PShortInt");
            RegisterSystemType(new PointerType(Ids.PWord, Ids.WordType), "PWord");
            RegisterSystemType(new PointerType(Ids.PSmallInt, Ids.SmallInt), "PSmallInt");
            RegisterSystemType(new PointerType(Ids.PCardinal, Ids.CardinalType), "PCardinal");
            RegisterSystemType(new PointerType(Ids.PLongword, Ids.LongWord), "PLongword");
            RegisterSystemType(new PointerType(Ids.PFixedUint, Ids.FixedUInt), "PFixedUint");
            RegisterSystemType(new PointerType(Ids.PInteger, Ids.IntegerType), "PInteger");
            RegisterSystemType(new PointerType(Ids.PLongInt, Ids.LongInt), "PLongInt");
            RegisterSystemType(new PointerType(Ids.PFixedInt, Ids.FixedInt), "PFixedInt");
            RegisterSystemType(new PointerType(Ids.PUInt64, Ids.UInt64Type), "PUInt64");
            RegisterSystemType(new PointerType(Ids.PInt64, Ids.CardinalType), "PInt64");
            RegisterSystemType(new PointerType(Ids.PNativeUInt, Ids.NativeUInt), "PNativeUInt");
            RegisterSystemType(new PointerType(Ids.PNativeInt, Ids.NativeInt), "PNativeInt");
            RegisterSystemType(new PointerType(Ids.PSingle, Ids.SingleType), "PSingle");
            RegisterSystemType(new PointerType(Ids.PDouble, Ids.DoubleType), "PDouble");
            RegisterSystemType(new PointerType(Ids.PExtended, Ids.Extended), "PExtended");
            RegisterSystemType(new PointerType(Ids.PAnsiChar, Ids.AnsiCharType), "PAnsiChar");
            RegisterSystemType(new PointerType(Ids.PWideChar, Ids.WideCharType), "PWideChar");
            RegisterSystemType(new PointerType(Ids.PAnsiString, Ids.AnsiStringType), "PAnsiString");
            RegisterSystemType(new PointerType(Ids.PRawByteString, Ids.RawByteString), "PRawByteString");
            RegisterSystemType(new PointerType(Ids.PUnicodeString, Ids.UnicodeStringType), "PUnicodeString");
            RegisterSystemType(new PointerType(Ids.PShortString, Ids.ShortStringType), "PShortString");
            RegisterSystemType(new PointerType(Ids.PWideString, Ids.WideStringType), "PWideString");
            RegisterSystemType(new PointerType(Ids.PBoolean, Ids.BooleanType), "PBoolean");
            RegisterSystemType(new PointerType(Ids.PLongBool, Ids.LongBoolType), "PLongBool");
            RegisterSystemType(new PointerType(Ids.PWordBool, Ids.WordBoolType), "PWordBool");
            RegisterSystemType(new PointerType(Ids.PPointer, Ids.GenericPointer), "PPointer");
            RegisterSystemType(new PointerType(Ids.PCurrency, Ids.Currency), "PCurrency");
        }

        /// <summary>
        ///     register real types
        /// </summary>
        private void RegisterRealTypes() {
            RegisterSystemType(new RealType(Ids.Real48Type, 48), "Real48");
            RegisterSystemType(new RealType(Ids.SingleType, 32), "Single");
            RegisterSystemType(new RealType(Ids.DoubleType, 64), "Double");
            RegisterSystemType(new ExtendedType(Ids.Extended), "Extended");
            RegisterSystemType(new RealType(Ids.Comp, 64), "Comp");
            RegisterSystemType(new RealType(Ids.Currency, 64), "Currency");
        }

        /// <summary>
        ///     register native integer types
        /// </summary>
        /// <param name="intSize">integer size</param>
        private void RegisterNativeIntTypes(NativeIntSize intSize) {
            RegisterSystemAlias(Ids.FixedInt, Ids.IntegerType, "FixedInt");
            RegisterSystemAlias(Ids.FixedUInt, Ids.CardinalType, "FixedUInt");

            if (intSize == NativeIntSize.Windows64bit) {
                RegisterSystemAlias(Ids.NativeInt, Ids.Int64Type, "NativeInt");
                RegisterSystemAlias(Ids.NativeUInt, Ids.UInt64Type, "NativeUInt");
                RegisterSystemAlias(Ids.LongInt, Ids.IntegerType, "LongInt");
                RegisterSystemAlias(Ids.LongWord, Ids.CardinalType, "LongWord");
            }
            else if (intSize == NativeIntSize.All64bit) {
                RegisterSystemAlias(Ids.NativeInt, Ids.Int64Type, "NativeInt");
                RegisterSystemAlias(Ids.NativeUInt, Ids.UInt64Type, "NativeUInt");
                RegisterSystemAlias(Ids.LongInt, Ids.Int64Type, "LongInt");
                RegisterSystemAlias(Ids.LongWord, Ids.UInt64Type, "LongWord");
            }
            else {
                RegisterSystemAlias(Ids.NativeInt, Ids.IntegerType, "NativeInt");
                RegisterSystemAlias(Ids.NativeUInt, Ids.CardinalType, "NativeUInt");
                RegisterSystemAlias(Ids.LongInt, Ids.IntegerType, "LongInt");
                RegisterSystemAlias(Ids.LongWord, Ids.CardinalType, "LongWord");
            }
        }

        /// <summary>
        ///     register string types
        /// </summary>
        private void RegisterStringTypes(IRuntimeValueFactory runtime) {
            RegisterSystemType(new AnsiCharType(Ids.AnsiCharType), "AnsiChar");
            RegisterSystemType(new WideCharType(Ids.WideCharType), "WideChar");
            RegisterSystemType(new AnsiStringType(Ids.AnsiStringType), "AnsiString");
            RegisterSystemType(new AnsiStringType(Ids.RawByteString), "RawByteString");
            RegisterSystemType(new ShortStringType(Ids.ShortStringType, runtime.Integers.ToIntegerValue(0xff)), "ShortString");
            RegisterSystemType(new UnicodeStringType(Ids.UnicodeStringType), "UnicodeString");
            RegisterSystemType(new WideStringType(Ids.WideStringType), "WideString");
        }

        /// <summary>
        ///     register boolean types
        /// </summary>
        private void RegisterBoolTypes() {
            RegisterSystemType(new BooleanType(Ids.BooleanType, 1), "Boolean");
            RegisterSystemType(new BooleanType(Ids.ByteBoolType, 8), "ByteBool");
            RegisterSystemType(new BooleanType(Ids.WordBoolType, 16), "WordBool");
            RegisterSystemType(new BooleanType(Ids.LongBoolType, 32), "LongBool");
        }

        /// <summary>
        ///     register integer types
        /// </summary>
        private void RegisterIntTypes() {
            RegisterSystemType(new IntegralType(Ids.ByteType, false, 8), Names.Byte);
            RegisterSystemType(new IntegralType(Ids.ShortInt, true, 8), Names.ShortInt);
            RegisterSystemType(new IntegralType(Ids.WordType, false, 16), Names.Word);
            RegisterSystemType(new IntegralType(Ids.SmallInt, true, 16), Names.SmallInt);
            RegisterSystemType(new IntegralType(Ids.CardinalType, false, 32), Names.Cardinal);
            RegisterSystemType(new IntegralType(Ids.IntegerType, true, 32), Names.Integer);
            RegisterSystemType(new Integral64BitType(Ids.UInt64Type, false), Names.UInt64);
            RegisterSystemType(new Integral64BitType(Ids.Int64Type, true), Names.Int64);

            RegisterSystemAlias(Ids.Unsigned8BitInteger, Ids.ByteType, Names.UInt8);
            RegisterSystemAlias(Ids.Signed8BitInteger, Ids.ShortInt, Names.Int8);
            RegisterSystemAlias(Ids.Unsigned16BitInteger, Ids.WordType, Names.UInt16);
            RegisterSystemAlias(Ids.Signed16BitInteger, Ids.SmallInt, Names.Int16);
            RegisterSystemAlias(Ids.Unsigned32BitInteger, Ids.CardinalType, Names.UInt32);
            RegisterSystemAlias(Ids.Signed32BitInteger, Ids.IntegerType, Names.Int32);
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
                result = types[Ids.ErrorType];

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
        private void RegisterTObject() {
            var def = new StructuredTypeDeclaration(Ids.TObject, StructuredTypeKind.Class);
            //var meta = new MetaStructuredTypeDeclaration(KnownTypeIds.TClass, KnownTypeIds.TObject);
            //var alias = TypeCreator.CreateTypeAlias(KnownTypeIds.TClass, false, KnownTypeIds.TClassAlias);
            RegisterSystemType(def, "TObject");
            //RegisterSystemType(meta, "TObject");
            //RegisterSystemType(alias, "TClass");
            //def.MetaType = Runtime.Types.MakeTypeInstanceReference(meta.TypeId, CommonTypeKind.MetaClassType);
            //def.AddOrExtendMethod("Create", ProcedureKind.Constructor).AddParameterGroup();
            //def.AddOrExtendMethod("Free", ProcedureKind.Procedure).AddParameterGroup();
            //def.AddOrExtendMethod("DisposeOf", ProcedureKind.Procedure).AddParameterGroup();
            //def.AddOrExtendMethod("CleanupInstance", ProcedureKind.Procedure).AddParameterGroup();
            //def.AddOrExtendMethod("ClassType", ProcedureKind.Function).AddParameterGroup(KnownTypeIds.TClass);
            //def.AddOrExtendMethod("FieldAddress", ProcedureKind.Function).AddParameterGroup(//
            //   "Name",
            //    KnownTypeIds.ShortStringType, //
            //    KnownTypeIds.GenericPointer)[0].ConstantParam = true;
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
        public ITypeReference MakeTypeInstanceReference(int typeId)
            => Runtime.Types.MakeTypeInstanceReference(typeId, GetTypeKindOf(typeId));

        private int ResolveAlias(int typeId) {
            var typeDef = GetTypeByIdOrUndefinedType(typeId);
            if (typeDef is TypeAlias alias)
                return ResolveAlias(alias.BaseTypeId);
            return typeId;
        }

        /// <summary>
        ///     cast one type to another type
        /// </summary>
        /// <param name="sourceType">source type</param>
        /// <param name="targetType">target type</param>
        /// <returns></returns>
        public int Cast(int sourceType, int targetType) {
            sourceType = ResolveAlias(sourceType);

            if (sourceType == ResolveAlias(targetType))
                return targetType;

            var sourceTypeKind = GetTypeKindOf(sourceType);

            if (sourceTypeKind.IsIntegral())
                return CastIntTo(targetType);

            if (sourceTypeKind.IsChar())
                return CastCharTo(targetType);

            if (sourceTypeKind == CommonTypeKind.BooleanType)
                return CastBooleanTo(targetType);

            if (sourceTypeKind == CommonTypeKind.RecordType)
                return CastRecordTo(sourceType, targetType);

            return Ids.ErrorType;
        }

        private int CastIntTo(int targetType) {
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

            return Ids.ErrorType;
        }

        private int CastRecordTo(int sourceType, int targetType) {
            if (this.AreRecordTypesCompatible(sourceType, targetType))
                return targetType;

            return Ids.ErrorType;
        }

        private int CastBooleanTo(int targetType) {
            var targetTypeKind = GetTypeKindOf(targetType);

            if (targetTypeKind == CommonTypeKind.BooleanType)
                return targetType;

            return Ids.ErrorType;
        }

        private int CastCharTo(int targetType) {
            var targetTypeKind = GetTypeKindOf(targetType);

            if (targetTypeKind.IsIntegral())
                return targetType;

            if (targetTypeKind.IsChar())
                return targetType;

            if (targetTypeKind.IsString())
                return targetType;

            if (targetTypeKind == CommonTypeKind.BooleanType)
                return targetType;

            if (targetTypeKind == CommonTypeKind.EnumerationType)
                return targetType;

            if (targetTypeKind == CommonTypeKind.SubrangeType)
                return targetType;

            return Ids.ErrorType;
        }

        /// <summary>
        ///     make a reference to a type
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ITypeReference MakeTypeReference(int typeId)
            => Runtime.Types.MakeTypeReference(typeId);

        /// <summary>
        ///     find an intrinsic routine from the system unit
        /// </summary>
        /// <param name="routineId"></param>
        /// <returns></returns>
        public IRoutine GetIntrinsicRoutine(IntrinsicRoutineId routineId) {
            var system = SystemUnit as IUnitType;

            foreach (var reference in system.Symbols) {
                if (!(reference.Value.Symbol is IRoutine routine))
                    continue;

                if (routine.RoutineId == routineId)
                    return routine;
            }

            return default;
        }
    }
}
