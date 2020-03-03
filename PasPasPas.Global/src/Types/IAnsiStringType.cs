namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     ANSI string type
    /// </summary>
    public interface IAnsiStringType : IStringType {
        ushort WithCodePage { get; }
    }
}
