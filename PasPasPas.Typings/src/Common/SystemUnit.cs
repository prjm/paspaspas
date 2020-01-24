using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Routines;
using PasPasPas.Typings.Simple;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     system wide definitions
    /// </summary>
    public class SystemUnit : ISystemUnit {

        /// <summary>
        ///     create a new system unit
        /// </summary>
        public SystemUnit() {
            Symbols = new Dictionary<string, ITypeSymbol>();
            ErrorType = new ErrorType();

            // integral types
            ByteType = Register(new IntegralType(false, 8));
            ShortIntType = Register(new IntegralType(true, 8));
            WordType = Register(new IntegralType(false, 16));
            SmallIntType = Register(new IntegralType(true, 16));
            CardinalType = Register(new IntegralType(false, 32));
            IntegerType = Register(new IntegralType(true, 32));

            // intrinsic functions
            Register(new Abs());
            Register(new Chr());
            Register(new Concat());
            Register(new HiOrLo(HiLoMode.Hi));
            Register(new HighOrLow(HighOrLowMode.High));
            Register(new Length());
            Register(new HiOrLo(HiLoMode.Lo));
            Register(new HighOrLow(HighOrLowMode.Low));
            Register(new MulDivInt64());
            Register(new Odd());
            Register(new Ord());
            Register(new Pi());
            Register(new PredOrSucc(PredSuccMode.Pred));
            Register(new PtrRoutine());
            Register(new Round());
            Register(new PredOrSucc(PredSuccMode.Succ));
            Register(new SizeOf());
            Register(new Sqr());
            Register(new Swap());
            Register(new Trunc());

            // dynamic procedures
            systemUnit.AddGlobal(new WriteLn());
        }

        /// <summary>
        ///     register a type definition
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="definition"></param>
        /// <returns></returns>
        private T Register<T>(T definition) where T : ITypeSymbol {
            Symbols.Add(definition.LongName, definition);
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
        ///     type name
        /// </summary>
        public string Name
            => KnownNames.System;

        /// <summary>
        ///     registered symbols
        /// </summary>
        private Dictionary<string, ITypeSymbol> Symbols { get; }

        /// <summary>
        ///     error type
        /// </summary>
        public ErrorType ErrorType { get; }

        public CommonTypeKind TypeKind => throw new System.NotImplementedException();

        public ITypeRegistry TypeRegistry => throw new System.NotImplementedException();

        public uint TypeSizeInBytes => throw new System.NotImplementedException();

        public string ShortName => throw new System.NotImplementedException();

        public string LongName => throw new System.NotImplementedException();

        public int TypeId => throw new System.NotImplementedException();

        public ITypeDefinition TypeDefinition => throw new System.NotImplementedException();

        IEnumerable<ITypeSymbol> IUnitType.Symbols => throw new System.NotImplementedException();

        public bool CanBeAssignedFrom(ITypeDefinition otherType) => throw new System.NotImplementedException();

        /// <summary>
        ///     get a known type definition
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public ITypeSymbol GetSymbol(string name) {
            if (!Symbols.TryGetValue(name, out var result))
                result = ErrorType;

            return result;
        }
    }
}