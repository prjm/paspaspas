using System.Collections.Generic;
using PasPasPas.Globals.Options;
using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     warning options
    /// </summary>
    public class WarningOptions : IWarningOptions {

        /// <summary>
        ///     create a new set of warning options
        /// </summary>
        public WarningOptions() : this(null) { }

        /// <summary>
        ///     create a new set of warning options
        /// </summary>
        /// <param name="parentOptions">parent options</param>
        public WarningOptions(IWarningOptions parentOptions) {
            ParentOptions = parentOptions;
            options = new Dictionary<WarningKey, WarningOption>();
            AddOptions();
        }

        /// <summary>
        ///     add possible options
        /// </summary>
        private void AddOptions() {
            AddOption("W1001", "SYMBOL_DEPRECATED");
            AddOption("W1000", "SYMBOL_LIBRARY");
            AddOption("W1002", "SYMBOL_PLATFORM");
            AddOption("W1003", "SYMBOL_EXPERIMENTAL");
            AddOption("W1004", "UNIT_LIBRARY");
            AddOption("W1005", "UNIT_PLATFORM");
            AddOption("W1006", "UNIT_DEPRECATED");
            AddOption("W1007", "UNIT_EXPERIMENTAL");
            AddOption("W1008", "HRESULT_COMPAT");
            AddOption("W1009", "HIDING_MEMBER");
            AddOption("W1010", "HIDDEN_VIRTUAL");
            AddOption("W1011", "GARBAGE");
            AddOption("W1012", "BOUNDS_ERROR");
            AddOption("W1013", "ZERO_NIL_COMPAT");
            AddOption("W1014", "STRING_CONST_TRUNCED");
            AddOption("W1015", "FOR_LOOP_VAR_VARPAR");
            AddOption("W1016", "TYPED_CONST_VARPAR");
            AddOption("W1017", "ASG_TO_TYPED_CONST");
            AddOption("W1018", "CASE_LABEL_RANGE");
            AddOption("W1019", "FOR_VARIABLE");
            AddOption("W1020", "CONSTRUCTING_ABSTRACT");
            AddOption("W1021", "COMPARISON_FALSE");
            AddOption("W1022", "COMPARISON_TRUE");
            AddOption("W1023", "COMPARING_SIGNED_UNSIGNED");
            AddOption("W1024", "COMBINING_SIGNED_UNSIGNED");
            AddOption("W1025", "UNSUPPORTED_CONSTRUCT");
            AddOption("W1026", "FILE_OPEN");
            AddOption("W1027", "FILE_OPEN_UNITSRC");
            AddOption("W1028", "BAD_GLOBAL_SYMBOL");
            AddOption("W1029", "DUPLICATE_CTOR_DTOR");
            AddOption("W1030", "INVALID_DIRECTIVE");
            AddOption("W1031", "PACKAGE_NO_LINK");
            AddOption("W1032", "PACKAGED_THREADVAR");
            AddOption("W1033", "IMPLICIT_IMPORT");
            AddOption("W1034", "HPPEMIT_IGNORED");
            AddOption("W1035", "NO_RETVAL");
            AddOption("W1036", "USE_BEFORE_DEF");
            AddOption("W1037", "FOR_LOOP_VAR_UNDEF");
            AddOption("W1038", "UNIT_NAME_MISMATCH");
            AddOption("W1039", "NO_CFG_FILE_FOUND");
            AddOption("W1040", "IMPLICIT_VARIANTS");
            AddOption("W1041", "UNICODE_TO_LOCALE");
            AddOption("W1042", "LOCALE_TO_UNICODE");
            AddOption("W1043", "IMAGEBASE_MULTIPLE");
            AddOption("W1044", "SUSPICIOUS_TYPECAST");
            AddOption("W1045", "PRIVATE_PROPACCESSOR");
            AddOption("W1046", "UNSAFE_TYPE");
            AddOption("W1047", "UNSAFE_CODE");
            AddOption("W1048", "UNSAFE_CAST");
            AddOption("W1049", "OPTION_TRUNCATED");
            AddOption("W1050", "WIDECHAR_REDUCED");
            AddOption("W1051", "DUPLICATES_IGNORED");
            AddOption("W1052", "UNIT_INIT_SEQ");
            AddOption("W1053", "LOCAL_PINVOKE");
            AddOption("W1054", "MESSAGE_DIRECTIVE");
            AddOption("W1055", "TYPEINFO_IMPLICITLY_ADDED");
            AddOption("W1056", "RLINK_WARNING");
            AddOption("W1057", "IMPLICIT_STRING_CAST");
            AddOption("W1058", "IMPLICIT_STRING_CAST_LOSS");
            AddOption("W1059", "EXPLICIT_STRING_CAST");
            AddOption("W1060", "EXPLICIT_STRING_CAST_LOSS");
            AddOption("W1061", "CVT_WCHAR_TO_ACHAR");
            AddOption("W1062", "CVT_NARROWING_STRING_LOST");
            AddOption("W1063", "CVT_ACHAR_TO_WCHAR");
            AddOption("W1064", "CVT_WIDENING_STRING_LOST");
            AddOption("W1065", "NON_PORTABLE_TYPECAST");
            AddOption("W1066", "LOST_EXTENDED_PRECISION");
            AddOption("W1067", "LNKDFM_NOTFOUND");
            AddOption("W1068", "IMMUTABLE_STRINGS");
            AddOption("W1069", "MOBILE_DELPHI");
            AddOption("W1070", "UNSAFE_VOID_POINTER");
            AddOption("W1201", "XML_WHITESPACE_NOT_ALLOWED");
            AddOption("W1202", "XML_UNKNOWN_ENTITY");
            AddOption("W1203", "XML_INVALID_NAME_START");
            AddOption("W1204", "XML_INVALID_NAME");
            AddOption("W1205", "XML_EXPECTED_CHARACTER");
            AddOption("W1206", "XML_CREF_NO_RESOLVE");
            AddOption("W1207", "XML_NO_PARM");
            AddOption("W1208", "XML_NO_MATCHING_PARM");
        }

        /// <summary>
        ///     clear options
        /// </summary>
        public void Clear() {
            foreach (var option in options.Values)
                option.ResetToDefault();
        }

        /// <summary>
        ///     reset options on a new unit
        /// </summary>
        public void ResetOnNewUnit() {
            foreach (var option in options.Values)
                option.ResetToDefault();
        }

        /// <summary>
        ///     add an option
        /// </summary>
        /// <param name="number"></param>
        /// <param name="ident"></param>
        private void AddOption(string number, string ident) => options.Add(new WarningKey(number, ident), new WarningOption(this, number, ident));

        /// <summary>
        ///     parent options
        /// </summary>
        public IWarningOptions ParentOptions { get; }

        /// <summary>
        ///     options
        /// </summary>
        private readonly IDictionary<WarningKey, WarningOption> options;

        /// <summary>
        ///     get the warning mode by key
        /// </summary>
        /// <param name="key">warning key</param>
        /// <returns>warning mode</returns>
        public WarningMode GetModeByKey(WarningKey key) {
            if (!options.TryGetValue(key, out var setting))
                return WarningMode.Undefined;
            return setting.Mode;
        }

        /// <summary>
        ///     check if a given identifier exists
        /// </summary>
        /// <param name="warningType"></param>
        /// <returns></returns>
        public bool HasWarningIdent(string warningType)
            => options.ContainsKey(new WarningKey("", warningType));

        /// <summary>
        ///     get warning mode by warning identifier
        /// </summary>
        /// <param name="ident"></param>
        /// <returns></returns>
        public WarningMode GetModeByIdentifier(string ident)
            => GetModeByKey(new WarningKey("", ident));

        /// <summary>
        ///     set a mode by key
        /// </summary>
        /// <param name="warningType">warning identifier</param>
        /// <param name="mode">warning mode</param>
        public void SetModeByIdentifier(string warningType, WarningMode mode) => SetModeByKey(new WarningKey("", warningType), mode);

        /// <summary>
        ///     set the warning mode by key
        /// </summary>
        /// <param name="key">warning key</param>
        /// <param name="mode">warning mode</param>
        public void SetModeByKey(WarningKey key, WarningMode mode) {
            if (options.TryGetValue(key, out var setting))
                setting.Mode = mode;

        }
    }
}
