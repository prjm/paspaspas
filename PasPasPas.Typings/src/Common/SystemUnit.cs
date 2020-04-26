using System.Collections.Immutable;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Hidden;
using PasPasPas.Typings.Routines;
using PasPasPas.Typings.Routines.Runtime;
using PasPasPas.Typings.Simple;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Common {

    using Names = KnownNames;

    /// <summary>
    ///     system wide definitions
    /// </summary>
    internal class SystemUnit : UnitType, ISystemUnit {

        /// <summary>
        ///     create a new system unit
        /// </summary>
        /// <param name="types">global type registry</param>
        /// <param name="intSize">integer size</param>
        public SystemUnit(ITypeRegistry types, NativeIntSize intSize) : base(KnownNames.System, types) {
            ErrorType = RegisterType(new ErrorType(this));

            RegisterIntegralTypes();
            RegisterRealTypes();
            RegisterBooleanTypes();
            RegisterStringTypes();
            RegisterPointerTypes();
            RegisterNativeIntTypes(intSize);
            RegisterAliasTypes();
            RegisterHiddenTypes();
            RegisterOtherTypes();

            // intrinsic functions
            RegisterRoutine(new Abs());
            RegisterRoutine(new Chr());
            RegisterRoutine(new Concat());
            RegisterRoutine(new HiOrLo(HiLoMode.Hi));
            RegisterRoutine(new HighOrLow(HighOrLowMode.High));
            RegisterRoutine(new Length());
            RegisterRoutine(new HiOrLo(HiLoMode.Lo));
            RegisterRoutine(new HighOrLow(HighOrLowMode.Low));
            RegisterRoutine(new MulDivInt64());
            RegisterRoutine(new Odd());
            RegisterRoutine(new Ord());
            RegisterRoutine(new Pi());
            RegisterRoutine(new PredOrSucc(PredSuccMode.Pred));
            RegisterRoutine(new PtrRoutine());
            RegisterRoutine(new Round());
            RegisterRoutine(new PredOrSucc(PredSuccMode.Succ));
            RegisterRoutine(new SizeOf());
            RegisterRoutine(new Sqr());
            RegisterRoutine(new Swap());
            RegisterRoutine(new Trunc());
            FormatExpression = RegisterRoutine(new FormatExpression());

            // dynamic procedures
            RegisterRoutine(new WriteLn());
        }

        /// <summary>
        ///     register real types
        /// </summary>
        private void RegisterRealTypes() {
            RegisterType(new RealType(this, RealTypeKind.Real48));
            SingleType = RegisterType(new RealType(this, RealTypeKind.Single));
            DoubleType = RegisterType(new RealType(this, RealTypeKind.Double));
            ExtendedType = RegisterType(new RealType(this, RealTypeKind.Extended));
            RegisterType(new RealType(this, RealTypeKind.Comp));
            CurrencyType = RegisterType(new RealType(this, RealTypeKind.Currency));
        }

        private void RegisterIntegralTypes() {
            ByteType = RegisterType(new IntegralType(this, IntegralTypeKind.Byte));
            ShortIntType = RegisterType(new IntegralType(this, IntegralTypeKind.ShortInt));
            WordType = RegisterType(new IntegralType(this, IntegralTypeKind.Word));
            SmallIntType = RegisterType(new IntegralType(this, IntegralTypeKind.SmallInt));
            CardinalType = RegisterType(new IntegralType(this, IntegralTypeKind.Cardinal));
            IntegerType = RegisterType(new IntegralType(this, IntegralTypeKind.Integer));
            Int64Type = RegisterType(new IntegralType(this, IntegralTypeKind.Int64));
            UInt64Type = RegisterType(new IntegralType(this, IntegralTypeKind.UInt64));

            RegisterAlias(ByteType, Names.UInt8);
            RegisterAlias(ShortIntType, Names.Int8);
            RegisterAlias(WordType, Names.UInt16);
            RegisterAlias(SmallIntType, Names.Int16);
            RegisterAlias(CardinalType, Names.UInt32);
            RegisterAlias(IntegerType, Names.Int32);
        }

        private void RegisterBooleanTypes() {
            BooleanType = RegisterType(new BooleanType(this, BooleanTypeKind.Boolean));
            ByteBoolType = RegisterType(new BooleanType(this, BooleanTypeKind.ByteBool));
            WordBoolType = RegisterType(new BooleanType(this, BooleanTypeKind.WordBool));
            LongBoolType = RegisterType(new BooleanType(this, BooleanTypeKind.LongBool));
        }

        private void RegisterStringTypes() {
            AnsiCharType = RegisterType(new AnsiCharType(this));
            WideCharType = RegisterType(new WideCharType(this));
            AnsiStringType = RegisterType(new AnsiStringType(this, Names.AnsiString, Simple.AnsiStringType.DefaultSystemCodePage));
            RawByteStringType = RegisterType(new AnsiStringType(this, Names.RawByteString, Simple.AnsiStringType.NoCodePage));
            ShortStringType = RegisterType(new ShortStringType(this, 0xff));
            UnicodeStringType = RegisterType(new UnicodeStringType(this));
            WideStringType = RegisterType(new WideStringType(this));
        }

        private void RegisterPointerTypes() {
            GenericPointerType = RegisterType(new PointerType(this, default, Names.Pointer));
            RegisterType(new PointerType(this, ByteType, Names.PByte));
            RegisterType(new PointerType(this, ShortIntType, Names.PShortInt));
            RegisterType(new PointerType(this, WordType, Names.PWord));
            RegisterType(new PointerType(this, SmallIntType, Names.PSmallInt));
            RegisterType(new PointerType(this, CardinalType, Names.PCardinal));
            RegisterType(new PointerType(this, LongWordType, Names.PLongword));
            RegisterType(new PointerType(this, FixedUIntType, Names.PFixedUint));
            RegisterType(new PointerType(this, IntegerType, Names.PInteger));
            RegisterType(new PointerType(this, LongIntType, Names.PLongInt));
            RegisterType(new PointerType(this, FixedIntType, Names.PFixedInt));
            RegisterType(new PointerType(this, UInt64Type, Names.PUInt64));
            RegisterType(new PointerType(this, Int64Type, Names.PInt64));
            RegisterType(new PointerType(this, NativeUIntType, Names.PNativeUInt));
            RegisterType(new PointerType(this, NativeIntType, Names.PNativeInt));
            RegisterType(new PointerType(this, SingleType, Names.PSingle));
            RegisterType(new PointerType(this, DoubleType, Names.PDouble));
            RegisterType(new PointerType(this, ExtendedType, Names.PExtended));
            RegisterType(new PointerType(this, AnsiCharType, Names.PAnsiChar));
            PWideCharType = RegisterType(new PointerType(this, WideCharType, Names.PWideChar));
            RegisterType(new PointerType(this, AnsiStringType, Names.PAnsiString));
            RegisterType(new PointerType(this, RawByteStringType, Names.PRawByteString));
            PUnicodeStringType = RegisterType(new PointerType(this, UnicodeStringType, Names.PUnicodeString));
            RegisterType(new PointerType(this, ShortStringType, Names.PShortString));
            RegisterType(new PointerType(this, WideStringType, Names.PWideString));
            RegisterType(new PointerType(this, BooleanType, Names.PBoolean));
            RegisterType(new PointerType(this, ByteBoolType, Names.PByteBool));
            RegisterType(new PointerType(this, LongBoolType, Names.PLongBool));
            RegisterType(new PointerType(this, WordBoolType, Names.PWordBool));
            RegisterType(new PointerType(this, GenericPointerType, Names.PPointer));
            RegisterType(new PointerType(this, CurrencyType, Names.PCurrency));
        }

        /// <summary>
        ///     register type alias
        /// </summary>
        private void RegisterAliasTypes() {
            RegisterAlias(WideCharType, Names.Char);
            RegisterAlias(WideCharType, Names.Ucs2Char);
            RegisterAlias(CardinalType, Names.Ucs4Char);
            StringType = RegisterAlias(UnicodeStringType, Names.String);
            RegisterAlias(DoubleType, Names.Real);
            RegisterAlias(PWideCharType, Names.PChar);
            RegisterAlias(PUnicodeStringType, Names.PString);
        }

        private void RegisterHiddenTypes() {
            UnspecifiedType = RegisterType(new UnspecifiedType(this));
            NoType = RegisterType(new VoidType(this));
            NilType = RegisterType(new NilType(this));
            UnconstrainedGenericTypeParameter = RegisterType(new GenericTypeParameter(this, string.Empty, ImmutableArray<ITypeDefinition>.Empty));
        }

        private void RegisterOtherTypes() {
            RegisterType(new GenericArrayType(Names.TArray, this, default));
            UnspecifiedFileType = RegisterType(new FileType(this, Names.File, GenericPointerType));
            RegisterType(new GenericConstraintType(this, GenericConstraintKind.Class));
            RegisterType(new GenericConstraintType(this, GenericConstraintKind.Record));
            RegisterType(new GenericConstraintType(this, GenericConstraintKind.Constructor));
        }

        /// <summary>
        ///     register native integer types
        /// </summary>
        /// <param name="intSize">integer size</param>
        private void RegisterNativeIntTypes(NativeIntSize intSize) {
            FixedIntType = RegisterAlias(IntegerType, Names.FixedInt);
            FixedUIntType = RegisterAlias(CardinalType, Names.FixedUInt);

            if (intSize == NativeIntSize.Windows64bit) {
                NativeIntType = RegisterAlias(Int64Type, Names.NativeInt);
                NativeUIntType = RegisterAlias(UInt64Type, Names.NativeUInt);
                LongIntType = RegisterAlias(IntegerType, Names.LongInt);
                LongWordType = RegisterAlias(CardinalType, Names.LongWord);
            }
            else if (intSize == NativeIntSize.All64bit) {
                NativeIntType = RegisterAlias(Int64Type, Names.NativeInt);
                NativeUIntType = RegisterAlias(UInt64Type, Names.NativeUInt);
                LongIntType = RegisterAlias(Int64Type, Names.LongInt);
                LongWordType = RegisterAlias(UInt64Type, Names.LongWord);
            }
            else {
                NativeIntType = RegisterAlias(IntegerType, Names.NativeInt);
                NativeUIntType = RegisterAlias(CardinalType, Names.NativeUInt);
                LongIntType = RegisterAlias(IntegerType, Names.LongInt);
                LongWordType = RegisterAlias(CardinalType, Names.LongWord);
            }
        }

        /// <summary>
        ///     register a type definition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition"></param>
        /// <returns></returns>
        private T RegisterType<T>(T definition) where T : ITypeDefinition {
            Register(new ReferenceToTypeDefinition(definition));
            return definition;
        }

        /// <summary>
        ///     register a type alias
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="aliasName"></param>
        private IAliasedType RegisterAlias(ITypeDefinition baseType, string aliasName) {
            var alias = new TypeAlias(this, baseType, aliasName, false);
            Register(new ReferenceToTypeDefinition(alias));
            return alias;
        }

        /// <summary>
        ///     register a type definition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition"></param>
        /// <returns></returns>
        private T RegisterRoutine<T>(T definition) where T : IRoutineGroup {
            Register(definition);
            return definition;
        }


        /// <summary>
        ///     byte type
        /// </summary>
        public IIntegralType ByteType { get; private set; }

        /// <summary>
        ///     short int type
        /// </summary>
        public IIntegralType ShortIntType { get; private set; }

        /// <summary>
        ///     word type
        /// </summary>
        public IIntegralType WordType { get; private set; }

        /// <summary>
        ///     small int type
        /// </summary>
        public IIntegralType SmallIntType { get; private set; }

        /// <summary>
        ///     cardinal type
        /// </summary>
        public IIntegralType CardinalType { get; private set; }

        /// <summary>
        ///     integer type
        /// </summary>
        public IIntegralType IntegerType { get; private set; }

        /// <summary>
        ///     int64 type
        /// </summary>
        public IIntegralType Int64Type { get; private set; }

        /// <summary>
        ///     unsigned int64 type
        /// </summary>
        public IIntegralType UInt64Type { get; private set; }


        /// <summary>
        ///     error type
        /// </summary>
        public ITypeDefinition ErrorType { get; }

        /// <summary>
        ///     boolean type
        /// </summary>
        public IBooleanType BooleanType { get; private set; }

        /// <summary>
        ///     byte boolean type
        /// </summary>
        public IBooleanType ByteBoolType { get; private set; }

        /// <summary>
        ///     word boolean type
        /// </summary>
        public IBooleanType WordBoolType { get; private set; }

        /// <summary>
        ///     long boolean type
        /// </summary>
        public IBooleanType LongBoolType { get; private set; }

        /// <summary>
        ///     wide char type
        /// </summary>
        public ICharType WideCharType { get; private set; }

        /// <summary>
        ///     ANSI char type
        /// </summary>
        public ICharType AnsiCharType { get; private set; }

        /// <summary>
        ///     Unicode string type
        /// </summary>
        public IStringType UnicodeStringType { get; private set; }

        /// <summary>
        ///     ANSI string type
        /// </summary>
        public IStringType AnsiStringType { get; private set; }

        /// <summary>
        ///     raw byte string type
        /// </summary>
        public IStringType RawByteStringType { get; private set; }

        /// <summary>
        ///     short string type
        /// </summary>
        public IStringType ShortStringType { get; private set; }

        /// <summary>
        ///     extended type definition
        /// </summary>
        public IRealType ExtendedType { get; private set; }

        /// <summary>
        ///     native integer type
        /// </summary>
        public IAliasedType NativeIntType { get; private set; }

        /// <summary>
        ///     native unsigned integer type
        /// </summary>
        public IAliasedType NativeUIntType { get; private set; }

        /// <summary>
        ///     native long int type
        /// </summary>
        public IAliasedType LongIntType { get; private set; }

        /// <summary>
        ///     native long word type
        /// </summary>
        public IAliasedType LongWordType { get; private set; }

        /// <summary>
        ///     generic pointer type
        /// </summary>
        public ITypeDefinition GenericPointerType { get; private set; }

        /// <summary>
        ///     nil type
        /// </summary>
        public ITypeDefinition NilType { get; private set; }

        /// <summary>
        ///     unconstrained generic type parameter
        /// </summary>
        public IGenericTypeParameter UnconstrainedGenericTypeParameter { get; private set; }

        /// <summary>
        ///     wide string type
        /// </summary>
        public IStringType WideStringType { get; private set; }

        /// <summary>
        ///     fixed integer type
        /// </summary>
        public IAliasedType FixedIntType { get; private set; }

        /// <summary>
        ///     fixed unsigned int type
        /// </summary>
        public IAliasedType FixedUIntType { get; private set; }

        /// <summary>
        ///     single type
        /// </summary>
        public IRealType SingleType { get; private set; }

        /// <summary>
        ///     double type
        /// </summary>
        public IRealType DoubleType { get; private set; }

        /// <summary>
        ///     currency type
        /// </summary>
        public IRealType CurrencyType { get; private set; }

        /// <summary>
        ///     pointer to a wide char type
        /// </summary>
        public IPointerType PWideCharType { get; private set; }

        /// <summary>
        ///     pointer to a Unicode string type
        /// </summary>
        public IPointerType PUnicodeStringType { get; private set; }

        /// <summary>
        ///     unspecified type
        /// </summary>
        public IUnspecifiedType UnspecifiedType { get; private set; }

        /// <summary>
        ///     no type at all
        /// </summary>
        public INoType NoType { get; private set; }

        /// <summary>
        ///     common string type
        /// </summary>
        public IAliasedType StringType { get; private set; }

        /// <summary>
        ///     TObject type
        /// </summary>
        public IStructuredType TObjectType { get; private set; }

        /// <summary>
        ///     unspecified file type
        /// </summary>
        public IFileType UnspecifiedFileType { get; private set; }

        /// <summary>
        ///     format expression helper routine
        /// </summary>
        public IRoutineGroup FormatExpression { get; private set; }
    }
}