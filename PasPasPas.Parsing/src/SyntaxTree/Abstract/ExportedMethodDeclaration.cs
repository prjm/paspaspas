using System;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Utils;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     exported method declaration
    /// </summary>
    public class ExportedMethodDeclaration : MethodDeclaration, IExpressionTarget {

        /// <summary>
        ///     <b>true</b> if the exported symbol name stays in memory
        ///     (only for backwards compatibility)
        /// </summary>
        public bool IsResident { get; set; }

        private IExpression nameExpression;
        private IExpression indexExpression;

        /// <summary>
        ///     constant values
        /// </summary>
        public IExpression Value {
            get => nameExpression ?? indexExpression;

            set {
                if (HasIndex && indexExpression == null)
                    indexExpression = value;
                else if (HasName && nameExpression == null)
                    nameExpression = value;
                else
                    throw new InvalidOperationException();
            }
        }

        /// <summary>
        ///     export has a name
        /// </summary>
        public bool HasName { get; set; }

        /// <summary>
        ///     export has an index
        /// </summary>
        public bool HasIndex { get; set; }

        /// <summary>
        ///     enumerate all parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (var part in base.Parts)
                    yield return part;
                if (indexExpression != null)
                    yield return indexExpression;
                if (nameExpression != null)
                    yield return nameExpression;
            }
        }

        /// <summary>
        ///     accept parts
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptBaseParts(visitor);
            AcceptPart(this, indexExpression, visitor);
            AcceptPart(this, nameExpression, visitor);
            visitor.EndVisit(this);
        }
    }
}
