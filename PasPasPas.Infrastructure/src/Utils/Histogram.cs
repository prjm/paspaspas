using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    ///     interface for histogram printers
    /// </summary>
    public interface IHistogramPrinter {

        /// <summary>
        ///     start printing a histogram
        /// </summary>
        /// <param name="histogram"></param>
        void PrintHistogram(Histogram histogram);

        /// <summary>
        ///     print a histogram value
        /// </summary>
        /// <param name="histogram"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        void PrintValue(Histogram histogram, string key, long value);
    }

    /// <summary>
    ///     histogram base class
    /// </summary>
    public abstract class Histogram {

        /// <summary>
        ///     create a new histogram
        /// </summary>
        /// <param name="key"></param>
        public Histogram(string key)
            => Key = key;

        /// <summary>
        ///     histogram key
        /// </summary>
        public string Key { get; }

        /// <summary>
        ///     histogram length
        /// </summary>
        public abstract int Length { get; }

        /// <summary>
        ///     minimum
        /// </summary>
        public abstract long MinValue { get; }

        /// <summary>
        ///     maximum
        /// </summary>
        public abstract long MaxValue { get; }

        /// <summary>
        ///     data
        /// </summary>
        protected abstract IEnumerable<KeyValuePair<string, long>> Data { get; }

        /// <summary>
        ///     print this histogram
        /// </summary>
        /// <param name="result"></param>
        public void Print(IHistogramPrinter result) {
            result.PrintHistogram(this);

            foreach (var value in Data.OrderBy(t => -t.Value))
                result.PrintValue(this, value.Key, value.Value);
        }

        /// <summary>
        ///     data point count
        /// </summary>
        public abstract int Count { get; }

    }

    /// <summary>
    ///     histogram for debugging purposes
    /// </summary>
    public class Histogram<T> : Histogram {

        private readonly IDictionary<T, long> values
            = new Dictionary<T, long>();

        /// <summary>
        ///     count number of items
        /// </summary>
        public override int Count
            => values.Count;


        /// <summary>
        ///     create a new histogram
        /// </summary>
        /// <param name="key"></param>
        public Histogram(string key) : base(key) { }

        /// <summary>
        ///     minimum value
        /// </summary>
        public override long MinValue
            => values.Values.Min();

        /// <summary>
        ///     maximum value
        /// </summary>
        public override long MaxValue
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
        public override int Length
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
        ///     syntax nodes histogram
        /// </summary>
        public const string SyntaxNodes = "SyntaxNodes";

        /// <summary>
        ///     syntax list nodes histogram
        /// </summary>
        public const string SyntaxLists = "SyntaxLists";

        /// <summary>
        ///     designator items histogram
        /// </summary>
        public const string DesignatorItems = "DesignatorItems";

        /// <summary>
        ///     identifier pool values
        /// </summary>
        public const string IdentifierPoolValues = "IdentifierPoolValues";

        /// <summary>
        ///     string pool values
        /// </summary>
        public const string StringPoolValues = "StringPoolValues";

        /// <summary>
        ///     terminal pool values
        /// </summary>
        public const string TerminalPoolValues = "TerminalPoolValue";

        /// <summary>
        ///     token array values
        /// </summary>
        public const string TokenArrayValues = "TokenArrayValues";

    }

    /// <summary>
    ///     histograms
    /// </summary>
    public class Histograms {

        private readonly IDictionary<string, Histogram> data
            = new Dictionary<string, Histogram>();

        /// <summary>
        ///     histograms instance
        /// </summary>
        public static Histograms Instance
            => instance.Value;

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
        ///     sum count
        /// </summary>
        public int Count
            => data.Values.Sum(t => t.Count);

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
        public static void Print(IHistogramPrinter result) {
            var items = new List<Histogram>(instance.Value.data.Values);
            items.Sort((l, r) => string.Compare(l.Key, r.Key, StringComparison.Ordinal));

            foreach (var item in items)
                item.Print(result);
        }
    }

}