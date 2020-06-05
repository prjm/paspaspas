#nullable disable
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     options for conditional compilation
    /// </summary>
    public interface IConditionalCompilationOptions {

        /// <summary>
        ///     skip tokens
        /// </summary>
        bool Skip { get; }

        /// <summary>
        ///     check if conditions are present
        /// </summary>
        bool HasConditions { get; }

        /// <summary>
        ///     deny unit in a package
        /// </summary>
        IOption<DenyUnitInPackage> DenyInPackages { get; }

        /// <summary>
        ///     design only flag
        /// </summary>
        IOption<DesignOnlyUnit> DesignOnly { get; }

        /// <summary>
        ///     conditional symbols
        /// </summary>
        IEnumerableOptionCollection<ConditionalSymbol> Conditionals { get; }

        /// <summary>
        ///     add a <c>ifndef</c> condition
        /// </summary>
        /// <param name="symbolName"></param>
        void AddIfNDefCondition(string symbolName);

        /// <summary>
        ///     add a <c>ifde</c> condition
        /// </summary>
        /// <param name="symbolName"></param>
        void AddIfDefCondition(string symbolName);

        /// <summary>
        ///     define a symbol
        /// </summary>
        /// <param name="symbolName"></param>
        void DefineSymbol(string symbolName);

        /// <summary>
        ///     undefine a symbol
        /// </summary>
        /// <param name="symbolName"></param>
        void UndefineSymbol(string symbolName);

        /// <summary>
        ///     remove a <c>ifdef</c> condition
        /// </summary>
        void RemoveIfDefCondition();

        /// <summary>
        ///     add an if condition
        /// </summary>
        /// <param name="isValid"></param>
        void AddIfCondition(bool isValid);

        /// <summary>
        ///     add an else condition
        /// </summary>
        void AddElseCondition();

        /// <summary>
        ///     clear all options
        /// </summary>
        void Clear();

        /// <summary>
        ///     clear options on a new unit
        /// </summary>
        /// <param name="logSource"></param>
        void ResetOnNewUnit(ILogSource logSource);

        /// <summary>
        ///     test if a given symbol is defined
        /// </summary>
        /// <param name="symbolName"></param>
        /// <returns></returns>
        bool IsSymbolDefined(string symbolName);

        /// <summary>
        ///     add <c>ifopt</c> condition
        /// </summary>
        /// <param name="switchKind"></param>
        /// <param name="switchInfo"></param>
        /// <param name="switchState"></param>
        void AddIfOptCondition(string switchKind, SwitchInfo switchInfo, SwitchInfo switchState);
    }
}