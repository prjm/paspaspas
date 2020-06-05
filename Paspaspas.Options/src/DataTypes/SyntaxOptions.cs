#nullable disable
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     syntax options
    /// </summary>
    public class SyntaxOptions : ISyntaxOptions {

        /// <summary>
        ///     create a new set of syntax options
        /// </summary>
        /// <param name="baseOptions"></param>
        public SyntaxOptions(ISyntaxOptions baseOptions) {
            UseExtendedSyntax = new DerivedValueOption<ExtendedSyntax>(baseOptions?.UseExtendedSyntax);
            OpenStrings = new DerivedValueOption<OpenStringTypeMode>(baseOptions?.OpenStrings);
            LongStrings = new DerivedValueOption<LongStringMode>(baseOptions?.LongStrings);
            VarStringChecks = new DerivedValueOption<ShortVarStringCheck>(baseOptions?.VarStringChecks);
            TypedPointers = new DerivedValueOption<UsePointersWithTypeChecking>(baseOptions?.TypedPointers);
            BoolEval = new DerivedValueOption<BooleanEvaluation>(baseOptions?.BoolEval);
            WritableConstants = new DerivedValueOption<ConstantValue>(baseOptions?.WritableConstants);
            IndexOfFirstCharInString = new DerivedValueOption<FirstCharIndex>(baseOptions?.IndexOfFirstCharInString);
            ScopedEnums = new DerivedValueOption<RequireScopedEnumMode>(baseOptions?.ScopedEnums);
            PointerMath = new DerivedValueOption<PointerManipulation>(baseOptions?.PointerMath);
            VarPropSetter = new DerivedValueOption<VarPropSetterMode>(baseOptions?.VarPropSetter);
        }

        /// <summary>
        ///     switch to enable extended syntax
        /// </summary>
        public IOption<ExtendedSyntax> UseExtendedSyntax { get; }

        /// <summary>
        ///     flag for open strings
        /// </summary>
        public IOption<OpenStringTypeMode> OpenStrings { get; }

        /// <summary>
        ///     flag for long strings
        /// </summary>
        public IOption<LongStringMode> LongStrings { get; }

        /// <summary>
        ///     var string checks
        /// </summary>
        public IOption<ShortVarStringCheck> VarStringChecks { get; }

        /// <summary>
        ///     pointers with types
        /// </summary>
        public IOption<UsePointersWithTypeChecking> TypedPointers { get; }

        /// <summary>
        ///     boolean evaluation style
        /// </summary>
        public IOption<BooleanEvaluation> BoolEval { get; }

        /// <summary>
        ///     writable constants
        /// </summary>
        public IOption<ConstantValue> WritableConstants { get; }

        /// <summary>
        ///     index of first char in a string
        /// </summary>
        public IOption<FirstCharIndex> IndexOfFirstCharInString { get; }

        /// <summary>
        ///     flag to scoped enumerations
        /// </summary>
        public IOption<RequireScopedEnumMode> ScopedEnums { get; }

        /// <summary>
        ///     switch for pointer math
        /// </summary>
        public IOption<PointerManipulation> PointerMath { get; }

        /// <summary>
        ///     var prop setter mode
        /// </summary>
        public IOption<VarPropSetterMode> VarPropSetter { get; }

        /// <summary>
        ///     clear options
        /// </summary>
        public void Clear() {
            UseExtendedSyntax.ResetToDefault();
            OpenStrings.ResetToDefault();
            LongStrings.ResetToDefault();
            VarStringChecks.ResetToDefault();
            TypedPointers.ResetToDefault();
            BoolEval.ResetToDefault();
            WritableConstants.ResetToDefault();
            IndexOfFirstCharInString.ResetToDefault();
            ScopedEnums.ResetToDefault();
            PointerMath.ResetToDefault();
            VarPropSetter.ResetToDefault();
        }

        /// <summary>
        ///     reset options
        /// </summary>
        public void ResetOnNewUnit() {
            OpenStrings.ResetToDefault();
            LongStrings.ResetToDefault();
            VarStringChecks.ResetToDefault();
            BoolEval.ResetToDefault();
            WritableConstants.ResetToDefault();
            IndexOfFirstCharInString.ResetToDefault();
            ScopedEnums.ResetToDefault();
            PointerMath.ResetToDefault();
            VarPropSetter.ResetToDefault();
        }
    }
}
