using System.Collections.Generic;
using PasPasPas.Infrastructure.Input;
using PasPasPas.Parsing.SyntaxTree;

namespace PasPasPas.Parsing.Tokenizer {

    /// <summary>
    ///     line counters
    /// </summary>
    public class LineCounters {

        /// <summary>
        ///     line counters
        /// </summary>
        private Dictionary<IFileReference, LineCounter> counters
            = new Dictionary<IFileReference, LineCounter>();

        /// <summary>
        ///     processs token
        /// </summary>
        /// <param name="token">token to process</param>
        public void ProcessToken(Token token) {
            if (token == null)
                return;

            var path = token.FilePath;
            LineCounter counter = null;

            if (!counters.TryGetValue(path, out counter)) {
                counter = new LineCounter();
                counters.Add(path, counter);
            }

            ProcessToken(token, counter);
        }

        private static void ProcessToken(Token token, LineCounter counter) {
            token.StartPosition = new TextFilePosition(counter.Line, counter.Column);

            for (int index = 0; index < token.Value.Length; index++) {
                counter.ProcessChar(token.Value[index]);
            }

            token.EndPosition = new TextFilePosition(counter.Line, counter.Column);
        }
    }
}
