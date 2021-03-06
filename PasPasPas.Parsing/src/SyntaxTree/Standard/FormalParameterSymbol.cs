﻿#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     formal parameter
    /// </summary>
    public class FormalParameterSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new formal parameter
        /// </summary>
        /// <param name="attributes1"></param>
        /// <param name="parameterKind"></param>
        /// <param name="kind"></param>
        /// <param name="attributes2"></param>
        /// <param name="parameterName"></param>
        /// <param name="comma"></param>
        public FormalParameterSymbol(UserAttributesSymbol attributes1, Terminal parameterKind, int kind, UserAttributesSymbol attributes2, IdentifierSymbol parameterName, Terminal comma) {
            Attributes1 = attributes1;
            ParameterKind = parameterKind;
            Attributes2 = attributes2;
            ParameterName = parameterName;
            Comma = comma;
            Kind = kind;
        }

        /// <summary>
        ///     parameter attributes
        /// </summary>
        public SyntaxPartBase Attributes1 { get; }

        /// <summary>
        ///     parameter attributes
        /// </summary>
        public SyntaxPartBase Attributes2 { get; }

        /// <summary>
        ///     parameter name
        /// </summary>
        public IdentifierSymbol ParameterName { get; }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

        /// <summary>
        ///     parameter kind
        /// </summary>
        public int Kind { get; }

        /// <summary>
        ///     parameter kind
        /// </summary>
        public Terminal ParameterKind { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Attributes1, visitor);
            AcceptPart(this, ParameterKind, visitor);
            AcceptPart(this, Attributes2, visitor);
            AcceptPart(this, ParameterName, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Attributes1.GetSymbolLength() +
                ParameterKind.GetSymbolLength() +
                Attributes2.GetSymbolLength() +
                ParameterName.GetSymbolLength() +
                Comma.GetSymbolLength();

    }
}