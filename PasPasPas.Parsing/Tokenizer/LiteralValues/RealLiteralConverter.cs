using System;
using PasPasPas.Global.Runtime;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Infrastructure.Utils;
using SharpFloat.FloatingPoint;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {


    /// <summary>
    ///     helper class to convert real literals
    /// </summary>
    public class RealLiteralConverter : IEnvironmentItem, IRealConverter, ILookupFunction<string, IValue> {

        private readonly IRuntimeValueFactory constantsValues;
        private LookupTable<string, IValue> data;

        /// <summary>
        ///     table entries
        /// </summary>
        public LookupTable<string, IValue> Table
            => data;

        LookupTable ILookupFunction.Table
            => data;

        /// <summary>
        ///     number of parsed numbers
        /// </summary>
        public int Count
            => Table.Count;

        /// <summary>
        ///     caption
        /// </summary>
        public string Caption
            => "RealParser";

        /// <summary>
        ///     create a new real literal converter
        /// </summary>
        public RealLiteralConverter(IRuntimeValueFactory constValues) {
            data = new LookupTable<string, IValue>(ConvertLiterals);
            constantsValues = constValues;
        }

        /// <summary>
        ///     convert parsed literals to a real literal
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IValue ConvertLiterals(string value) {
            if (ExtF80.TryParse(value, out var realValue))
                return constantsValues.RealNumbers.ToExtendedValue(realValue);
            return constantsValues.RealNumbers.Invalid;
        }

        /// <summary>
        ///     convert a parsed real number to a real literal
        /// </summary>
        /// <returns></returns>
        public IValue Convert(string value)
            => data.GetValue(value);
    }
};