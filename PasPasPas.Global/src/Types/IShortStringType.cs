#nullable disable
namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     short string type definition
    /// </summary>
    public interface IShortStringType : IStringType {

        /// <summary>
        ///     string size
        /// </summary>
        byte Size { get; }
    }
}
