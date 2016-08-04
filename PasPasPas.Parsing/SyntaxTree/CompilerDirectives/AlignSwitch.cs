using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Api;
using PasPasPas.Options.DataTypes;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    public class AlignSwitch : SyntaxPartBase {
        public Alignment AlignValue { get; internal set; }

        public override void ToFormatter(PascalFormatter result) {
            throw new NotImplementedException();
        }
    }
}
