using PasPasPas.Infrastructure.Log;
using PasPasPas.Options.Bundles;
using System;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     options for conditional compilation
    /// </summary>
    public class ConditionalCompilationOptions {

        /// <summary>
        ///     list of conditional defines
        /// </summary>
        public DerivedListOptionCollection<ConditionalSymbol> Conditionals { get; }

        /// <summary>
        ///     active conditional
        /// </summary>
        public IConditionBranch CurrentCondition { get; private set; }

        /// <summary>
        ///     skip tokens
        /// </summary>
        public bool Skip { get; private set; }

        /// <summary>
        ///     deny unit in packages
        /// </summary>
        public DerivedValueOption<DenyUnitInPackage> DenyInPackages { get; }

        /// <summary>
        ///     design-time only package
        /// </summary>
        public DerivedValueOption<DesignOnlyUnit> DesignOnly { get; }

        /// <summary>
        ///     test fif conditions are availiabe
        /// </summary>
        public bool HasConditions
            => CurrentCondition != null;

        /// <summary>
        ///     create new option set for conditional compilation
        /// </summary>
        /// <param name="baseOptions"></param>
        public ConditionalCompilationOptions(ConditionalCompilationOptions baseOptions) {
            Conditionals = new DerivedListOptionCollection<ConditionalSymbol>(baseOptions?.Conditionals);
            DenyInPackages = new DerivedValueOption<DenyUnitInPackage>(baseOptions?.DenyInPackages);
            DesignOnly = new DerivedValueOption<DesignOnlyUnit>(baseOptions?.DesignOnly);
        }

        /// <summary>
        ///     test if a symbol is defined
        /// </summary>
        /// <param name="symbolName">symbol name</param>
        /// <returns><c>true</c> if the symbol is defined</returns>
        public bool IsSymbolDefined(string symbolName) {
            foreach (ConditionalSymbol conditional in Conditionals) {
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
        public void DefineSymbol(string symbolName) => ToggleSymbol(symbolName, true);

        /// <summary>
        ///     reset local conditionals
        /// </summary>
        public void ResetOnNewUnit(LogSource logSource) {
            DenyInPackages.ResetToDefault();
            DesignOnly.ResetToDefault();

            for (var i = Conditionals.OwnValues.Count - 1; i >= 0; i--) {
                ConditionalSymbol symbol = Conditionals.OwnValues[i];
                if (symbol.IsLocal)
                    Conditionals.OwnValues.RemoveAt(i);
                else
                    Conditionals.OwnValues[i].IsActive = true;
            }

            while (CurrentCondition != null) {
                logSource.LogError(OptionSet.PendingCondition, CurrentCondition);
                CurrentCondition = CurrentCondition.ParentBranch;
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

            foreach (ConditionalSymbol conditional in Conditionals) {
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
            var inInvalidBranch = false;
            var anotherConditionMatches = true;

            if (CurrentCondition != null && CurrentCondition.ParentBranch != null && !CurrentCondition.ParentBranch.Matches) {
                inInvalidBranch = true;
            }
            else if (CurrentCondition != null) {
                anotherConditionMatches = false;
                foreach (IConditionBranch condition in CurrentCondition.Parent.Conditions) {
                    if (condition.Matches) {
                        anotherConditionMatches = true;
                        break;
                    }
                }
            }

            AddConditionBranch(new ElseCondition() { Matches = !anotherConditionMatches && !inInvalidBranch });
            UpdateSkipState();
        }

        private void AddConditionBranch(ICondition condition) {
            IConditionRoot root;

            if (CurrentCondition == null) {
                root = new ConditionRoot(null);
            }
            else {
                root = CurrentCondition.Parent;
            }

            var branch = new ConditionBranch(root, condition);
            root.Conditions.Add(branch);
            CurrentCondition = branch;
        }

        /// <summary>
        ///     undefine a symbol at compile time
        /// </summary>
        /// <param name="symbolName">symbol name</param>
        public void UndefineSymbol(string symbolName) => ToggleSymbol(symbolName, false);

        /// <summary>
        ///     add a <c>ifndef</c> condition
        /// </summary>     
        /// <param name="value"></param>
        public void AddIfNDefCondition(string value) {
            if (string.IsNullOrWhiteSpace(value))
                return;

            AddNewCondition(new IfDefCondition() { Matches = !IsSymbolDefined(value), SymbolName = value });
            UpdateSkipState();
        }

        /// <summary>
        ///     remove an ifdef condition
        /// </summary>
        public void RemoveIfDefCondition() {
            CurrentCondition = CurrentCondition.ParentBranch;
            UpdateSkipState();
        }

        /// <summary>
        ///     add a ifdef condition
        /// </summary>
        /// <param name="value">symbol to look for</param>
        public void AddIfDefCondition(string value) {
            if (string.IsNullOrWhiteSpace(value))
                return;

            AddNewCondition(new IfDefCondition() { Matches = IsSymbolDefined(value), SymbolName = value });
            UpdateSkipState();
        }

        /// <summary>
        ///     add a new if condition
        /// </summary>
        /// <param name="isValid"></param>
        public void AddIfCondition(bool isValid) {
            AddNewCondition(new IfCondition() { Matches = isValid });
            UpdateSkipState();
        }

        private void AddNewCondition(ICondition condition) {
            var root = new ConditionRoot(CurrentCondition);
            var branch = new ConditionBranch(root, condition);
            root.Conditions.Add(branch);
            CurrentCondition = branch;
        }

        /// <summary>
        ///     updates skipping flag
        /// </summary>
        private void UpdateSkipState() {
            var doSkip = false;
            IConditionBranch condition = CurrentCondition;

            while (condition != null) {
                if (!condition.Matches) {
                    doSkip = true;
                    break;
                }

                condition = condition.ParentBranch;
            }

            Skip = doSkip;
        }

        /// <summary>
        ///     add a new condition
        /// </summary>
        /// <param name="switchKind"></param>
        /// <param name="requiredInfo"></param>
        /// <param name="switchInfo"></param>
        public void AddIfOptCondition(string switchKind, SwitchInfo requiredInfo, SwitchInfo switchInfo) {
            AddNewCondition(new IfOptCondition() {
                Matches = (requiredInfo != SwitchInfo.Undefined) && (requiredInfo == switchInfo),
                SwitchName = switchKind,
                RequiredCondition = requiredInfo
            });
            UpdateSkipState();
        }
    }


}
