using System;
using System.Collections.Generic;
using System.IO;
using PasPasPas.Api;
using PasPasPas.Globals.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;

namespace PasPasPasTests.Common {
    public class FilesAndPaths {

        private readonly Dictionary<string, string> data
            = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        ///     create a new set of files and paths
        /// </summary>
        /// <param name="p"></param>
        public FilesAndPaths(params (string name, string content)[] p) {
            foreach (var (name, content) in p) {
                Add(name, content);
            }
        }

        public IInputResolver CreateResolver(IInputResolver inputResolver = default) {
            IReaderInput doResolve(IFileReference f, IReaderApi a) {
                if (inputResolver != default) {
                    var value = inputResolver.Resolve(a, f);
                    if (value != default)
                        return value;
                }

                if (data.TryGetValue(f.Path, out var content))
                    return a.CreateInputForString(f, content);
                return default;
            }

            bool doCheck(IFileReference f) {
                if (inputResolver != default && inputResolver.CanResolve(f))
                    return true;
                return data.ContainsKey(f.Path);
            }

            return Factory.CreateInputResolver(doResolve, doCheck);
        }

        internal void Add(string fileName, string content) {
            var currentDirAndFile = Path.Combine(Directory.GetCurrentDirectory(), fileName);
            data.Add(currentDirAndFile, content);
        }

        internal IFileReference FindUnit(IEnvironment env, string v) {
            foreach (var f in data.Keys) {
                var fn = Path.GetFileName(f);
                if (string.Equals(v, fn, StringComparison.OrdinalIgnoreCase))
                    return env.CreateFileReference(f);
            }

            return env.CreateFileReference(v);
        }
    }
}
