﻿using PasPasPas.Api;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     section for constants
    /// </summary>
    public class ConstSection : ComposedPart<ConstDeclaration> {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ConstSection(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     kind of constant
        /// </summary>
        public int Kind { get; internal set; }
                = PascalToken.Undefined;

        /// <summary>
        ///     format const section
        /// </summary>
        /// <param name="result">formatter</param>
        public override void ToFormatter(PascalFormatter result) {
            if (Count < 1)
                return;

            if (Kind == PascalToken.Const)
                result.Keyword("const");
            else if (Kind == PascalToken.Resourcestring)
                result.Keyword("resourcestring");

            result.StartIndent();
            result.NewLine();

            FlattenToPascal(result, x => { });

            result.EndIndent();
        }
    }
}
