#nullable disable
using System.Collections.Generic;
using PasPasPas.Globals.Types;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     helper structure for structured type members
    /// </summary>
    public class MemberStatus {

        /// <summary>
        ///     visibility
        /// </summary>
        public MemberVisibility Visibility { get; set; }

        /// <summary>
        ///     <c>true</c> if the member is a class item
        /// </summary>
        public bool ClassItem { get; set; }

        /// <summary>
        ///     user attributes
        /// </summary>
        public List<SymbolAttributeItem> Attributes { get; }
            = new List<SymbolAttributeItem>();

    }
}
