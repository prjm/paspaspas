﻿using PasPasPas.Api;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.SyntaxTree.ObjectPascal {

    /// <summary>
    ///     property accessor
    /// </summary>
    public class ClassPropertyReadWrite : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassPropertyReadWrite(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        ///     accessor kind
        /// </summary>
        public int Kind { get; internal set; }

        /// <summary>
        ///     member name
        /// </summary>
        public NamespaceName Member { get; internal set; }

        /// <summary>
        ///     format accessor
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (Kind) {

                case TokenKind.Read:
                    result.Keyword("read");
                    break;

                case TokenKind.Write:
                    result.Keyword("write");
                    break;

                case TokenKind.Add:
                    result.Keyword("add");
                    break;

                case TokenKind.Remove:
                    result.Keyword("remove");
                    break;

            }

            result.Space();
            Member.ToFormatter(result);
        }
    }
}