using System.Collections.Generic;

namespace PasPasPas.Building.Definition {

    /// <summary>
    ///     reference to a setting
    /// </summary>
    public class SettingReference : Setting, IResolvableSetting, IClearableSetting {

        /// <summary>
        ///     name of the referenced setting
        /// </summary>
        public string ReferenceName { get; set; }

        /// <summary>
        ///     resolved setting
        /// </summary>
        public Setting ResolvedItem { get; set; }


        /// <summary>
        ///     clear the resolved setting
        /// </summary>
        public void Clear() => ResolvedItem = null;

        /// <summary>
        ///     resolve the referenced setting
        /// </summary>
        /// <param name="settings"></param>
        public void Resolve(IDictionary<string, Setting> settings) {
            Setting resolvedItem;
            if (settings.TryGetValue(ReferenceName, out resolvedItem))
                ResolvedItem = resolvedItem;
        }
    }
}
