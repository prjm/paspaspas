using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Parsing.SyntaxTree.Abstract;

namespace PasPasPas.Typings.Common {

    public partial class TypeAnnotator {

        private readonly Stack<IRoutine> currentMethodImplementation
            = new Stack<IRoutine>();


        /// <summary>
        ///     visit a parameter type definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ParameterTypeDefinition element) {
            if (element.TypeValue != default && element.TypeValue.TypeInfo != default) {

                if (currentMethodParameters.Count < 1)
                    return;

                var parms = currentMethodParameters.Peek();

                foreach (var name in element.Parameters) {
                    var param = parms.AddParameter(name.Name.CompleteName);
                    param.SymbolType = element.TypeValue.TypeInfo;
                }
            }
        }

    }
}
