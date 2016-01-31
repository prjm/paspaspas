using System;
using System.Text;

namespace PasPasPas.Api {

    /// <summary>
    ///     helper class to format pascal source
    /// </summary>
    public class PascalFormatter {

        private StringBuilder result
            = new StringBuilder();

        private int indent = 0;

        /// <summary>
        ///     get the formatted result
        /// </summary>
        public string Result
            => result.ToString();

        /// <summary>
        ///     add a keyword
        /// </summary>
        /// <param name="pascalKeyword">keyword to add</param>
        /// <returns>this formatter</returns>
        public PascalFormatter Keyword(string pascalKeyword) {
            result.Append(pascalKeyword);
            return this;
        }

        /// <summary>
        ///     add a subpart
        /// </summary>
        /// <param name="anotherPart">part</param>
        /// <returns>this formatter</returns>
        public PascalFormatter Part(ISyntaxPart anotherPart) {
            if (anotherPart != null)
                anotherPart.ToFormatter(this);
            return this;
        }

        /// <summary>
        ///     add a number
        /// </summary>
        /// <param name="value">numeric value</param>
        public void Number(string value) {
            result.Append(value);
        }

        /// <summary>
        ///     start indentation
        /// </summary>
        public void StartIndent() {
            indent++;
        }

        /// <summary>
        ///     print an operator
        /// </summary>
        /// <param name="op">operator</param>
        public void Operator(string op) {
            result.Append(op);
        }

        /// <summary>
        ///     end indentation
        /// </summary>
        public void EndIndent() {
            indent--;
        }

        /// <summary>
        ///     output a literal
        /// </summary>
        /// <param name="literal"></param>
        internal void Literal(string literal) {
            result.Append(literal);
        }

        /// <summary>
        ///     addds a space character
        /// </summary>
        public PascalFormatter Space() {
            result.Append(" ");
            return this;
        }

        /// <summary>
        ///     output an identifier
        /// </summary>
        /// <param name="value">identifier</param>
        public void Identifier(string value) {
            result.Append(value);
        }

        /// <summary>
        ///     append a linebreak
        /// </summary>
        public PascalFormatter NewLine() {
            result.Append(Environment.NewLine);
            for (int i = 0; i < indent; i++) {
                result.Append("  ");
            }
            return this;
        }

        /// <summary>
        ///     adds a punctuator
        /// </summary>
        /// <param name="punctuation"></param>
        public PascalFormatter Punct(string punctuation) {
            result.Append(punctuation);
            return this;
        }
    }
}
