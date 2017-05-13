using System;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     helper class for exceptions
    /// </summary>
    public class ExceptionHelper {

        /// <summary>
        ///     generates an exception if the argument ist null
        /// </summary>
        /// <param name="argumentName"></param>
        public static void ArgumentIsNull(string argumentName)
            => throw new ArgumentNullException(argumentName);

        /// <summary>
        ///     generates an exception for an invalid operation
        /// </summary>
        public static void InvalidOperation()
            => throw new InvalidOperationException();

        /// <summary>
        ///     generates an index out of range exception
        /// </summary>
        public static void IndexOutOfRange()
            => throw new IndexOutOfRangeException();

    }
}
