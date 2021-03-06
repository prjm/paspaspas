﻿#nullable disable
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Options;

namespace PasPasPas.Options.DataTypes {

    /// <summary>
    ///     path resolver for file includes
    /// </summary>
    public class IncludeFilePathResolver : SearchPathResolver {

        /// <summary>
        ///     create a new include file pat resolve
        /// </summary>
        /// <param name="options"></param>
        public IncludeFilePathResolver(IOptionSet options)
            : base(options) {
        }

        /// <summary>
        ///     resolve a path
        /// </summary>
        /// <param name="basePath"></param>
        /// <param name="pathToResolve"></param>
        /// <returns></returns>
        protected override ResolvedFile DoResolvePath(IFileReference basePath, IFileReference pathToResolve)
            => ResolveFromSearchPath(basePath, pathToResolve);

    }
}