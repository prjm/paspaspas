﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Environment;

namespace PasPasPas.Parsing.Tokenizer.LiteralValues {

    /// <summary>
    ///     boolean literal provider
    /// </summary>
    public class BooleanLiteralProvider : IEnvironmentItem, IBooleanLiteralProvider {

        public object TrueValue => true;

        public object FalseValue => false;

        /// <summary>
        ///     item count (not applicable)
        /// </summary>
        public int Count
            => -1;

        /// <summary>
        ///     caption
        /// </summary>
        public string Caption
            => "BooleanLiteralProvider";
    }
}