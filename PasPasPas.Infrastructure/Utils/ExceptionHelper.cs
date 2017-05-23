using System;
using PasPasPas.Infrastructure.Files;

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
    ///     exception for duplicate keys in a dictionary
    /// </summary>
    public class DuplicateKeyInDictionary : ArgumentException {

        /// <summary>
        ///     create a new exception, thrown if a key is already in a dictionary
        /// </summary>
        /// <param name="keyvalue">keyvalue</param>
        /// <param name="argumentName">parameter name</param>
        public DuplicateKeyInDictionary(object keyvalue, string argumentName) :
            base((keyvalue ?? "null").ToString(), argumentName) { }

    }

    /// <summary>
    ///     helper class for exceptions
    /// </summary>
    public class ExceptionHelper {

        /// <summary>
        ///     generates an exception if the argument is null
        /// </summary>
        /// <param name="argumentName"></param>
        public static void ArgumentIsNull(string argumentName)
            => throw new ArgumentNullException(argumentName);

        /// <summary>
        ///     generates an exception if the argument is out of range
        /// </summary>
        /// <param name="argumentValue"></param>
        /// <param name="argumentName"></param>
        public static void ArgumetOutOfRange(object argumentValue, string argumentName)
            => throw new ArgumentOutOfRangeException(argumentName, (argumentValue ?? "null").ToString());

        /// <summary>
        ///     generates an exception if the key is already present in the dictionary
        /// </summary>
        /// <param name="keyvalue">keyvalue</param>
        /// <param name="argumentName">argument name</param>
        public static void DuplicateKeyInDictionary(object keyvalue, string argumentName)
            => throw new DuplicateKeyInDictionary(keyvalue, argumentName);

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
        /// <param name="indexValue">index value</param>
        public static void IndexOutOfRange(object indexValue)
            => throw new IndexOutOfRangeException((indexValue ?? "null").ToString());

    }
}
