using System;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     flags to modify symbol resolve behavior
    /// </summary>
    [Flags()]
    public enum ResolverFlags {

        /// <summary>
        ///     unknown flags
        /// </summary>
        None = 0,

        /// <summary>
        ///     skip private class members
        /// </summary>
        SkipPrivate = 1,

        /// <summary>
        ///     resolve from another unit
        /// </summary>
        FromAnotherUnit = 2,

    }

    /// <summary>
    ///     helper functions for resolver flags
    /// </summary>
    public static class ResolverFlagsHelper {

        /// <summary>
        ///     check if private flags have to be skipped
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool MustSkipPrivate(this ResolverFlags flags)
            => (flags & ResolverFlags.SkipPrivate) == ResolverFlags.SkipPrivate;

        /// <summary>
        ///     check if resolving from another unit
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static bool IsResolvingFromAnotherUnit(this ResolverFlags flags)
            => (flags & ResolverFlags.FromAnotherUnit) == ResolverFlags.FromAnotherUnit;

    }

}
