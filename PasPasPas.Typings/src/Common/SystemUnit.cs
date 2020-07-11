using System.Collections.Immutable;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Hidden;
using PasPasPas.Typings.Routines;
using PasPasPas.Typings.Routines.Runtime;
using PasPasPas.Typings.Simple;
using PasPasPas.Typings.Structured;
using Names = PasPasPas.Globals.Types.KnownNames;

namespace PasPasPas.Typings.Common {

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

            RegisterHiddenTypes();

            RegisterIntegralTypes();
            RegisterRealTypes();
            RegisterBooleanTypes();
            RegisterStringTypes();
            RegisterNativeIntTypes(intSize);
            RegisterPointerTypes();
            RegisterAliasTypes();
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
            Real48Type = RegisterType(new RealType(this, RealTypeKind.Real48));
            SingleType = RegisterType(new RealType(this, RealTypeKind.Single));
            DoubleType = RegisterType(new RealType(this, RealTypeKind.Double));
            ExtendedType = RegisterType(new RealType(this, RealTypeKind.Extended));
            CompType = RegisterType(new RealType(this, RealTypeKind.Comp));
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

            UInt8Type = RegisterAlias(ByteType, Names.UInt8);
            Int8Type = RegisterAlias(ShortIntType, Names.Int8);
            UInt16Type = RegisterAlias(WordType, Names.UInt16);
            Int16Type = RegisterAlias(SmallIntType, Names.Int16);
            UInt32Type = RegisterAlias(CardinalType, Names.UInt32);
            Int32Type = RegisterAlias(IntegerType, Names.Int32);
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
            AnsiStringType = RegisterType(new AnsiStringType(this, Simple.AnsiStringType.DefaultSystemCodePage), Names.AnsiString, Simple.AnsiStringType.GetMangledName(Simple.AnsiStringType.DefaultSystemCodePage));
            RawByteStringType = RegisterType(new AnsiStringType(this, Simple.AnsiStringType.NoCodePage), Names.RawByteString, Simple.AnsiStringType.GetMangledName(Simple.AnsiStringType.NoCodePage));
            ShortStringType = RegisterType(new ShortStringType(this, 0xff));
            UnicodeStringType = RegisterType(new UnicodeStringType(this));
            WideStringType = RegisterType(new WideStringType(this));
        }

        private void RegisterPointerTypes() {
            var pv = new PointerType(this, Names.Pointer, default);
            GenericPointerType = RegisterType(pv, pv.Name, pv.MangledName);
            PByteType = RegisterPointerType(ByteType, Names.PByte);
            PShortIntType = RegisterPointerType(ShortIntType, Names.PShortInt);
            PWordType = RegisterPointerType(WordType, Names.PWord);
            PSmallIntType = RegisterPointerType(SmallIntType, Names.PSmallInt);
            PCardinalType = RegisterPointerType(CardinalType, Names.PCardinal);
            PLongwordType = RegisterPointerType(LongWordType, Names.PLongword);
            PFixedUIntType = RegisterPointerType(FixedUIntType, Names.PFixedUint);
            PIntegerType = RegisterPointerType(IntegerType, Names.PInteger);
            PLongIntType = RegisterPointerType(LongIntType, Names.PLongInt);
            PFixedIntType = RegisterPointerType(FixedIntType, Names.PFixedInt);
            PUInt64Type = RegisterPointerType(UInt64Type, Names.PUInt64);
            PInt64Type = RegisterPointerType(Int64Type, Names.PInt64);
            PNativeUIntType = RegisterPointerType(NativeUIntType, Names.PNativeUInt);
            PNativeIntType = RegisterPointerType(NativeIntType, Names.PNativeInt);
            PSingleType = RegisterPointerType(SingleType, Names.PSingle);
            PDoubleType = RegisterPointerType(DoubleType, Names.PDouble);
            PExtendedType = RegisterPointerType(ExtendedType, Names.PExtended);
            PAnsiCharType = RegisterPointerType(AnsiCharType, Names.PAnsiChar);
            PWideCharType = RegisterPointerType(WideCharType, Names.PWideChar);
            PAnsiStringType = RegisterPointerType(AnsiStringType, Names.PAnsiString);
            PRawByteStringType = RegisterPointerType(RawByteStringType, Names.PRawByteString);
            PUnicodeStringType = RegisterPointerType(UnicodeStringType, Names.PUnicodeString);
            PShortStringType = RegisterPointerType(ShortStringType, Names.PShortString);
            PWideStringType = RegisterPointerType(WideStringType, Names.PWideString);
            PBooleanType = RegisterPointerType(BooleanType, Names.PBoolean);
            PByteBoolType = RegisterPointerType(ByteBoolType, Names.PByteBool);
            PLongBoolType = RegisterPointerType(LongBoolType, Names.PLongBool);
            PWordBoolType = RegisterPointerType(WordBoolType, Names.PWordBool);
            var ppv = new PointerType(this, Names.PPointer, new NamedType(GenericPointerType, Names.PPointer, Names.PV));
            PPointerType = RegisterType(ppv, Names.PPointer, Names.PPV);
            PCurrencyType = RegisterPointerType(CurrencyType, Names.PCurrency);
        }

        /// <summary>
        ///     register type alias
        /// </summary>
        private void RegisterAliasTypes() {
            CharType = RegisterAlias(WideCharType, Names.Char);
            Ucs2CharType = RegisterAlias(WideCharType, Names.Ucs2Char);
            Ucs4CharType = RegisterAlias(CardinalType, Names.Ucs4Char);
            StringType = RegisterAlias(UnicodeStringType, Names.String);
            RealType = RegisterAlias(DoubleType, Names.Real);
            PCharType = RegisterAlias(PWideCharType, Names.PChar);
            PStringType = RegisterAlias(PUnicodeStringType, Names.PString);
        }

        private void RegisterHiddenTypes() {
            UnspecifiedType = new UnspecifiedType(this);
            NoType = new VoidType(this);
            NilType = new NilType(this);
            UnconstrainedGenericTypeParameter = new GenericTypeParameter(this, string.Empty, ImmutableArray<ITypeDefinition>.Empty);
            RoutineGroupType = new RoutineGroupType(this);
        }

        private void RegisterOtherTypes() {
            RegisterType(new GenericArrayType(Names.TArray, this, UnconstrainedGenericTypeParameter, IntegerType));
            UnspecifiedFileType = new FileType(this, GenericPointerType);
            RegisterType(new NamedType(UnspecifiedFileType, Names.File, FileType.GetMangledName(this, Names.File)));
            GenericClassConstraint = new GenericConstraintType(this, GenericConstraintKind.Class);
            GenericRecordConstraint = new GenericConstraintType(this, GenericConstraintKind.Record);
            GenericConstructorConstraint = new GenericConstraintType(this, GenericConstraintKind.Constructor);
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
        private T RegisterType<T>(T definition) where T : INamedTypeSymbol {
            Register(definition);
            return definition;
        }

        private PointerType RegisterPointerType(IMangledNameTypeSymbol definition, string name) {
            var pointerType = new PointerType(this, name, definition);
            Register(new NamedType(pointerType, name, pointerType.MangledName));
            return pointerType;
        }

        /// <summary>
        ///     register a type definition
        /// </summary>
        /// <typeparam name="T">type of type definition</typeparam>
        /// <param name="definition">type definition</param>
        /// <param name="mangledName">mangled type name</param>
        /// <param name="name">type name</param>
        /// <returns></returns>
        private T RegisterType<T>(T definition, string name, string mangledName) where T : ITypeDefinition {
            Register(new NamedType(definition, name, mangledName));
            return definition;
        }


        /// <summary>
        ///     register a type alias
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="aliasName"></param>
        private IAliasedType RegisterAlias(ITypeDefinition baseType, string aliasName) {
            var alias = new TypeAlias(this, baseType, aliasName, false);
            Register(alias);
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
            if (definition is IntrinsicRoutine ir)
                ir.TypeRegistry = this.TypeRegistry;

            return definition;
        }


        /// <summary>
        ///     byte type
        /// </summary>
        public IIntegralType ByteType { get; private set; }
            = default!;

        /// <summary>
        ///     short int type
        /// </summary>
        public IIntegralType ShortIntType { get; private set; }
            = default!;

        /// <summary>
        ///     word type
        /// </summary>
        public IIntegralType WordType { get; private set; }
            = default!;

        /// <summary>
        ///     small int type
        /// </summary>
        public IIntegralType SmallIntType { get; private set; }
            = default!;

        /// <summary>
        ///     cardinal type
        /// </summary>
        public IIntegralType CardinalType { get; private set; }
            = default!;

        /// <summary>
        ///     integer type
        /// </summary>
        public IIntegralType IntegerType { get; private set; }
            = default!;

        /// <summary>
        ///     int64 type
        /// </summary>
        public IIntegralType Int64Type { get; private set; }
            = default!;

        /// <summary>
        ///     unsigned int64 type
        /// </summary>
        public IIntegralType UInt64Type { get; private set; }
            = default!;

        public IAliasedType UInt8Type { get; private set; }
                    = default!;
        public IAliasedType Int8Type { get; private set; }
                    = default!;

        public IAliasedType UInt16Type { get; private set; }
                    = default!;

        public IAliasedType Int16Type { get; private set; }
                    = default!;

        public IAliasedType UInt32Type { get; private set; }
                    = default!;

        public IAliasedType Int32Type { get; private set; }
                    = default!;

        /// <summary>
        ///     error type
        /// </summary>
        public IErrorType ErrorType { get; }

        /// <summary>
        ///     boolean type
        /// </summary>
        public IBooleanType BooleanType { get; private set; }
            = default!;

        /// <summary>
        ///     byte boolean type
        /// </summary>
        public IBooleanType ByteBoolType { get; private set; }
            = default!;

        /// <summary>
        ///     word boolean type
        /// </summary>
        public IBooleanType WordBoolType { get; private set; }
            = default!;

        /// <summary>
        ///     long boolean type
        /// </summary>
        public IBooleanType LongBoolType { get; private set; }
            = default!;

        /// <summary>
        ///     wide char type
        /// </summary>
        public ICharType WideCharType { get; private set; }
            = default!;

        /// <summary>
        ///     ANSI char type
        /// </summary>
        public ICharType AnsiCharType { get; private set; }
            = default!;

        /// <summary>
        ///     Unicode string type
        /// </summary>
        public IStringType UnicodeStringType { get; private set; }
            = default!;

        /// <summary>
        ///     ANSI string type
        /// </summary>
        public IStringType AnsiStringType { get; private set; }
            = default!;

        /// <summary>
        ///     raw byte string type
        /// </summary>
        public IStringType RawByteStringType { get; private set; }
            = default!;

        /// <summary>
        ///     short string type
        /// </summary>
        public IStringType ShortStringType { get; private set; }
            = default!;

        /// <summary>
        ///     extended type definition
        /// </summary>
        public IRealType ExtendedType { get; private set; }
            = default!;

        /// <summary>
        ///     comp type
        /// </summary>
        public IRealType CompType { get; private set; }
            = default!;

        /// <summary>
        ///     native integer type
        /// </summary>
        public IAliasedType NativeIntType { get; private set; }
            = default!;

        /// <summary>
        ///     native unsigned integer type
        /// </summary>
        public IAliasedType NativeUIntType { get; private set; }
            = default!;

        /// <summary>
        ///     native long int type
        /// </summary>
        public IAliasedType LongIntType { get; private set; }
            = default!;

        /// <summary>
        ///     native long word type
        /// </summary>
        public IAliasedType LongWordType { get; private set; }
            = default!;

        /// <summary>
        ///     generic pointer type
        /// </summary>
        public IPointerType GenericPointerType { get; private set; }
            = default!;

        public IPointerType PByteType { get; private set; }
            = default!;

        public IPointerType PShortIntType { get; private set; }
            = default!;

        public IPointerType PWordType { get; private set; }
            = default!;

        public IPointerType PSmallIntType { get; private set; }
            = default!;

        public IPointerType PCardinalType { get; private set; }
            = default!;

        public IPointerType PLongwordType { get; private set; }
            = default!;

        public IPointerType PFixedUIntType { get; private set; }
            = default!;

        public IPointerType PIntegerType { get; private set; }
            = default!;

        public IPointerType PLongIntType { get; private set; }
            = default!;

        /// <summary>
        ///     nil type
        /// </summary>
        public ITypeDefinition NilType { get; private set; }
            = default!;

        /// <summary>
        ///     unconstrained generic type parameter
        /// </summary>
        public IGenericTypeParameter UnconstrainedGenericTypeParameter { get; private set; }
            = default!;

        /// <summary>
        ///     routine group type
        /// </summary>
        public IRoutineGroupType RoutineGroupType { get; private set; }
            = default!;

        /// <summary>
        ///     wide string type
        /// </summary>
        public IStringType WideStringType { get; private set; }
            = default!;

        /// <summary>
        ///     fixed integer type
        /// </summary>
        public IAliasedType FixedIntType { get; private set; }
            = default!;

        /// <summary>
        ///     fixed unsigned int type
        /// </summary>
        public IAliasedType FixedUIntType { get; private set; }
            = default!;

        public IRealType Real48Type { get; private set; }
            = default!;

        /// <summary>
        ///     single type
        /// </summary>
        public IRealType SingleType { get; private set; }
            = default!;

        /// <summary>
        ///     double type
        /// </summary>
        public IRealType DoubleType { get; private set; }
            = default!;

        public IAliasedType PCharType { get; private set; }
            = default!;

        public IAliasedType PStringType { get; private set; }
            = default!;

        /// <summary>
        ///     currency type
        /// </summary>
        public IRealType CurrencyType { get; private set; }
            = default!;

        public IPointerType PFixedIntType { get; private set; }
            = default!;

        public IPointerType PUInt64Type { get; private set; }
            = default!;

        public IPointerType PInt64Type { get; private set; }
            = default!;

        public IPointerType PNativeUIntType { get; private set; }
            = default!;

        public IPointerType PNativeIntType { get; private set; }
            = default!;

        public IPointerType PSingleType { get; private set; }
            = default!;

        public IPointerType PDoubleType { get; private set; }
            = default!;

        public IPointerType PExtendedType { get; private set; }
            = default!;

        public IPointerType PAnsiCharType { get; private set; }
            = default!;

        /// <summary>
        ///     pointer to a wide char type
        /// </summary>
        public IPointerType PWideCharType { get; private set; }
            = default!;

        public IPointerType PAnsiStringType { get; private set; }
            = default!;

        public IPointerType PRawByteStringType { get; private set; }
            = default!;

        /// <summary>
        ///     pointer to a Unicode string type
        /// </summary>
        public IPointerType PUnicodeStringType { get; private set; }
            = default!;

        public IPointerType PShortStringType { get; private set; }
            = default!;

        public IPointerType PWideStringType { get; private set; }
            = default!;

        public IPointerType PBooleanType { get; private set; }
            = default!;

        public IPointerType PByteBoolType { get; private set; }
            = default!;

        public IPointerType PLongBoolType { get; private set; }
            = default!;

        public IPointerType PWordBoolType { get; private set; }
            = default!;

        public IPointerType PPointerType { get; private set; }
            = default!;

        public IPointerType PCurrencyType { get; private set; }
            = default!;

        /// <summary>
        ///     unspecified type
        /// </summary>
        public IUnspecifiedType UnspecifiedType { get; private set; }
            = default!;

        /// <summary>
        ///     no type at all
        /// </summary>
        public INoType NoType { get; private set; }
            = default!;

        /// <summary>
        ///     ucs4 char type
        /// </summary>
        public IAliasedType Ucs4CharType { get; private set; }
            = default!;

        /// <summary>
        ///     common string type
        /// </summary>
        public IAliasedType StringType { get; private set; }
            = default!;

        public IAliasedType RealType { get; private set; }
            = default!;

        /// <summary>
        ///     TObject type
        /// </summary>
        public IStructuredType TObjectType { get; private set; }
            = default!;

        /// <summary>
        ///     unspecified file type
        /// </summary>
        public IFileType UnspecifiedFileType { get; private set; }
            = default!;

        /// <summary>
        ///     generic class constraint
        /// </summary>
        public ITypeDefinition GenericClassConstraint { get; private set; }
            = default!;

        /// <summary>
        ///     generic record constraint
        /// </summary>
        public ITypeDefinition GenericRecordConstraint { get; private set; }
            = default!;

        /// <summary>
        ///     generic constructor constraint
        /// </summary>
        public ITypeDefinition GenericConstructorConstraint { get; private set; }
            = default!;

        /// <summary>
        ///     format expression helper routine
        /// </summary>
        public IRoutineGroup FormatExpression { get; private set; }
            = default!;

        /// <summary>
        ///     char type
        /// </summary>
        public IAliasedType CharType { get; private set; }
            = default!;

        /// <summary>
        ///     ucs2 char type
        /// </summary>
        public IAliasedType Ucs2CharType { get; private set; }
            = default!;

        IRoutineGroup ISystemUnit.FormatExpression => throw new System.NotImplementedException();
    }
}