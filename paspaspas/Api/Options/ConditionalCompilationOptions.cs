using System;
using System.Collections.Generic;

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
        public Stack<ICondition> Conditions { get; }
            = new Stack<ICondition>();

        /// <summary>
        ///     skip tokens
        /// </summary>
        public bool Skip { get; private set; }

        /// <summary>
        ///     deny unit in packages
        /// </summary>
        public DerivedValueOption<DenyUnitInPackages> DenyInPackages { get; }

        /// <summary>
        ///     create new option set for conditional compilation
        /// </summary>
        /// <param name="baseOptions"></param>
        public ConditionalCompilationOptions(ConditionalCompilationOptions baseOptions) {
            Conditionals = new DerivedListOption<ConditionalSymbol>(baseOptions?.Conditionals);
            DenyInPackages = new DerivedValueOption<DenyUnitInPackages>(baseOptions?.DenyInPackages);
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
        ///     undefine a symbol at compile time
        /// </summary>
        /// <param name="symbolName">symbol name</param>
        public void UndefineSymbol(string symbolName) {
            ToggleSymbol(symbolName, false);
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
            Conditions.Push(new IfDefCondition() { Matches = IsSymbolDefined(value), SymbolName = value });
            UpdateSkipState();
        }

        /// <summary>
        ///     updates skipping flag
        /// </summary>
        private void UpdateSkipState() {
            var doSkip = false;

            foreach (var condition in Conditions) {
                if (!condition.Matches) {
                    doSkip = true;
                    break;
                }
            }

            Skip = doSkip;
        }
    }
}
