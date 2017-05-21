using System;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     exception for empty string arguments
    /// </summary>
    public class ArgumentStringEmptyException : ArgumentException {

        /// <summary>
        ///     creates a new exception, thrown if a string argument is empty or whitepace
        /// </summary>
        /// <param name="argumentName">name of the argument</param>
        public ArgumentStringEmptyException(string argumentName) : base("", argumentName) { }
    }

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
        ///     generates an exception for empty strings
        /// </summary>
        /// <param name="argumentName">name of the argument</param>
        public static void StringEmpty(string argumentName)
            => new ArgumentStringEmptyException(argumentName);

        /// <summary>
        ///     generates an index out of range exception
        /// </summary>
        public static void IndexOutOfRange()
            => throw new IndexOutOfRangeException();

    }
}
