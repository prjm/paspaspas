using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Hidden {

    /// <summary>
    ///     invalid / error type
    /// </summary>
    internal class ErrorType : CompilerDefinedType, IErrorType {

        /// <summary>
        ///     create a new error type
        /// </summary>
        /// <param name="definingUnit"></param>
        public ErrorType(IUnitType definingUnit) : base(definingUnit) { }

        /// <summary>
        ///     error type
        /// </summary>
        public override BaseType BaseType
            => BaseType.Error;

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => 0;

        /// <summary>
        ///     error name
        /// </summary>
        public override string Name
            => KnownNames.Error;

        /// <summary>
        ///     error name
        /// </summary>
        public override string MangledName
            => KnownNames.Error;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(ITypeDefinition? other)
            => other is IErrorType;
    }
}
