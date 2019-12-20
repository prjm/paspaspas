using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     extended type
    /// </summary>
    public class ExtendedType : RealType {

        /// <summary>
        ///     create a new extended type
        /// </summary>
        /// <param name="withId"></param>
        public ExtendedType(int withId) : base(withId, 80) { }

        /// <summary>
        ///     long type name
        /// </summary>
        public override string LongName
            => KnownNames.Extended;

        /// <summary>
        ///     short type name
        /// </summary>
        public override string ShortName
            => KnownNames.G;

    }
}
