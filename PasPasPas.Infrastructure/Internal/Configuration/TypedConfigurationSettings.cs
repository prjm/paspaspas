using System;
using System.Collections.Generic;

namespace PasPasPas.Infrastructure.Internal.Configuration {

    /// <summary>
    ///     configurationn entries
    /// </summary>
    internal abstract class ConfigurationEntry {

        /// <summary>
        ///     configuration name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        ///     configuration value as string
        /// </summary>
        public abstract string ValueAsString { get; set; }

        /// <summary>
        ///     compares two entries
        /// </summary>
        /// <param name="obj"></param>
        /// <returns><c>true</c> if the entries have the same name</returns>
        public override bool Equals(object obj) {
            var other = obj as ConfigurationEntry;
            if (other != null) {
                return string.Equals(Name, other.Name, StringComparison.OrdinalIgnoreCase);
            }
            else {
                return base.Equals(obj);
            }
        }

        public override int GetHashCode() {
            if (!string.IsNullOrEmpty(Name))
                return Name.GetHashCode();
            else
                return 0;
        }
    }

    /// <summary>
    ///     string setting value
    /// </summary>
    internal class StringConfigurationEntry : ConfigurationEntry {

        /// <summary>
        ///     setting value
        /// </summary>
        public override string ValueAsString { get; set; }
    }

    /// <summary>
    ///     settings implementation
    /// </summary>
    public class TypedConfigurationSettings : IConfigurationSettings {

        private Dictionary<string, ConfigurationEntry> entries
            = new Dictionary<string, ConfigurationEntry>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     get a configuration value
        /// </summary>
        /// <param name="settingName">setting name</param>
        /// <param name="defaultValue">default value</param>
        /// <returns></returns>
        public string GetValue(string settingName, string defaultValue) {
            var result = defaultValue;
            ConfigurationEntry entry;

            if (entries.TryGetValue(settingName, out entry)) {
                result = entry.ValueAsString;
            }

            return result;
        }

        /// <summary>
        ///     set string value
        /// </summary>
        /// <param name="settingName">setting name</param>
        /// <param name="settingValue">setting value</param>
        /// <returns></returns>
        public string SetValue(string settingName, string settingValue) {
            string result = null;
            ConfigurationEntry entry;
            ConfigurationEntry newEntry = new StringConfigurationEntry() {
                Name = settingName,
                ValueAsString = settingValue
            };

            if (entries.TryGetValue(settingName, out entry)) {
                result = entry.ValueAsString;
            }

            entries[settingName] = newEntry;
            return result;
        }
    }
}
