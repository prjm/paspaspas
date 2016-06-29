namespace PasPasPas.Infrastructure.Input {

    /// <summary>
    ///     position in a text file
    /// </summary>
    public class TextFilePosition {

        /// <summary>
        ///     create a new textfile position
        /// </summary>
        public TextFilePosition() {
            Line = 1;
            Column = 1;
        }

        /// <summary>
        ///     line
        /// </summary>
        public int Column { get; set; }

        /// <summary>
        ///     column
        /// </summary>
        public int Line { get; set; }
    }
}