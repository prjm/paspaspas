﻿#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     procedure declaration
    /// </summary>
    public class ProcedureDeclarationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     procedure declaration
        /// </summary>
        /// <param name="attributes">attributes</param>
        /// <param name="heading"></param>
        /// <param name="semicolon"></param>
        /// <param name="directives"></param>
        /// <param name="body"></param>
        /// <param name="semicolon2"></param>
        public ProcedureDeclarationSymbol(UserAttributesSymbol attributes, ProcedureDeclarationHeadingSymbol heading, Terminal semicolon, FunctionDirectivesSymbol directives, BlockSymbol body, Terminal semicolon2) {
            Attributes = attributes;
            Heading = heading;
            Semicolon = semicolon;
            Directives = directives;
            Body = body;
            Semicolon2 = semicolon2;
        }

        /// <summary>
        ///     attributes
        /// </summary>
        public UserAttributesSymbol Attributes { get; }

        /// <summary>
        ///     function directives
        /// </summary>
        public FunctionDirectivesSymbol Directives { get; }

        /// <summary>
        ///     procedure declaration heading
        /// </summary>
        public ProcedureDeclarationHeadingSymbol Heading { get; }

        /// <summary>
        ///     procedure body
        /// </summary>
        public BlockSymbol ProcedureBody { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     block body
        /// </summary>
        public BlockSymbol Body { get; }

        /// <summary>
        ///     semicolon
        /// </summary>
        public Terminal Semicolon2 { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Attributes, visitor);
            AcceptPart(this, Heading, visitor);
            AcceptPart(this, Semicolon, visitor);
            AcceptPart(this, Directives, visitor);
            AcceptPart(this, Body, visitor);
            AcceptPart(this, Semicolon2, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Attributes.GetSymbolLength() +
                Heading.GetSymbolLength() +
                Semicolon.GetSymbolLength() +
                Directives.GetSymbolLength() +
                Body.GetSymbolLength() +
                Semicolon2.GetSymbolLength();

    }
}
