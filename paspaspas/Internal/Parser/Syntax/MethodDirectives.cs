﻿using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     method directrives
    /// </summary>
    public class MethodDirectives : SyntaxPartBase {


        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public MethodDirectives(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     format directives
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}