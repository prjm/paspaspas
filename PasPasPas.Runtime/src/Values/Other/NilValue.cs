using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Structured.Other {

    /// <summary>
    ///     nil value
    /// </summary>
    public class NilValue : RuntimeValueBase {

        /// <summary>
        ///     create a new nil value
        /// </summary>
        /// <param name="typeDefinition"></param>
        public NilValue(ITypeDefinition typeDefinition) : base(typeDefinition) {
        }
    }
}
