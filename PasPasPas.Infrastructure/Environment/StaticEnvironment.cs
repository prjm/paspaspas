using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Infrastructure.Environment {

    /// <summary>
    ///     static dependency manager
    /// </summary>
    public class StaticEnvironment {

        /// <summary>
        ///     internal class to manage entries
        /// </summary>
        private class EnvironmentEntry {
            public Guid id;
            public Lazy<object> creator;
            public int usage = 0;
        }

        private static ConcurrentDictionary<Guid, EnvironmentEntry> environment
            = new ConcurrentDictionary<Guid, EnvironmentEntry>();

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
        public static bool Register(Guid id, Func<object> data) {
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
        public static T Require<T>(ref Guid id) where T : class {
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
        public static T Optional<T>(Guid id) where T : class {
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
