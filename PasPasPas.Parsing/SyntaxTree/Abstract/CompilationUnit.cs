using PasPasPas.Infrastructure.Input;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     
    /// </summary>
    public class CompilationUnit : SymbolTableEntryBase {

        /// <summary>
        ///     unit file type
        /// </summary>
        public CompilationUnitType FileType { get; set; }

        /// <summary>
        ///     Name of the unit
        /// </summary>
        public SymbolName UnitName { get; set; }

        /// <summary>
        ///     name of the unit
        /// </summary>
        public override string SymbolName
        {
            get
            {
                if (UnitName != null)
                    return UnitName.CompleteName;
                else
                    return "Unit_" + GetHashCode();
            }
        }

        /// <summary>
        ///     file reference of the unit
        /// </summary>
        public IFileReference FilePath { get; set; }
    }
}