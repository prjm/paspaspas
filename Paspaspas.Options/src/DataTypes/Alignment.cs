using System;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     record alignment
    /// </summary>
    public enum Alignment {

        /// <summary>
        ///     undefined alignment
        /// </summary>
        Undefined = 0,

        /// <summary>
        ///     unaligned
        /// </summary>
        Unaligned = 1,

        /// <summary>
        ///     word alignment
        /// </summary>
        Word = 2,

        /// <summary>
        ///     double word alignment
        /// </summary>
        DoubleWord = 3,

        /// <summary>
        ///     quad word aligmnet
        /// </summary>
        QuadWord = 4,

        /// <summary>
        ///     double quad word aligment
        /// </summary>
        DoubleQuadWord = 5,

    }

    /// <summary>
    ///     helper for alignment
    /// </summary>
    public static class AlignmentHelper {

        /// <summary>
        ///     parse alignment from string
        /// </summary>
        /// <param name="value"></param>
        /// <param name="alignment"></param>
        /// <returns></returns>
        public static bool TryParse(string value, out Alignment alignment) {
            int alignValue;

            if (string.Equals(value, "+", StringComparison.Ordinal)) {
                alignment = Alignment.QuadWord;
                return true;
            }

            if (string.Equals(value, "-", StringComparison.Ordinal)) {
                alignment = Alignment.Unaligned;
                return true;
            }

            if (int.TryParse(value, out alignValue)) {

                switch (alignValue) {
                    case 1:
                        alignment = Alignment.Unaligned;
                        return true;

                    case 2:
                        alignment = Alignment.Word;
                        return true;

                    case 4:
                        alignment = Alignment.DoubleWord;
                        return true;


                    case 8:
                        alignment = Alignment.QuadWord;
                        return true;

                    case 16:
                        alignment = Alignment.DoubleQuadWord;
                        return true;
                }

            }

            alignment = Alignment.Undefined;
            return false;
        }

    }

}