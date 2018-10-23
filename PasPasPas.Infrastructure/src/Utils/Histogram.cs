using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     histogram base class
    /// </summary>
    public abstract class Histogram {

        /// <summary>
        ///     histogram id
        /// </summary>
        private readonly string key;

        /// <summary>
        ///     create a new histogram
        /// </summary>
        /// <param name="key"></param>
        public Histogram(string key)
            => this.key = key;

        /// <summary>
        ///     histogram key
        /// </summary>
        public string Key
            => key;

        /// <summary>
        ///     histogram length
        /// </summary>
        protected abstract int Length { get; }

        /// <summary>
        ///     minimum
        /// </summary>
        protected abstract long MinValue { get; }

        /// <summary>
        ///     maximum
        /// </summary>
        protected abstract long MaxValue { get; }

        /// <summary>
        ///     data
        /// </summary>
        protected abstract IEnumerable<KeyValuePair<string, long>> Data { get; }

        /// <summary>
        ///     print this histogram
        /// </summary>
        /// <param name="result"></param>
        public void Print(TextWriter result) {

            if (Length < 1)
                return;

            var max = MaxValue;
            var min = MinValue;

            if (max == min)
                return;

            var unit = 30.0 / (max - min);
            result.WriteLine(key);
            result.WriteLine(new string('-', 80));
            result.WriteLine();

            foreach (var value in Data.OrderBy(t => -t.Value)) {

                if (value.Value < max / 10)
                    break;

                var offset = (int)(unit * (value.Value - min));
                var key = value.Key;
                key = key.Replace('\n', ' ');
                key = key.Replace('\t', ' ');
                key = key.Replace('\r', ' ');
                key = key.Trim();

                if (key.Length < 20) {
                    result.Write(key);
                    result.Write(new string(' ', 20 - key.Length));
                }
                else {
                    result.Write(key.Substring(0, 20));
                }

                result.Write(new string('X', offset));
                if (offset < 30)
                    result.Write(new string(' ', 30 - offset));
                result.Write("    ");
                result.WriteLine(value.Value);
            }

            result.WriteLine();
            result.WriteLine();
        }
    }

    /// <summary>
    ///     histogram for debugging purposes
    /// </summary>
    public class Histogram<T> : Histogram {

        private readonly IDictionary<T, long> values
            = new Dictionary<T, long>();


        /// <summary>
        ///     create a new histogram
        /// </summary>
        /// <param name="key"></param>
        public Histogram(string key) : base(key) { }

        /// <summary>
        ///     minimum value
        /// </summary>
        protected override long MinValue
            => values.Values.Min();

        /// <summary>
        ///     maximum value
        /// </summary>
        protected override long MaxValue
            => values.Values.Max();

        /// <summary>
        ///     get histogram data
        /// </summary>
        protected override IEnumerable<KeyValuePair<string, long>> Data {
            get {
                foreach (var item in values)
                    yield return new KeyValuePair<string, long>(item.Key.ToString(), item.Value);
            }
        }

        /// <summary>
        ///     length
        /// </summary>
        protected override int Length
            => values.Count;

        /// <summary>
        ///     Register a value
        /// </summary>
        /// <param name="value"></param>
        public void RegisterValue(T value) {
            lock (values) {
                if (!values.TryGetValue(value, out var count))
                    count = 0;
                values[value] = 1 + count;
            }
        }

    }

    /// <summary>
    ///     global histogram keys
    /// </summary>
    public static class HistogramKeys {

        /// <summary>
        ///     syntax nodes histograms
        /// </summary>
        public const string SyntaxNodes = "SyntaxNodes";

    }

    /// <summary>
    ///     histograms
    /// </summary>
    public class Histograms {

        private readonly IDictionary<string, Histogram> data
            = new Dictionary<string, Histogram>();

        /// <summary>
        ///     single instance
        /// </summary>
        private static readonly Lazy<Histograms> instance
            = new Lazy<Histograms>(true);

        /// <summary>
        ///     enable histograms
        /// </summary>
        public static bool Enable { get; set; }

        /// <summary>
        ///     register a value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        private void RegisterValue<T>(string key, T value) {
            lock (data) {
                if (!data.TryGetValue(key, out var histogram)) {
                    histogram = new Histogram<T>(key);
                    data[key] = histogram;
                }
                ((Histogram<T>)histogram).RegisterValue(value);
            }
        }

        /// <summary>
        ///     register a value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="value"></param>
        [Conditional("DEBUG")]
        public static void Value<T>(string key, T value) {
            if (Enable)
                instance.Value.RegisterValue(key, value);
        }

        /// <summary>
        ///     print all histograms
        /// </summary>
        /// <param name="result"></param>
        public static void Print(TextWriter result) {
            var items = new List<Histogram>(instance.Value.data.Values);
            items.Sort((l, r) => string.Compare(l.Key, r.Key, StringComparison.Ordinal));

            foreach (var item in items)
                item.Print(result);
        }
    }

}
