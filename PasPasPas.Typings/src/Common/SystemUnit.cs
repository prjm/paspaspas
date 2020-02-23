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
    public class SystemUnit : UnitType, ISystemUnit {

        /// <summary>
        ///     create a new system unit
        /// </summary>
        /// <param name="types"></param>
        public SystemUnit(ITypeRegistry types) : base(KnownNames.System, types) {
            ErrorType = RegisterType(new ErrorType(this));

            RegisterIntegralTypes();
            RegisterRealTypes();

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

            // dynamic procedures
            RegisterRoutine(new WriteLn());
        }

        /// <summary>
        ///     register real types
        /// </summary>
        private void RegisterRealTypes() {
            RegisterType(new RealType(this, RealTypeKind.Real48));
            RegisterType(new RealType(this, RealTypeKind.Single));
            RegisterType(new RealType(this, RealTypeKind.Double));
            ExtendedType = RegisterType(new RealType(this, RealTypeKind.Extended));
            RegisterType(new RealType(this, RealTypeKind.Comp));
            RegisterType(new RealType(this, RealTypeKind.Currency));
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
            RegisterType(new BooleanType(this, BooleanTypeKind.ByteBool));
            RegisterType(new BooleanType(this, BooleanTypeKind.WordBool));
            RegisterType(new BooleanType(this, BooleanTypeKind.LongBool));
        }

        /// <summary>
        ///     register a type definition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition"></param>
        /// <returns></returns>
        private T RegisterType<T>(T definition) where T : ITypeDefinition {
            Register(definition);
            return definition;
        }

        /// <summary>
        ///     register a type alias
        /// </summary>
        /// <param name="baseType"></param>
        /// <param name="aliasName"></param>
        private void RegisterAlias(ITypeDefinition baseType, string aliasName) {
            var alias = new TypeAlias(this, baseType, aliasName, false);
            Register(alias);
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

        public ICharType WideCharType => throw new System.NotImplementedException();

        public ICharType AnsiCharType => throw new System.NotImplementedException();

        public IStringType UnicodeStringType => throw new System.NotImplementedException();

        public IStringType AnsiStringType => throw new System.NotImplementedException();

        public IStringType ShortStringType => throw new System.NotImplementedException();

        /// <summary>
        ///     extended type definition
        /// </summary>
        public IRealType ExtendedType { get; private set; }

        public IAliasedType NativeIntType => throw new System.NotImplementedException();

        public ITypeDefinition GenericPointerType => throw new System.NotImplementedException();

        public ITypeDefinition NilType => throw new System.NotImplementedException();

        public IStringType WideStringType => throw new System.NotImplementedException();
    }
}