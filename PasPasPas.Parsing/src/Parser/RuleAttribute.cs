#nullable disable
using System;

namespace PasPasPas.Parsing.Parser {

    /// <summary>
    ///     rule attribute for classes
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class RuleAttribute : Attribute {

        /// <summary>
        ///     create a new rule
        /// </summary>
        /// <param name="ruleName">rule name</param>
        /// <param name="rule">rule</param>
        public RuleAttribute(string ruleName, string rule) : this(ruleName, rule, false) {
        }

        /// <summary>
        ///     create a new rule
        /// </summary>
        /// <param name="ruleName">rule name</param>
        /// <param name="rule">rule</param>
        /// <param name="incomplete">marker for incomplete rules</param>
        public RuleAttribute(string ruleName, string rule, bool incomplete) {
            RuleName = ruleName;
            Rule = rule;
            Incomplete = incomplete;
        }

        /// <summary>
        ///     Rule name
        /// </summary>
        public string Rule { get; }

        /// <summary>
        ///     rule
        /// </summary>
        public string RuleName { get; }

        /// <summary>
        ///     marker for incomplete rules
        /// </summary>
        public bool Incomplete { get; }
    }
}