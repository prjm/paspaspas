using PasPasPas.Infrastructure.Files;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     pascal unit
    /// </summary>
    public class Unit : StandardSyntaxTreeBase {

        /// <summary>
        ///     file path
        /// </summary>
        public IFileReference FilePath
            => UnitHead?.FirstTerminalToken?.FilePath;

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
