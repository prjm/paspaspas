using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     a set of static dependencies
    /// </summary>
    public static class StaticDependency {

        /// <summary>
        ///     undefined dependency
        /// </summary>
        public const int Undefined = 0;

        /// <summary>
        ///     string builder pool
        /// </summary>
        public const int StringBuilderPool = 1;

        /// <summary>
        ///     integer parser
        /// </summary>
        public const int ParsedIntegers = 2;

        /// <summary>
        ///     hex number parser
        /// </summary>
        public const int ParsedHexNumbers = 3;

        /// <summary>
        ///     char literal converter
        /// </summary>
        public const int ConvertedCharLiterals = 4;

        /// <summary>
        ///     real literal converter
        /// </summary>
        public const int ConvertedRealLiterals = 5;

        /// <summary>
        ///     token sequence pool
        /// </summary>
        public const int TokenSequencePool = 6;
    }

    /// <summary>
    ///     static dependency manager
    /// </summary>
    public class StaticEnvironment {

        /// <summary>
        ///     internal class to manage entries
        /// </summary>
        private class EnvironmentEntry {
            public int id;
            public Lazy<object> creator;
            public int usage = 0;
        }

        private static ConcurrentDictionary<int, EnvironmentEntry> environment
            = new ConcurrentDictionary<int, EnvironmentEntry>();

        /// <summary>
        ///     get a list of static entries
        /// </summary>
        public static IEnumerable<object> Entries
            => environment.Values.Where(t => t.creator.IsValueCreated).Select(t => t.creator.Value).ToList();

        /// <summary>
        ///     clears registry
        /// </summary>
        public static void Clear()
            => environment.Clear();

        /// <summary>
        ///     register a cached lookup function
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public static bool Register(int id, Func<object> data) {
            if (environment.ContainsKey(id))
                return false;

            return environment.TryAdd(id, new EnvironmentEntry() {
                id = id,
                creator = new Lazy<object>(data, true)
            });
        }


        /// <summary>
        ///     require a static dependency
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public static T Require<T>(int id) where T : class {
            var value = Optional<T>(id);

            if (value == null) {
                ExceptionHelper.InvalidOperation();
                return null;
            }

            return value;
        }

        /// <summary>
        ///     resolve an optional static dependency
        /// </summary>
        /// <typeparam name="T">result type</typeparam>
        /// <param name="id">entry id</param>
        /// <returns>optional value</returns>
        public static T Optional<T>(int id) where T : class {
            if (environment.TryGetValue(id, out var entry)) {
                if (entry.creator.Value is T result) {
                    Interlocked.Increment(ref entry.usage);
                    return result;
                }
            }
            return null;
        }


    }
}
