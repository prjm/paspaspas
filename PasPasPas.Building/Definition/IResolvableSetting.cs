using System.Collections.Generic;

namespace PasPasPas.Building.Definition {

    /// <summary>
    ///     resolvable setting
    /// </summary>
    public interface IResolvableSetting {

        /// <summary>
        ///     resolved item
        /// </summary>
        Setting ResolvedItem { get; set; }

        /// <summary>
        ///     resolve a setting
        /// </summary>
        /// <param name="settings"></param>
        void Resolve(IDictionary<string, Setting> settings);
    }
}
