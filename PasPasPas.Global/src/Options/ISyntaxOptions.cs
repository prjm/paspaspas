using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     syntax options
    /// </summary>
    public interface ISyntaxOptions {

        /// <summary>
        ///     boolean evaluation
        /// </summary>
        IOption<BooleanEvaluation> BoolEval { get; }

        /// <summary>
        ///     option to use extended syntax
        /// </summary>
        IOption<ExtendedSyntax> UseExtendedSyntax { get; }

        /// <summary>
        ///     long string mode
        /// </summary>
        IOption<LongStringMode> LongStrings { get; }

        /// <summary>
        ///     open string mode
        /// </summary>
        IOption<OpenStringTypeMode> OpenStrings { get; }

        /// <summary>
        ///     writable constants
        /// </summary>
        IOption<ConstantValue> WritableConstants { get; }

        /// <summary>
        ///     string index mode
        /// </summary>
        IOption<FirstCharIndex> IndexOfFirstCharInString { get; }

        /// <summary>
        ///     string checks
        /// </summary>
        IOption<ShortVarStringCheck> VarStringChecks { get; }

        /// <summary>
        ///     typed pointers
        /// </summary>
        IOption<UsePointersWithTypeChecking> TypedPointers { get; }

        /// <summary>
        ///     use scoped enumerations
        /// </summary>
        IOption<RequireScopedEnumMode> ScopedEnums { get; }

        /// <summary>
        ///     enabled pointer math
        /// </summary>
        IOption<PointerManipulation> PointerMath { get; }

        /// <summary>
        ///     var prop setter mode
        /// </summary>
        IOption<VarPropSetterMode> VarPropSetter { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        void Clear();

        /// <summary>
        ///     reset options on a new unit
        /// </summary>
        void ResetOnNewUnit();
    }
}