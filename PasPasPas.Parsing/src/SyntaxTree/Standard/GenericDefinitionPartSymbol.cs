﻿#nullable disable
using System.Collections.Immutable;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     generic definition part
    /// </summary>
    public class GenericDefinitionPartSymbol : VariableLengthSyntaxTreeBase<ConstrainedGenericSymbol> {

        /// <summary>
        ///     create a new generic definition part
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="terminal"></param>
        public GenericDefinitionPartSymbol(IdentifierSymbol identifier, Terminal terminal) : base(ImmutableArray<ConstrainedGenericSymbol>.Empty) {
            Identifier = identifier;
            CommaOrSemicolon = terminal;
        }

        /// <summary>
        ///     create a new generic definition part
        /// </summary>
        /// <param name="genericDefinitionPart"></param>
        /// <param name="terminal"></param>
        public GenericDefinitionPartSymbol(GenericDefinitionPartSymbol genericDefinitionPart, Terminal terminal) : base(ImmutableArray<ConstrainedGenericSymbol>.Empty) {
            DefinitionPart = genericDefinitionPart;
            CommaOrSemicolon = terminal;

        }

        /// <summary>
        ///     create a new generic definition part
        /// </summary>
        /// <param name="identifier"></param>
        public GenericDefinitionPartSymbol(IdentifierSymbol identifier) : base(ImmutableArray<ConstrainedGenericSymbol>.Empty)
            => Identifier = identifier;

        /// <summary>
        ///     create a new generic definition part
        /// </summary>
        /// <param name="identifier"></param>
        /// <param name="terminal"></param>
        /// <param name="items"></param>
        public GenericDefinitionPartSymbol(IdentifierSymbol identifier, Terminal terminal, ImmutableArray<ConstrainedGenericSymbol> items) : base(items) {
            Identifier = identifier;
            CommaOrSemicolon = terminal;
        }

        /// <summary>
        ///     parse identifier
        /// </summary>
        public IdentifierSymbol Identifier { get; }

        /// <summary>
        ///     generic definition part
        /// </summary>
        public GenericDefinitionPartSymbol DefinitionPart { get; }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal CommaOrSemicolon { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Identifier, visitor);
            AcceptPart(this, DefinitionPart, visitor);
            AcceptPart(this, CommaOrSemicolon, visitor);
            AcceptPart(this, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Identifier.GetSymbolLength() +
               DefinitionPart.GetSymbolLength() +
               CommaOrSemicolon.GetSymbolLength() +
               ItemLength;
    }
}