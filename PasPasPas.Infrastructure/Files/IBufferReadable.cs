namespace PasPasPas.Infrastructure.Files {

    /// <summary>
    ///     file, readable by a file buffer
    /// </summary>
    public interface IBufferReadable {

        /// <summary>
        ///     connect file to a buffer item
        /// </summary>
        /// <param name="item">buffer item</param>
        void ToBufferItem(FileBufferItem item);

    }
}
