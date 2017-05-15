using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     formal parameter definition
    /// </summary>
    public class ParameterDefinition : SymbolTableEntryBase {

        /// <summary>
        ///     attributes
        /// </summary>
        public IList<SymbolAttribute> Attributes { get; set; }

        /// <summary>
        ///     parameter name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     parameter kind
        /// </summary>
        public ParameterReferenceKind ParameterKind { get; set; }

        /// <summary>
        ///     symbol name
        /// </summary>
        protected override string InternalSymbolName
            => Name?.CompleteName;

        /// <summary>
        ///     map parameter reference kind
        /// </summary>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        [System.Obsolete]
        public static ParameterReferenceKind MapKind(int parameterType) {
            switch (parameterType) {
                case TokenKind.Const:
                    return ParameterReferenceKind.Const;
                case TokenKind.Var:
                    return ParameterReferenceKind.Var;
                case TokenKind.Out:
                    return ParameterReferenceKind.Out;
                default:
                    return ParameterReferenceKind.Undefined;
            }
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}
