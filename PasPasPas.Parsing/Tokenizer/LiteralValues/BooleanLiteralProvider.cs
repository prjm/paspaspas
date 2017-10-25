using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     boolean literal provider
    /// </summary>
    public class BooleanLiteralProvider : IBooleanLiteralProvider {

        public object TrueValue => true;

        public object FalseValue => false;
    }
}
