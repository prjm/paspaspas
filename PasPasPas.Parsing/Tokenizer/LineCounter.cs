namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     newline style
    /// </summary>
    public enum NewlineStyle {

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
        ///     current lline number
        /// </summary>
        public int Line => currentLine;

        /// <summary>
        ///     newline style
        /// </summary>
        private NewlineStyle style = NewlineStyle.Undefined;

        /// <summary>
        ///     newline style
        /// </summary>
        public NewlineStyle Style => style;

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
            bool count = false;

            if (input == '\r') {
                count = SetStyle(NewlineStyle.Cr);
                isFollowing = true;
            }
            else if (input == '\n') {
                count = SetStyle(NewlineStyle.Lf);
                isFollowing = true;
            }
            else {
                isFollowing = false;
            }

            if (count)
                currentLine++;

            return false;
        }

        private bool SetStyle(NewlineStyle followingStyle) {
            if (style == NewlineStyle.Undefined) {
                style = followingStyle;
                return true;
            }

            if (style == followingStyle && (style == NewlineStyle.Cr || style == NewlineStyle.Lf))
                return true;

            if (!isFollowing && (style == NewlineStyle.CrLf || style == NewlineStyle.LfCr))
                return true;

            if (style == NewlineStyle.Cr || style == NewlineStyle.CrLf) {
                if (followingStyle == NewlineStyle.Lf) {
                    if (isFollowing) {
                        style = NewlineStyle.CrLf;
                        return false;
                    }

                    style = NewlineStyle.Mixed;
                    return true;
                }
            }

            if (style == NewlineStyle.Lf || style == NewlineStyle.LfCr) {
                if (followingStyle == NewlineStyle.Cr) {
                    if (isFollowing) {
                        style = NewlineStyle.LfCr;
                        return false;
                    }

                    style = NewlineStyle.Mixed;
                    return true;
                }
            }

            if (style != followingStyle)
                style = NewlineStyle.Mixed;

            return true;
        }
    }
}
