﻿#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     simple field declaration
    /// </summary>
    public class ClassFieldSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new class field symbol
        /// </summary>
        /// <param name="names"></param>
        /// <param name="colonSymbol"></param>
        /// <param name="typeDecl"></param>
        /// <param name="hint"></param>
        /// <param name="semicolon"></param>
        public ClassFieldSymbol(IdentifierListSymbol names, Terminal colonSymbol, TypeSpecificationSymbol typeDecl, HintingInformationListSymbol hint, Terminal semicolon) {
            Names = names;
            ColonSymbol = colonSymbol;
            TypeDecl = typeDecl;
            Hint = hint;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationListSymbol Hint { get; }

        /// <summary>
        ///     names
        /// </summary>
        public IdentifierListSymbol Names { get; }

        /// <summary>
        ///     type declaration
        /// </summary>
        public TypeSpecificationSymbol TypeDecl { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     colon
        /// </summary>
        public Terminal ColonSymbol { get; }


        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Names, visitor);
            AcceptPart(this, ColonSymbol, visitor);
            AcceptPart(this, TypeDecl, visitor);
            AcceptPart(this, Hint, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Names.GetSymbolLength() +
               ColonSymbol.GetSymbolLength() +
               TypeDecl.GetSymbolLength() +
               Hint.GetSymbolLength() +
               Semicolon.GetSymbolLength();

    }
}