﻿using System.Collections.Immutable;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     type name / reference to a type
    /// </summary>
    public class TypeName : VariableLengthSyntaxTreeBase<GenericNamespaceName> {

        /// <summary>
        ///     create a new string type name
        /// </summary>
        /// <param name="stringType"></param>
        public TypeName(Terminal stringType) : base(ImmutableArray<GenericNamespaceName>.Empty)
            => StringTypeSymbol = stringType;

        /// <summary>
        ///     generate a new combined generic names
        /// </summary>
        /// <param name="names"></param>
        public TypeName(ImmutableArray<GenericNamespaceName> names) : base(names) {
        }

        /// <summary>
        ///     string type
        /// </summary>
        public int StringType
            => StringTypeSymbol.GetSymbolKind();

        /// <summary>
        ///     string type symbol
        /// </summary>
        public Terminal StringTypeSymbol { get; }

        /// <summary>
        ///     map type name kind
        /// </summary>
        /// <returns></returns>
        public MetaTypeKind MapTypeKind() {
            switch (StringType) {
                case TokenKind.String:
                    return MetaTypeKind.String;

                case TokenKind.AnsiString:
                    return MetaTypeKind.AnsiString;

                case TokenKind.ShortString:
                    return MetaTypeKind.ShortString;

                case TokenKind.WideString:
                    return MetaTypeKind.WideString;

                case TokenKind.UnicodeString:
                    return MetaTypeKind.UnicodeString;
            }

            return MetaTypeKind.NamedType;
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, StringTypeSymbol, visitor);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length =>
            StringTypeSymbol.GetSymbolLength() + ItemLength;

    }
}
