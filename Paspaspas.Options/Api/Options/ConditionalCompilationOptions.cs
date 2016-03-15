using System;
using System.Collections.Generic;
using System.Linq;

namespace PasPasPas.Api.Options {

    /// <summary>
    ///     options for conditional compilation
    /// </summary>
    public class ConditionalCompilationOptions {

        /// <summary>
        ///     list of conditional defines
        /// </summary>
        public DerivedListOption<ConditionalSymbol> Conditionals { get; }


        /// <summary>
        ///     active conditional
        /// </summary>
        public Stack<List<ICondition>> Conditions { get; }
            = new Stack<List<ICondition>>();

        /// <summary>
        ///     skip tokens
        /// </summary>
        public bool Skip { get; private set; }

        /// <summary>
        ///     deny unit in packages
        /// </summary>
        public DerivedValueOption<DenyUnitInPackages> DenyInPackages { get; }

        /// <summary>
        ///     design-time only package
        /// </summary>
        public DerivedValueOption<DesignOnlyUnit> DesignOnly { get; }

        /// <summary>
        ///     create new option set for conditional compilation
        /// </summary>
        /// <param name="baseOptions"></param>
        public ConditionalCompilationOptions(ConditionalCompilationOptions baseOptions) {
            Conditionals = new DerivedListOption<ConditionalSymbol>(baseOptions?.Conditionals);
            DenyInPackages = new DerivedValueOption<DenyUnitInPackages>(baseOptions?.DenyInPackages);
            DesignOnly = new DerivedValueOption<DesignOnlyUnit>(baseOptions?.DesignOnly);
        }

        /// <summary>
        ///     test if a symbol is defined
        /// </summary>
        /// <param name="symbolName">symbol name</param>
        /// <returns><c>true</c> if the symbol is defined</returns>
        public bool IsSymbolDefined(string symbolName) {
            foreach (var conditional in Conditionals) {
                if (string.Equals(conditional.Name, symbolName, StringComparison.OrdinalIgnoreCase) && conditional.IsActive)
                    return true;
            }
            return false;
        }

        /// <summary>
        ///     clear values
        /// </summary>
        public void Clear() {
            Conditionals.OwnValues.Clear();
            DenyInPackages.ResetToDefault();
            DesignOnly.ResetToDefault();
        }


        /// <summary>
        ///     define a symbol at compile time
        /// </summary>
        /// <param name="symbolName"></param>
        public void DefineSymbol(string symbolName) {
            ToggleSymbol(symbolName, true);
        }

        /// <summary>
        ///     reset local conditionals
        /// </summary>
        public void ResetOnNewUnit() {
            DenyInPackages.ResetToDefault();
            DesignOnly.ResetToDefault();

            for (int i = Conditionals.OwnValues.Count - 1; i >= 0; i--) {
                var symbol = Conditionals.OwnValues[i];
                if (symbol.IsLocal)
                    Conditionals.OwnValues.RemoveAt(i);
                else
                    Conditionals.OwnValues[i].IsActive = true;
            }

            UpdateSkipState();
        }

        /// <summary>
        ///     activate / deactivate symbol at compile time
        /// </summary>
        /// <param name="symbolName"></param>
        /// <param name="isActive"></param>
        private void ToggleSymbol(string symbolName, bool isActive) {

            if (string.IsNullOrEmpty(symbolName))
                return;

            foreach (var conditional in Conditionals) {
                if (string.Equals(conditional.Name, symbolName, StringComparison.OrdinalIgnoreCase)) {
                    conditional.IsActive = isActive;
                    return;
                }
            }

            Conditionals.OwnValues.Add(new ConditionalSymbol() {
                Name = symbolName,
                IsActive = isActive,
                IsLocal = true
            });
        }


        /// <summary>
        ///     add a else condition
        /// </summary>
        public void AddElseCondition() {
            bool anotherConditionMatches = false;
            if (Conditions.Count > 0) {
                var lastConditions = Conditions.Peek();
                foreach (var condition in lastConditions) {
                    if (condition.Matches) {
                        anotherConditionMatches = true;
                        break;
                    }
                }
            }

            AddConditionBranch(new ElseCondition() { Matches = !anotherConditionMatches });
            UpdateSkipState();
        }

        private void AddConditionBranch(ICondition condition) {
            if (Conditions.Count < 1) {
                AddNewCondition(condition);
                return;
            }

            var lastConditions = Conditions.Peek();
            lastConditions.Add(condition);
        }

        /// <summary>
        ///     undefine a symbol at compile time
        /// </summary>
        /// <param name="symbolName">symbol name</param>
        public void UndefineSymbol(string symbolName) {
            ToggleSymbol(symbolName, false);
        }

        /// <summary>
        ///     add a <c>ifndef</c> condition
        /// </summary>
        /// <param name="value"></param>
        public void AddIfNDefCondition(string value) {
            AddNewCondition(new IfDefCondition() { Matches = !IsSymbolDefined(value), SymbolName = value });
            UpdateSkipState();
        }

        /// <summary>
        ///     remove an ifdef condition
        /// </summary>
        public void RemoveIfDefCondition() {
            Conditions.Pop();
            UpdateSkipState();
        }

        /// <summary>
        ///     add a ifdef condition
        /// </summary>
        /// <param name="value">symbol to look for</param>
        public void AddIfDefCondition(string value) {
            AddNewCondition(new IfDefCondition() { Matches = IsSymbolDefined(value), SymbolName = value });
            UpdateSkipState();
        }

        private void AddNewCondition(ICondition ifDefCondition) {
            var conditions = new List<ICondition>();
            conditions.Add(ifDefCondition);
            Conditions.Push(conditions);
        }

        /// <summary>
        ///     updates skipping flag
        /// </summary>
        private void UpdateSkipState() {
            var doSkip = false;

            foreach (var conditions in Conditions) {
                if (conditions.Count < 1)
                    continue;

                var condition = conditions.Last();
                if (!condition.Matches) {
                    doSkip = true;
                    break;
                }
            }

            Skip = doSkip;
        }
    }
}
