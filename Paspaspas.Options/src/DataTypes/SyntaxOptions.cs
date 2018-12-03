namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     syntax options
    /// </summary>
    public class SyntaxOptions {

        /// <summary>
        ///     create a new set of syntax options
        /// </summary>
        /// <param name="baseOptions"></param>
        public SyntaxOptions(SyntaxOptions baseOptions) {
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
        }

        /// <summary>
        ///     switch to enable extended syntax
        /// </summary>
        public DerivedValueOption<ExtendedSyntax> UseExtendedSyntax { get; }

        /// <summary>
        ///     flag for open strings
        /// </summary>
        public DerivedValueOption<OpenStringTypeMode> OpenStrings { get; }

        /// <summary>
        ///     flag for long strings
        /// </summary>
        public DerivedValueOption<LongStringMode> LongStrings { get; }

        /// <summary>
        ///     var string checks
        /// </summary>
        public DerivedValueOption<ShortVarStringCheck> VarStringChecks { get; }

        /// <summary>
        ///     pointers with types
        /// </summary>
        public DerivedValueOption<UsePointersWithTypeChecking> TypedPointers { get; }

        /// <summary>
        ///     boolean evaluation style
        /// </summary>
        public DerivedValueOption<BooleanEvaluation> BoolEval { get; }

        /// <summary>
        ///     writable constants
        /// </summary>
        public DerivedValueOption<ConstantValue> WritableConstants { get; }

        /// <summary>
        ///     index of first char in a string
        /// </summary>
        public DerivedValueOption<FirstCharIndex> IndexOfFirstCharInString { get; }

        /// <summary>
        ///     flag to scoped enumerations
        /// </summary>
        public DerivedValueOption<RequireScopedEnumMode> ScopedEnums { get; }

        /// <summary>
        ///     switch for pointer math
        /// </summary>
        public DerivedValueOption<PointerManipulation> PointerMath { get; }

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
        }
    }
}
