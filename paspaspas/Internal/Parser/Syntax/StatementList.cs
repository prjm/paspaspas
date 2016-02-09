﻿using System;
using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     statement list
    /// </summary>
    public class StatementList : ComposedPart<Statement> {

        /// <summary>
        ///     create a new syntax element
        /// </summary>
        /// <param name="parser"></param>
        public StatementList(IParserInformationProvider parser) : base(parser) { }

        /// <summary>
        ///     format statements
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}