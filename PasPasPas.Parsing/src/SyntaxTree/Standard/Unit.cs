using PasPasPas.Infrastructure.Files;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     pascal unit
    /// </summary>
    public class Unit : StandardSyntaxTreeBase {

        /// <summary>
        ///     hints
        /// </summary>
        public HintingInformationList Hints
            => UnitHead?.Hint;

        /// <summary>
        ///     unit block
        /// </summary>
        public UnitBlock UnitBlock { get; set; }

        /// <summary>
        ///     unit head section
        /// </summary>
        public UnitHead UnitHead { get; set; }

        /// <summary>
        ///     unit implementation section
        /// </summary>
        public UnitImplementation UnitImplementation { get; set; }

        /// <summary>
        ///     unit interface
        /// </summary>
        public UnitInterface UnitInterface { get; set; }

        /// <summary>
        ///     unit name
        /// </summary>
        public NamespaceName UnitName
            => UnitHead?.UnitName;

        /// <summary>
        ///     path of the unit
        /// </summary>
        public IFileReference FilePath { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
