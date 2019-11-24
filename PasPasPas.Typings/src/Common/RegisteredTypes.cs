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
            systemUnit = new UnitType(KnownTypeIds.SystemUnit, "System");
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
        ///     register built-in types
        /// </summary>
        private void RegisterCommonTypes(IRuntimeValueFactory runtime, NativeIntSize intSize) {
            RegisterType(new ErrorType(KnownTypeIds.ErrorType));

            RegisterIntTypes();
            RegisterBoolTypes();
            RegisterStringTypes(runtime);
            RegisterRealTypes();
            RegisterPointerTypes();
            RegisterAliasTypes();
            RegisterNativeIntTypes(intSize);
            RegisterHiddenTypes();
            RegisterSystemType(new GenericArrayType(KnownTypeIds.GenericArrayType), "TArray", 1);
            RegisterSystemType(new FileType(KnownTypeIds.UntypedFile, KnownTypeIds.GenericPointer), "");
            RegisterSystemType(new GenericConstraintType(KnownTypeIds.GenericClassConstraint), "");
            RegisterSystemType(new GenericConstraintType(KnownTypeIds.GenericRecordConstraint), "");
            RegisterSystemType(new GenericConstraintType(KnownTypeIds.GenericConstructorConstraint), "");
        }

        private void RegisterHiddenTypes() {
            RegisterSystemType(new UnspecifiedType(KnownTypeIds.UnspecifiedType), null);
            RegisterSystemType(new VoidType(KnownTypeIds.NoType), null);
            RegisterSystemType(new GenericTypeParameter(KnownTypeIds.UnconstrainedGenericTypeParameter, ImmutableArray<int>.Empty), null);
        }

        /// <summary>
        ///     register type alias
        /// </summary>
        private void RegisterAliasTypes() {
            RegisterSystemType(new TypeAlias(KnownTypeIds.CharType, KnownTypeIds.WideCharType), "Char");
            RegisterSystemType(new TypeAlias(KnownTypeIds.Ucs2CharType, KnownTypeIds.WideCharType), "UCS2Char");
            RegisterSystemType(new TypeAlias(KnownTypeIds.Ucs4CharType, KnownTypeIds.CardinalType), "UCS4Char");
            RegisterSystemType(new TypeAlias(KnownTypeIds.StringType, KnownTypeIds.UnicodeStringType), "String");
            RegisterSystemType(new TypeAlias(KnownTypeIds.Real, KnownTypeIds.DoubleType), "Real");
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
            RegisterSystemType(new PointerType(KnownTypeIds.PDouble, KnownTypeIds.DoubleType), "PDouble");
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
            RegisterSystemType(new RealType(KnownTypeIds.Real48Type, 48), "Real48");
            RegisterSystemType(new RealType(KnownTypeIds.SingleType, 32), "Single");
            RegisterSystemType(new RealType(KnownTypeIds.DoubleType, 64), "Double");
            RegisterSystemType(new ExtendedType(KnownTypeIds.Extended), "Extended");
            RegisterSystemType(new RealType(KnownTypeIds.Comp, 64), "Comp");
            RegisterSystemType(new RealType(KnownTypeIds.Currency, 64), "Currency");
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
        private void RegisterStringTypes(IRuntimeValueFactory runtime) {
            RegisterSystemType(new AnsiCharType(KnownTypeIds.AnsiCharType), "AnsiChar");
            RegisterSystemType(new WideCharType(KnownTypeIds.WideCharType), "WideChar");
            RegisterSystemType(new AnsiStringType(KnownTypeIds.AnsiStringType), "AnsiString");
            RegisterSystemType(new AnsiStringType(KnownTypeIds.RawByteString), "RawByteString");
            RegisterSystemType(new ShortStringType(KnownTypeIds.ShortStringType, runtime.Integers.ToIntegerValue(0xff)), "ShortString");
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

            RegisterSystemType(new TypeAlias(KnownTypeIds.Unsigned8BitInteger, KnownTypeIds.ByteType), "UInt8");
            RegisterSystemType(new TypeAlias(KnownTypeIds.Signed8BitInteger, KnownTypeIds.ShortInt), "Int8");
            RegisterSystemType(new TypeAlias(KnownTypeIds.Unsigned16BitInteger, KnownTypeIds.WordType), "UInt16");
            RegisterSystemType(new TypeAlias(KnownTypeIds.Signed16BitInteger, KnownTypeIds.SmallInt), "Int16");
            RegisterSystemType(new TypeAlias(KnownTypeIds.Unsigned32BitInteger, KnownTypeIds.CardinalType), "UInt32");
            RegisterSystemType(new TypeAlias(KnownTypeIds.Signed32BitInteger, KnownTypeIds.IntegerType), "Int32");
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
        private void RegisterTObject() {
            var def = new StructuredTypeDeclaration(KnownTypeIds.TObject, StructuredTypeKind.Class);
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

            return KnownTypeIds.ErrorType;
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

            return KnownTypeIds.ErrorType;
        }

        private int CastRecordTo(int sourceType, int targetType) {
            if (this.AreRecordTypesCompatible(sourceType, targetType))
                return targetType;

            return KnownTypeIds.ErrorType;
        }

        private int CastBooleanTo(int targetType) {
            var targetTypeKind = GetTypeKindOf(targetType);

            if (targetTypeKind == CommonTypeKind.BooleanType)
                return targetType;

            return KnownTypeIds.ErrorType;
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

            return KnownTypeIds.ErrorType;
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
