using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using PasPasPas.Infrastructure.Utils;

namespace SampleRunner {

    /// <summary>
    ///     html histogram value
    /// </summary>
    public class HtmlHistogramValue {

        /// <summary>
        ///     value
        /// </summary>
        public long Value { get; internal set; }

        /// <summary>
        ///     key
        /// </summary>
        public string Key { get; internal set; }

        /// <summary>
        ///     formatted key
        /// </summary>
        public object FormattedKey
            => System.Security.SecurityElement.Escape(Key);
    }

    public class HtmlHistogram {

        private readonly long max;

        public int Id { get; }

        private readonly long min;
        private readonly IList<HtmlHistogramValue> values
            = new List<HtmlHistogramValue>();

        public HtmlHistogram(Histogram histogram, int id) {
            Key = histogram.Key;
            min = histogram.MinValue;
            max = histogram.MaxValue;
            Id = id;
        }

        public string Key { get; }
        public object FormattedKey
            => System.Security.SecurityElement.Escape(Key);

        internal void AddValue(string key, long value)
            => values.Add(new HtmlHistogramValue() { Key = key, Value = value });

        internal void Render(StreamWriter writer) {
            writer.WriteLine($"<h2><a name=\"{Id}\">{FormattedKey}</a></h2>");
            writer.WriteLine("<table width=\"800\" style=\"font-size: 8pt;\">");

            var factor = 500.0 / (max - min);

            foreach (var value in values) {

                if (value.Value == min)
                    continue;

                var width = (int)System.Math.Round((value.Value - min) * factor);
                writer.WriteLine("<tr>");
                writer.WriteLine($"<td><div style=\"width: 200px; height: 40px; overflow: auto\">{value.FormattedKey}</div></td>");
                writer.WriteLine($"<td><div style=\"background-color: #336699; height: 15px; width: {width}px\"></div></td>");
                writer.WriteLine($"<td>{value.Value.ToString(CultureInfo.CurrentCulture)}</td>");
                writer.WriteLine("</tr>");
            }

            writer.WriteLine("</table>");
            writer.WriteLine("<hr/>");
        }
    }

    public class HtmlHistogramPrinter : IHistogramPrinter {

        private readonly IList<HtmlHistogram> histograms
            = new List<HtmlHistogram>();

        public void PrintHistogram(Histogram histogram)
            => histograms.Add(new HtmlHistogram(histogram, histograms.Count));

        public void PrintValue(Histogram histogram, string key, long value) {
            var item = histograms.First(t => string.Equals(histogram.Key, t.Key, StringComparison.OrdinalIgnoreCase));
            item.AddValue(key, value);
        }

        internal void Render(string path) {
            using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
            using (var writer = new StreamWriter(fileStream, Encoding.UTF8)) {
                writer.WriteLine("<html><head><title>Histograms</title></head><body>");
                writer.WriteLine("<h2>Histograms</h2><p><ul>");
                foreach (var histogram in histograms) {
                    writer.WriteLine($"<li><a href=\"#{histogram.Id}\">{histogram.FormattedKey}</a></li>");
                }
                writer.WriteLine("</ul></p>");

                foreach (var histogram in histograms)
                    histogram.Render(writer);

                writer.WriteLine("</body></html>");
                writer.Close();
            }
        }
    }
}
