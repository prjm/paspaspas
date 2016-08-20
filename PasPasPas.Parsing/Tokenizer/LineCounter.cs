namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     newline style
    /// </summary>
    public enum NewLineStyle {

        /// <summary>
        ///     undefined newline style
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     carriage return
        /// </summary>
        Cr = 1,

        /// <summary>
        ///     linefeed
        /// </summary>
        Lf = 2,

        /// <summary>
        ///     carriage return and linefeed
        /// </summary>
        CrLf = 3,

        /// <summary>
        ///     linefeed and carriage return
        /// </summary>
        LfCr = 4,

        /// <summary>
        ///     mixed newline style
        /// </summary>
        Mixed = 99

    }

    /// <summary>
    ///     a simple line counter
    /// </summary>
    public class LineCounter {

        /// <summary>
        ///     current line
        /// </summary>
        private int currentLine = 1;

        /// <summary>
        ///     current column
        /// </summary>
        private int currentColumn = 1;

        /// <summary>
        ///     current line number
        /// </summary>
        public int Line => currentLine;

        /// <summary>
        ///     current column number
        /// </summary>
        public int Column => currentColumn;

        /// <summary>
        ///     newline style
        /// </summary>
        private NewLineStyle style = NewLineStyle.Undefined;

        /// <summary>
        ///     newline style
        /// </summary>
        public NewLineStyle Style => style;

        /// <summary>
        ///     flag to indicate following charcaters
        /// </summary>
        private bool isFollowing = false;

        /// <summary>
        ///     process a input character
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public bool ProcessChar(char input) {
            bool countLine = false;

            if (input == '\r') {
                countLine = SetStyle(NewLineStyle.Cr);
                isFollowing = true;
            }
            else if (input == '\n') {
                countLine = SetStyle(NewLineStyle.Lf);
                isFollowing = true;
            }
            else {
                isFollowing = false;
            }

            if (countLine) {
                currentLine++;
                currentColumn = 1;
            }
            else {
                currentColumn++;
            }

            return false;
        }

        private bool SetStyle(NewLineStyle followingStyle) {
            if (style == NewLineStyle.Undefined) {
                style = followingStyle;
                return true;
            }

            if (style == followingStyle && (style == NewLineStyle.Cr || style == NewLineStyle.Lf))
                return true;

            if (!isFollowing && (style == NewLineStyle.CrLf || style == NewLineStyle.LfCr))
                return true;

            if (style == NewLineStyle.Cr || style == NewLineStyle.CrLf) {
                if (followingStyle == NewLineStyle.Lf) {
                    if (isFollowing) {
                        style = NewLineStyle.CrLf;
                        return false;
                    }

                    style = NewLineStyle.Mixed;
                    return true;
                }
            }

            if (style == NewLineStyle.Lf || style == NewLineStyle.LfCr) {
                if (followingStyle == NewLineStyle.Cr) {
                    if (isFollowing) {
                        style = NewLineStyle.LfCr;
                        return false;
                    }

                    style = NewLineStyle.Mixed;
                    return true;
                }
            }

            if (style != followingStyle)
                style = NewLineStyle.Mixed;

            return true;
        }

        /// <summary>
        ///     test if a given character can trigger a new line
        /// </summary>
        /// <param name="currentChar">char to test</param>
        /// <returns><c>true</c> if the character can trigger a new line</returns>
        public static bool IsNewLineChar(char currentChar)
            => currentChar == '\r' || currentChar == '\n';
    }
}
