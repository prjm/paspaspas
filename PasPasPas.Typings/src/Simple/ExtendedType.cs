namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     extended type
    /// </summary>
    public class ExtendedType : RealType {

        /// <summary>
        ///     create a new extended type
        /// </summary>
        /// <param name="withId"></param>
        public ExtendedType(int withId) : base(withId) { }

        /// <summary>
        ///     type name
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => "Extended";

    }
}
