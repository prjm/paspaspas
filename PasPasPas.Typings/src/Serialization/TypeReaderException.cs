#nullable disable
using System;
using System.Runtime.Serialization;

namespace PasPasPas.Typings.Serialization {

    /// <summary>
    ///     exception for type reading / writing
    /// </summary>
    [Serializable]
    public class TypeReaderWriteException : Exception {

        /// <summary>
        ///     create a new exception
        /// </summary>
        public TypeReaderWriteException() { }

        /// <summary>
        ///     create a new exception
        /// </summary>
        /// <param name="message"></param>
        public TypeReaderWriteException(string message) : base(message) { }

        /// <summary>
        ///     create a new exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public TypeReaderWriteException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        ///     create a new exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected TypeReaderWriteException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    /// <summary>
    ///     unexpected end of file exception
    /// </summary>
    [Serializable]
    public class UnexpectedEndOfFileException : TypeReaderWriteException {

        /// <summary>
        ///     create a new exception
        /// </summary>
        public UnexpectedEndOfFileException() { }

        /// <summary>
        ///     create a new exception
        /// </summary>
        /// <param name="message"></param>
        public UnexpectedEndOfFileException(string message) : base(message) { }

        /// <summary>
        ///     create a new exception
        /// </summary>
        /// <param name="message"></param>
        /// <param name="inner"></param>
        public UnexpectedEndOfFileException(string message, Exception inner) : base(message, inner) { }

        /// <summary>
        ///     create a new exception
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected UnexpectedEndOfFileException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}
