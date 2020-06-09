using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Runtime.Values.Structured.Other {

    /// <summary>
    ///     nil value
    /// </summary>
    internal class NilValue : RuntimeValueBase {

        /// <summary>
        ///     create a new nil value
        /// </summary>
        /// <param name="typeDefinition"></param>
        public NilValue(ITypeDefinition typeDefinition) : base(typeDefinition) {
        }

        public override bool Equals(IValue? other)
            => other is NilValue;

        public override int GetHashCode()
            => 0;

        public override string GetValueString()
            => KnownNames.Nil;
    }
}
