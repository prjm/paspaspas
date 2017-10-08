using System;
using System.Linq;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using PasPasPas.Infrastructure.Files;

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

        /// <summary>
        ///     char string pool
        /// </summary>
        public const int CharStringPool = 7;

        /// <summary>
        ///     string pool
        /// </summary>
        public const int StringPool = 8;

        /// <summary>
        ///     tokenizer pattern for standard syntax
        /// </summary>
        public const int TokenizerPatternFactory = 9;

        /// <summary>
        ///     file access
        /// </summary>
        public const int FileAccess = 10;

        /// <summary>
        ///     log manager
        /// </summary>
        public const int LogManager = 11;


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
            public object item;
            public int usage = 0;
        }

        private ConcurrentDictionary<int, EnvironmentEntry> environment
            = new ConcurrentDictionary<int, EnvironmentEntry>();

        /// <summary>
        ///     get a list of static entries
        /// </summary>
        public IEnumerable<object> Entries
            => environment.Values.Select(t => t.item).ToList();

        /// <summary>
        ///     clears registry
        /// </summary>
        public void Clear() {
            var keysToRemove = new List<int>();

            foreach (var entry in environment.Values) {
                if (entry.item is IManualStaticCache cache)
                    cache.Clear();
                else
                    keysToRemove.Add(entry.id);
            }

            foreach (var id in keysToRemove)
                environment.TryRemove(id, out var _);
        }

        /// <summary>
        ///     register a cached lookup function
        /// </summary>
        /// <param name="id"></param>
        /// <param name="data"></param>
        public bool Register(int id, object data) {
            if (environment.ContainsKey(id))
                return false;

            return environment.TryAdd(id, new EnvironmentEntry() {
                id = id,
                item = data
            });
        }


        /// <summary>
        ///     require a static dependency
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T Require<T>(int id) where T : class
            => Optional<T>(id) ?? throw new InvalidOperationException($"Unregistered id {id}");

        /// <summary>
        ///     resolve an optional static dependency
        /// </summary>
        /// <typeparam name="T">result type</typeparam>
        /// <param name="id">entry id</param>
        /// <returns>optional value</returns>
        public T Optional<T>(int id) where T : class {
            if (environment.TryGetValue(id, out var entry)) {
                if (entry.item is T result) {
                    Interlocked.Increment(ref entry.usage);
                    return result;
                }
            }
            return null;
        }

        /// <summary>
        ///     provide a manual instance
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <param name="item"></param>
        public void Provide<T>(int id, T item) {
            var entry = new EnvironmentEntry {
                id = id,
                item = item
            };
            environment.TryAdd(id, entry);
        }

    }
}
