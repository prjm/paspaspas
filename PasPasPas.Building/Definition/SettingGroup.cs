using System.Linq;
using System.Collections.Generic;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Infrastructure.Files;

namespace PasPasPas.Building.Definition {

    /// <summary>
    ///    settings group
    /// </summary>
    public class SettingGroup {

        /// <summary>
        ///     settings
        /// </summary>
        public IList<Setting> Items { get; }
            = new List<Setting>();

        /// <summary>
        ///     clear settings
        /// </summary>
        public void Clear() {
            foreach (var item in Items.OfType<IClearableSetting>())
                item.Clear();
        }

        /// <summary>
        ///     resolve settings
        /// </summary>
        /// <param name="variables"></param>
        public void Resolve(Dictionary<string, Setting> variables) {
            foreach (var item in Items.OfType<IResolvableSetting>())
                item.Resolve(variables);
        }

        /// <summary>
        ///     get file references from this setting
        /// </summary>
        /// <returns>file references</returns>
        public IList<IFileReference> AsFileList(IFileAccess fileAccess) {
            var result = new List<IFileReference>();

            foreach (var item in Items) {
                var reference = item as IResolvableSetting;
                Setting setting;

                if (reference == null)
                    setting = item;
                else
                    setting = reference.ResolvedItem;


                var fileBasedSettings = setting as IFileReferenceSetting;
                foreach (var path in fileBasedSettings.GetReferencedFiles(fileAccess))
                    result.Add(path);

            }

            return result;
        }
    }
}