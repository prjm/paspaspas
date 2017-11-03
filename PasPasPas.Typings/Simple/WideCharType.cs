using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     wide char type
    /// </summary>
    public class WideCharType : TypeBase {

        /// <summary>
        ///     wide char type
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="withName"></param>
        public WideCharType(int withId, ScopedName withName = null) : base(withId, withName) {
        }

        /// <summary>
        ///     wide char type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.WideCharType;
    }
}
