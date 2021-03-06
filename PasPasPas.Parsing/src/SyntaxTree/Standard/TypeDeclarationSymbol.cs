﻿#nullable disable
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     type declaration
    /// </summary>
    public class TypeDeclarationSymbol : StandardSyntaxTreeBase {

        /// <summary>
        ///     create a new type declaration
        /// </summary>
        /// <param name="attributes"></param>
        /// <param name="genericTypeIdent"></param>
        /// <param name="equalsSign"></param>
        /// <param name="typeSpecification"></param>
        /// <param name="hints"></param>
        /// <param name="semicolon"></param>
        public TypeDeclarationSymbol(UserAttributesSymbol attributes, GenericTypeIdentifierSymbol genericTypeIdent, Terminal equalsSign, TypeSpecificationSymbol typeSpecification, HintingInformationListSymbol hints, Terminal semicolon) {
            Attributes = attributes;
            TypeId = genericTypeIdent;
            EqualsSign = equalsSign;
            TypeSpecification = typeSpecification;
            Hint = hints;
            Semicolon = semicolon;
        }

        /// <summary>
        ///     user attributes
        /// </summary>
        public UserAttributesSymbol Attributes { get; }

        /// <summary>
        ///     hinting directives
        /// </summary>
        public HintingInformationListSymbol Hint { get; }

        /// <summary>
        ///     equals sign
        /// </summary>
        public Terminal Semicolon { get; }

        /// <summary>
        ///     type specification
        /// </summary>
        public TypeSpecificationSymbol TypeSpecification { get; }

        /// <summary>
        ///     type id
        /// </summary>
        public GenericTypeIdentifierSymbol TypeId { get; }

        /// <summary>
        ///     equals sign
        /// </summary>
        public Terminal EqualsSign { get; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, Attributes, visitor);
            AcceptPart(this, TypeId, visitor);
            AcceptPart(this, EqualsSign, visitor);
            AcceptPart(this, TypeSpecification, visitor);
            AcceptPart(this, Hint, visitor);
            AcceptPart(this, Semicolon, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => Attributes.GetSymbolLength() +
               TypeId.GetSymbolLength() +
               EqualsSign.GetSymbolLength() +
               TypeSpecification.GetSymbolLength() +
               Hint.GetSymbolLength() +
               Semicolon.GetSymbolLength();

    }
}