using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Hidden;
using PasPasPas.Typings.Routines;
using PasPasPas.Typings.Routines.Runtime;
using PasPasPas.Typings.Simple;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Common {

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

            // integral types
            ByteType = RegisterType(new IntegralType(this, IntegralTypeKind.Byte));
            ShortIntType = RegisterType(new IntegralType(this, IntegralTypeKind.ShortInt));
            WordType = RegisterType(new IntegralType(this, IntegralTypeKind.Word));
            SmallIntType = RegisterType(new IntegralType(this, IntegralTypeKind.SmallInt));
            CardinalType = RegisterType(new IntegralType(this, IntegralTypeKind.Cardinal));
            IntegerType = RegisterType(new IntegralType(this, IntegralTypeKind.Integer));

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
        public IIntegralType ByteType { get; }

        /// <summary>
        ///     short int type
        /// </summary>
        public IIntegralType ShortIntType { get; }

        /// <summary>
        ///     word type
        /// </summary>
        public IIntegralType WordType { get; }

        /// <summary>
        ///     small int type
        /// </summary>
        public IIntegralType SmallIntType { get; }

        /// <summary>
        ///     cardinal type
        /// </summary>
        public IIntegralType CardinalType { get; }

        /// <summary>
        ///     integer type
        /// </summary>
        public IIntegralType IntegerType { get; }

        /// <summary>
        ///     error type
        /// </summary>
        public ITypeDefinition ErrorType { get; }

    }
}