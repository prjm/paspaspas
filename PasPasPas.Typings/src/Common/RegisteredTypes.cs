using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Hidden;
using PasPasPas.Typings.Operators;
using PasPasPas.Typings.Simple;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Common {

    using Names = KnownNames;

    /// <summary>
    ///     common type registry - contains all system defined types
    /// </summary>
    /// <remarks>
    ///     <list type="bullet">
    ///         <item>each registered type gets an unique id</item>
    ///     </list>
    /// </remarks>
    public class RegisteredTypes : ITypeRegistry, IEnvironmentItem {

        private readonly List<IUnitType> units
            = new List<IUnitType>();

        private readonly IDictionary<int, IOperator> operators
            = new Dictionary<int, IOperator>();

        /// <summary>
        ///     system unit
        /// </summary>
        public ISystemUnit SystemUnit { get; }

        /// <summary>
        ///     runtime constant values
        /// </summary>
        public IRuntimeValueFactory Runtime { get; }

        /// <summary>
        ///     list pools
        /// </summary>
        public IListPools ListPools { get; }

        /// <summary>
        ///     create a new type registry
        /// </summary>
        /// <param name="intSize">integer size</param>
        /// <param name="runtime">runtime values</param>
        /// <param name="listPools">list pools</param>
        public RegisteredTypes(IRuntimeValueFactory runtime, IListPools listPools, NativeIntSize intSize) {
            Runtime = runtime;
            ListPools = listPools;
            SystemUnit = new SystemUnit(this);
            RegisterUnit(SystemUnit);

            RegisterCommonTypes(intSize);
            RegisterCommonOperators();
            RegisterTObject();
        }

        /// <summary>
        ///     register a unit
        /// </summary>
        /// <param name="unitTpe"></param>
        private void RegisterUnit(IUnitType unitTpe)
            => units.Add(unitTpe);

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
        ///     register built-in types
        /// </summary>
        private void RegisterCommonTypes(NativeIntSize intSize) {
            RegisterIntTypes();
            RegisterStringTypes();
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
            RegisterSystemType(new PointerType(Ids.GenericPointer, Ids.UntypedPointer, Names.Pointer));
            RegisterSystemType(new PointerType(Ids.PByte, Ids.ByteType, Names.PByte));
            RegisterSystemType(new PointerType(Ids.PShortInt, Ids.ShortInt, Names.PShortInt));
            RegisterSystemType(new PointerType(Ids.PWord, Ids.WordType, Names.PWord));
            RegisterSystemType(new PointerType(Ids.PSmallInt, Ids.SmallInt, Names.PSmallInt));
            RegisterSystemType(new PointerType(Ids.PCardinal, Ids.CardinalType, Names.PCardinal));
            RegisterSystemType(new PointerType(Ids.PLongword, Ids.LongWord, Names.PLongword));
            RegisterSystemType(new PointerType(Ids.PFixedUint, Ids.FixedUInt, Names.PFixedUint));
            RegisterSystemType(new PointerType(Ids.PInteger, Ids.IntegerType, Names.PInteger));
            RegisterSystemType(new PointerType(Ids.PLongInt, Ids.LongInt, Names.PLongInt));
            RegisterSystemType(new PointerType(Ids.PFixedInt, Ids.FixedInt, Names.PFixedInt));
            RegisterSystemType(new PointerType(Ids.PUInt64, Ids.UInt64Type, Names.PUInt64));
            RegisterSystemType(new PointerType(Ids.PInt64, Ids.Int64Type, Names.PInt64));
            RegisterSystemType(new PointerType(Ids.PNativeUInt, Ids.NativeUInt, Names.PNativeUInt));
            RegisterSystemType(new PointerType(Ids.PNativeInt, Ids.NativeInt, Names.PNativeInt));
            RegisterSystemType(new PointerType(Ids.PSingle, Ids.SingleType, Names.PSingle));
            RegisterSystemType(new PointerType(Ids.PDouble, Ids.DoubleType, Names.PDouble));
            RegisterSystemType(new PointerType(Ids.PExtended, Ids.Extended, Names.PExtended));
            RegisterSystemType(new PointerType(Ids.PAnsiChar, Ids.AnsiCharType, Names.PAnsiChar));
            RegisterSystemType(new PointerType(Ids.PWideChar, Ids.WideCharType, Names.PWideChar));
            RegisterSystemType(new PointerType(Ids.PAnsiString, Ids.AnsiStringType, Names.PAnsiString));
            RegisterSystemType(new PointerType(Ids.PRawByteString, Ids.RawByteString, Names.PRawByteString));
            RegisterSystemType(new PointerType(Ids.PUnicodeString, Ids.UnicodeStringType, Names.PUnicodeString));
            RegisterSystemType(new PointerType(Ids.PShortString, Ids.ShortStringType, Names.PShortString));
            RegisterSystemType(new PointerType(Ids.PWideString, Ids.WideStringType, Names.PWideString));
            RegisterSystemType(new PointerType(Ids.PBoolean, Ids.BooleanType, Names.PBoolean));
            RegisterSystemType(new PointerType(Ids.PLongBool, Ids.LongBoolType, Names.PLongBool));
            RegisterSystemType(new PointerType(Ids.PWordBool, Ids.WordBoolType, Names.PWordBool));
            RegisterSystemType(new PointerType(Ids.PPointer, Ids.GenericPointer, Names.PPointer));
            RegisterSystemType(new PointerType(Ids.PCurrency, Ids.Currency, Names.PCurrency));
        }

        /// <summary>
        ///     register native integer types
        /// </summary>
        /// <param name="intSize">integer size</param>
        private void RegisterNativeIntTypes(NativeIntSize intSize) {
            RegisterSystemAlias(Ids.FixedInt, Ids.IntegerType, Names.FixedInt);
            RegisterSystemAlias(Ids.FixedUInt, Ids.CardinalType, Names.FixedUInt);

            if (intSize == NativeIntSize.Windows64bit) {
                RegisterSystemAlias(Ids.NativeInt, Ids.Int64Type, Names.NativeInt);
                RegisterSystemAlias(Ids.NativeUInt, Ids.UInt64Type, Names.NativeUInt);
                RegisterSystemAlias(Ids.LongInt, Ids.IntegerType, Names.LongInt);
                RegisterSystemAlias(Ids.LongWord, Ids.CardinalType, Names.LongWord);
            }
            else if (intSize == NativeIntSize.All64bit) {
                RegisterSystemAlias(Ids.NativeInt, Ids.Int64Type, Names.NativeInt);
                RegisterSystemAlias(Ids.NativeUInt, Ids.UInt64Type, Names.NativeUInt);
                RegisterSystemAlias(Ids.LongInt, Ids.Int64Type, Names.LongInt);
                RegisterSystemAlias(Ids.LongWord, Ids.UInt64Type, Names.LongWord);
            }
            else {
                RegisterSystemAlias(Ids.NativeInt, Ids.IntegerType, Names.NativeInt);
                RegisterSystemAlias(Ids.NativeUInt, Ids.CardinalType, Names.NativeUInt);
                RegisterSystemAlias(Ids.LongInt, Ids.IntegerType, Names.LongInt);
                RegisterSystemAlias(Ids.LongWord, Ids.CardinalType, Names.LongWord);
            }
        }

        /// <summary>
        ///     register string types
        /// </summary>
        private void RegisterStringTypes() {
            RegisterSystemType(new AnsiCharType(Ids.AnsiCharType));
            RegisterSystemType(new WideCharType(Ids.WideCharType));
            RegisterSystemType(new AnsiStringType(Names.AnsiString, Ids.AnsiStringType, AnsiStringType.DefaultSystemCodePage));
            RegisterSystemType(new AnsiStringType(Names.RawByteString, Ids.RawByteString, AnsiStringType.NoCodePage));
            RegisterSystemType(new ShortStringType(Ids.ShortStringType, 0xff));
            RegisterSystemType(new UnicodeStringType(Ids.UnicodeStringType));
            RegisterSystemType(new WideStringType(Ids.WideStringType));
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
        public IOldTypeReference MakeTypeInstanceReference(int typeId)
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

            return Ids.Unused;
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

            return Ids.Unused;
        }

        private int CastRecordTo(int sourceType, int targetType) {
            if (this.AreRecordTypesCompatible(sourceType, targetType))
                return targetType;

            return Ids.Unused;
        }

        private int CastBooleanTo(int targetType) {
            var targetTypeKind = GetTypeKindOf(targetType);

            if (targetTypeKind == CommonTypeKind.BooleanType)
                return targetType;

            return Ids.Unused;
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

            return Ids.Unused;
        }

        /// <summary>
        ///     make a reference to a type
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public IOldTypeReference MakeTypeReference(int typeId)
            => Runtime.Types.MakeTypeReference(typeId);

        /// <summary>
        ///     find an intrinsic routine from the system unit
        /// </summary>
        /// <param name="routineId"></param>
        /// <returns></returns>
        public IRoutineGroup GetIntrinsicRoutine(IntrinsicRoutineId routineId) {
            var system = SystemUnit as IUnitType;

            foreach (var reference in system.Symbols) {
                if (!(reference is IRoutineGroup routine))
                    continue;

                if (routine.RoutineId == routineId)
                    return routine;
            }

            return default;
        }
    }
}
