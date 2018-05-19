﻿using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     pseudo x64 operation
    /// </summary>
    public class AsmPseudoOp : StandardSyntaxTreeBase {

        /// <summary>
        ///     operation kind
        /// </summary>
        public Identifier Kind { get; set; }

        /// <summary>
        ///     skip stack frames
        /// </summary>
        public bool NoFrame { get; set; }

        /// <summary>
        ///     number of parameters
        /// </summary>
        public StandardInteger NumberOfParams { get; set; }

        /// <summary>
        ///     params pseudo op
        /// </summary>
        public bool ParamsOperation { get; set; }

        /// <summary>
        ///     pushenv pseudo op
        /// </summary>
        public bool PushEnvOperation { get; set; }

        /// <summary>
        ///     register name
        /// </summary>
        public Identifier Register { get; set; }

        /// <summary>
        ///     savenv pseudo op
        /// </summary>
        public bool SaveEnvOperation { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }
    }
}