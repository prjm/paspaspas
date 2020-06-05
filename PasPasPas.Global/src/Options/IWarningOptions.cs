#nullable disable
using PasPasPas.Globals.Options.DataTypes;

namespace PasPasPas.Globals.Options {

    /// <summary>
    ///     warning options
    /// </summary>
    public interface IWarningOptions {

        /// <summary>
        ///     check if a warning is enabled
        /// </summary>
        /// <param name="warningType"></param>
        /// <returns></returns>
        bool HasWarningIdent(string warningType);

        /// <summary>
        ///     set warning mode by identifier
        /// </summary>
        /// <param name="warningType"></param>
        /// <param name="mode"></param>
        void SetModeByIdentifier(string warningType, WarningMode mode);

        /// <summary>
        ///     clear options
        /// </summary>
        void Clear();

        /// <summary>
        ///     clear options on a new unit
        /// </summary>
        void ResetOnNewUnit();

        /// <summary>
        ///     get the warning mode by an identifier
        /// </summary>
        /// <param name="identifier"></param>
        /// <returns></returns>
        WarningMode GetModeByIdentifier(string identifier);

        /// <summary>
        ///     get warning mode by key
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        WarningMode GetModeByKey(WarningKey key);
    }
}