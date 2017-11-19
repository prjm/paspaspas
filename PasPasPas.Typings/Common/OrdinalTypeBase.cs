using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     base class for ordinal types
    /// </summary>
    public abstract class OrdinalTypeBase : TypeBase {

        /// <summary>
        ///     create a new ordinal type
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="withName"></param>
        public OrdinalTypeBase(int withId, ScopedName withName = null) : base(withId, withName) {
        }

        /// <summary>
        ///     provide scope
        /// </summary>
        /// <param name="name"></param>
        /// <param name="scope"></param>
        public override void ProvideScope(string name, IScope scope) {
            //.. later record helpers
        }

    }
}
