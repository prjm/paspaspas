using System;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Options.DataTypes;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.Tokenizer.LiteralValues;
using PasPasPas.Typings.Hidden;
using PasPasPas.Typings.Operators;
using PasPasPas.Typings.Routines;
using PasPasPas.Typings.Simple;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     common type registry - contains all system types
    /// </summary>
    public class RegisteredTypes : ITypeRegistry, IEnvironmentItem {

        private readonly IDictionary<int, ITypeDefinition> types
            = new Dictionary<int, ITypeDefinition>();

        private readonly IDictionary<int, IOperator> operators
            = new Dictionary<int, IOperator>();

        private readonly UnitType systemUnit;
        private readonly object idLock = new object();
        private int userTypeIds = 1000;

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
        IEnumerable<ITypeDefinition> ITypeRegistry.RegisteredTypes
            => types.Values;

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
        /// <param name="unwrapper">literal unwrapper</param>
        /// <param name="intParser">integer parser</param>
        public RegisteredTypes(StringPool pool, IIntegerLiteralParser intParser, ILiteralUnwrapper unwrapper, NativeIntSize intSize) {
            systemUnit = new UnitType(TypeIds.SystemUnit);
            RegisterType(systemUnit);

            RegisterCommonTypes(intSize);
            RegisterCommonOperators(unwrapper);
            RegisterTObject(pool);
            RegisterCommonFunctions(intParser);
        }

        /// <summary>
        ///     register common functions
        /// </summary>
        private void RegisterCommonFunctions(IIntegerLiteralParser intParser) {
            systemUnit.AddGlobal(new Abs(this));
            systemUnit.AddGlobal(new High(this, intParser));
        }

        /// <summary>
        ///     register common operators
        /// </summary>
        private void RegisterCommonOperators(ILiteralUnwrapper unwrapper) {
            LogicalOperators.RegisterOperators(this);
            ArithmeticOperators.RegisterOperators(unwrapper, this);
            RelationalOperators.RegisterOperators(this);
            StringOperators.RegisterOperators(this);
        }

        /// <summary>
        ///     register an operator
        /// </summary>
        /// <param name="newOperator">operator to register</param>
        public void RegisterOperator(IOperator newOperator) {
            operators.Add(newOperator.Kind, newOperator);
            newOperator.TypeRegistry = this;
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
            RegisterType(new ErrorType(TypeIds.ErrorType));

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
            RegisterSystemType(new UnspecifiedType(TypeIds.UnspecifiedType), null);
        }

        /// <summary>
        ///     register type alias
        /// </summary>
        private void RegisterAliasTypes() {
            RegisterSystemType(new TypeAlias(TypeIds.CharType, TypeIds.WideCharType), "Char");
            RegisterSystemType(new TypeAlias(TypeIds.Ucs2CharType, TypeIds.WideCharType), "UCS2Char");
            RegisterSystemType(new TypeAlias(TypeIds.Ucs4CharType, TypeIds.CardinalType), "UCS4Char");
            RegisterSystemType(new TypeAlias(TypeIds.StringType, TypeIds.UnicodeStringType), "String");
            RegisterSystemType(new TypeAlias(TypeIds.Real, TypeIds.Double), "Real");
            RegisterSystemType(new TypeAlias(TypeIds.PChar, TypeIds.PAnsiChar), "PChar");
            RegisterSystemType(new TypeAlias(TypeIds.PString, TypeIds.PUnicodeString), "PString");
        }

        private void RegisterPointerTypes() {
            RegisterSystemType(new PointerType(TypeIds.GenericPointer, TypeIds.UntypedPointer), "Pointer");
            RegisterSystemType(new PointerType(TypeIds.PByte, TypeIds.ByteType), "PByte");
            RegisterSystemType(new PointerType(TypeIds.PShortInt, TypeIds.ShortInt), "PShortInt");
            RegisterSystemType(new PointerType(TypeIds.PWord, TypeIds.WordType), "PWord");
            RegisterSystemType(new PointerType(TypeIds.PSmallInt, TypeIds.SmallInt), "PSmallInt");
            RegisterSystemType(new PointerType(TypeIds.PCardinal, TypeIds.CardinalType), "PCardinal");
            RegisterSystemType(new PointerType(TypeIds.PLongword, TypeIds.LongWord), "PLongword");
            RegisterSystemType(new PointerType(TypeIds.PFixedUint, TypeIds.FixedUInt), "PFixedUint");
            RegisterSystemType(new PointerType(TypeIds.PInteger, TypeIds.IntegerType), "PInteger");
            RegisterSystemType(new PointerType(TypeIds.PLongInt, TypeIds.LongInt), "PLongInt");
            RegisterSystemType(new PointerType(TypeIds.PFixedInt, TypeIds.FixedInt), "PFixedInt");
            RegisterSystemType(new PointerType(TypeIds.PUInt64, TypeIds.Uint64Type), "PUInt64");
            RegisterSystemType(new PointerType(TypeIds.PInt64, TypeIds.CardinalType), "PInt64");
            RegisterSystemType(new PointerType(TypeIds.PNativeUInt, TypeIds.NativeUInt), "PNativeUInt");
            RegisterSystemType(new PointerType(TypeIds.PNativeInt, TypeIds.NativeInt), "PNativeInt");
            RegisterSystemType(new PointerType(TypeIds.PSingle, TypeIds.SingleType), "PSingle");
            RegisterSystemType(new PointerType(TypeIds.PDouble, TypeIds.Double), "PDouble");
            RegisterSystemType(new PointerType(TypeIds.PExtended, TypeIds.Extended), "PExtended");
            RegisterSystemType(new PointerType(TypeIds.PAnsiChar, TypeIds.AnsiCharType), "PAnsiChar");
            RegisterSystemType(new PointerType(TypeIds.PWideChar, TypeIds.WideCharType), "PWideChar");
            RegisterSystemType(new PointerType(TypeIds.PAnsiString, TypeIds.AnsiStringType), "PAnsiString");
            RegisterSystemType(new PointerType(TypeIds.PRawByteString, TypeIds.RawByteString), "PRawByteString");
            RegisterSystemType(new PointerType(TypeIds.PUnicodeString, TypeIds.UnicodeStringType), "PUnicodeString");
            RegisterSystemType(new PointerType(TypeIds.PShortString, TypeIds.ShortStringType), "PShortString");
            RegisterSystemType(new PointerType(TypeIds.PWideString, TypeIds.WideStringType), "PWideString");
            RegisterSystemType(new PointerType(TypeIds.PBoolean, TypeIds.BooleanType), "PBoolean");
            RegisterSystemType(new PointerType(TypeIds.PLongBool, TypeIds.LongBoolType), "PLongBool");
            RegisterSystemType(new PointerType(TypeIds.PWordBool, TypeIds.WordBoolType), "PWordBool");
            RegisterSystemType(new PointerType(TypeIds.PPointer, TypeIds.GenericPointer), "PPointer");
            RegisterSystemType(new PointerType(TypeIds.PCurrency, TypeIds.Currency), "PCurrency");
        }

        /// <summary>
        ///     register real types
        /// </summary>
        private void RegisterRealTypes() {
            RegisterSystemType(new RealType(TypeIds.Real48Type), "Real48");
            RegisterSystemType(new RealType(TypeIds.SingleType), "Single");
            RegisterSystemType(new RealType(TypeIds.Double), "Double");
            RegisterSystemType(new RealType(TypeIds.Extended), "Extended");
            RegisterSystemType(new RealType(TypeIds.Comp), "Comp");
            RegisterSystemType(new RealType(TypeIds.Currency), "Currency");
        }

        /// <summary>
        ///     register native integer types
        /// </summary>
        /// <param name="intSize">integer size</param>
        private void RegisterNativeIntTypes(NativeIntSize intSize) {
            RegisterSystemType(new TypeAlias(TypeIds.FixedInt, TypeIds.IntegerType), "FixedInt");
            RegisterSystemType(new TypeAlias(TypeIds.FixedUInt, TypeIds.CardinalType), "FixedUInt");

            if (intSize == NativeIntSize.Windows64bit) {
                RegisterSystemType(new TypeAlias(TypeIds.NativeInt, TypeIds.Int64Type), "NativeInt");
                RegisterSystemType(new TypeAlias(TypeIds.NativeUInt, TypeIds.Uint64Type), "NativeUInt");
                RegisterSystemType(new TypeAlias(TypeIds.LongInt, TypeIds.IntegerType), "LongInt");
                RegisterSystemType(new TypeAlias(TypeIds.LongWord, TypeIds.CardinalType), "LongWord");
            }
            else if (intSize == NativeIntSize.All64bit) {
                RegisterSystemType(new TypeAlias(TypeIds.NativeInt, TypeIds.Int64Type), "NativeInt");
                RegisterSystemType(new TypeAlias(TypeIds.NativeUInt, TypeIds.Uint64Type), "NativeUInt");
                RegisterSystemType(new TypeAlias(TypeIds.LongInt, TypeIds.Int64Type), "LongInt");
                RegisterSystemType(new TypeAlias(TypeIds.LongWord, TypeIds.Uint64Type), "LongWord");
            }
            else {
                RegisterSystemType(new TypeAlias(TypeIds.NativeInt, TypeIds.IntegerType), "NativeInt");
                RegisterSystemType(new TypeAlias(TypeIds.NativeUInt, TypeIds.CardinalType), "NativeUInt");
                RegisterSystemType(new TypeAlias(TypeIds.LongInt, TypeIds.IntegerType), "LongInt");
                RegisterSystemType(new TypeAlias(TypeIds.LongWord, TypeIds.CardinalType), "LongWord");
            }
        }

        /// <summary>
        ///     register string types
        /// </summary>
        private void RegisterStringTypes() {
            RegisterSystemType(new AnsiCharType(TypeIds.AnsiCharType), "AnsiChar");
            RegisterSystemType(new WideCharType(TypeIds.WideCharType), "WideChar");
            RegisterSystemType(new AnsiStringType(TypeIds.AnsiStringType), "AnsiString");
            RegisterSystemType(new AnsiStringType(TypeIds.RawByteString), "RawByteString");
            RegisterSystemType(new ShortStringType(TypeIds.ShortStringType), "ShortString");
            RegisterSystemType(new UnicodeStringType(TypeIds.UnicodeStringType), "UnicodeString");
            RegisterSystemType(new WideStringType(TypeIds.WideStringType), "WideString");
        }

        /// <summary>
        ///     register boolean types
        /// </summary>
        private void RegisterBoolTypes() {
            RegisterSystemType(new BooleanType(TypeIds.BooleanType, 1), "Boolean");
            RegisterSystemType(new BooleanType(TypeIds.ByteBoolType, 8), "ByteBool");
            RegisterSystemType(new BooleanType(TypeIds.WordBoolType, 16), "WordBool");
            RegisterSystemType(new BooleanType(TypeIds.LongBoolType, 32), "LongBool");
        }

        /// <summary>
        ///     register integer types
        /// </summary>
        private void RegisterIntTypes() {
            RegisterSystemType(new IntegralType(TypeIds.ByteType, false, 8), "Byte");
            RegisterSystemType(new IntegralType(TypeIds.ShortInt, true, 8), "ShortInt");
            RegisterSystemType(new IntegralType(TypeIds.WordType, false, 16), "Word");
            RegisterSystemType(new IntegralType(TypeIds.SmallInt, true, 16), "SmallInt");
            RegisterSystemType(new IntegralType(TypeIds.CardinalType, false, 32), "Cardinal");
            RegisterSystemType(new IntegralType(TypeIds.IntegerType, true, 32), "Integer");
            RegisterSystemType(new Integral64BitType(TypeIds.Uint64Type, false), "UInt64");
            RegisterSystemType(new Integral64BitType(TypeIds.Int64Type, true), "Int64");
        }

        /// <summary>
        ///     gets an registered operator
        /// </summary>
        /// <param name="operatorKind">operator kind</param>
        /// <returns></returns>
        public IOperator GetOperator(int operatorKind) {
            operators.TryGetValue(operatorKind, out var result);
            return result;
        }

        /// <summary>
        ///     get a type definition or the error fallback
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ITypeDefinition GetTypeByIdOrUndefinedType(int typeId) {
            if (!types.TryGetValue(typeId, out var result))
                result = types[TypeIds.ErrorType];

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
            var def = new StructuredTypeDeclaration(TypeIds.TObject, StructuredTypeKind.Class);
            var meta = new MetaStructuredTypeDeclaration(TypeIds.TClass, TypeIds.TObject);
            RegisterSystemType(def, "TObject");
            RegisterSystemType(meta, "TClass");
            def.MetaType = meta;
            def.AddOrExtendMethod("Create", ProcedureKind.Constructor).AddParameterGroup();
            def.AddOrExtendMethod("Free", ProcedureKind.Procedure).AddParameterGroup();
            def.AddOrExtendMethod("DisposeOf", ProcedureKind.Procedure).AddParameterGroup();
            def.AddOrExtendMethod("CleanupInstance", ProcedureKind.Procedure).AddParameterGroup();
            def.AddOrExtendMethod("ClassType", ProcedureKind.Function).AddParameterGroup(GetTypeByIdOrUndefinedType(TypeIds.TClass));
            def.AddOrExtendMethod("FieldAddress", ProcedureKind.Function).AddParameterGroup(//
                "Name",
                GetTypeByIdOrUndefinedType(TypeIds.ShortStringType), //
                GetTypeByIdOrUndefinedType(TypeIds.GenericPointer))[0].ConstantParam = true;
        }

    }
}
