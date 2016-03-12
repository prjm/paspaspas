namespace PasPasPas.Infrastructure.Internal.Configuration {

    /// <summary>
    ///     configuration settings
    /// </summary>
    public interface IConfigurationSettings {

        /// <summary>
        ///     get string value
        /// </summary>
        /// <param name="settingName">setting name</param>
        /// <param name="defaultValue">setting value</param>
        /// <returns>string setting</returns>
        string GetValue(string settingName, string defaultValue);

        /// <summary>
        ///     set a string value
        /// </summary>
        /// <param name="settigName">setting name</param>
        /// <param name="settingValue">setting value</param>
        /// <returns>old value, if extisting</returns>
        string SetValue(string settigName, string settingValue);

    }
}
