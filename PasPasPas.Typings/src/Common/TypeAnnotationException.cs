using System;
using System.Runtime.Serialization;
using PasPasPas.Parsing.SyntaxTree.Abstract;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     exception in type annotation
    /// </summary>
    /// <remarks>thrown in invalid states, not at invalid input</remarks>
    [Serializable]
    public class TypeAnnotationException : Exception {

        [NonSerialized]
        private readonly AbstractSyntaxPartBase element;

        /// <summary>
        ///     create a new exception
        /// </summary>
        public TypeAnnotationException() { }

        /// <summary>
        ///     create a new exception
        /// </summary>
        /// <param name="message">exception message</param>
        public TypeAnnotationException(string message) : base(message) { }

        /// <summary>
        ///     create a new exception
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="innerException">inner exception</param>
        public TypeAnnotationException(string message, Exception innerException) : base(message, innerException) {
        }

        /// <summary>
        ///     create a new exception
        /// </summary>
        /// <param name="message">error message</param>
        /// <param name="element">element which generated the exception</param>
        public TypeAnnotationException(string message, AbstractSyntaxPartBase element) : this(message)
            => this.element = element;

        /// <summary>
        ///     create a new exception (by serialization)
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected TypeAnnotationException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }
}