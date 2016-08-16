using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     design-time only switch
    /// </summary>
    public class DesignOnly : SyntaxPartBase {


        /// <summary>       
        ///     switch vlaue
        /// </summary>
        public DesignOnlyUnit DesignTimeOnly { get; set; }

    }
}
