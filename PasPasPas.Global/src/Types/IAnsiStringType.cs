namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     ANSI string type
    /// </summary>
    public interface IAnsiStringType : IStringType {

        /// <summary>
        ///     code page
        /// </summary>
        ushort WithCodePage { get; }
    }
}
