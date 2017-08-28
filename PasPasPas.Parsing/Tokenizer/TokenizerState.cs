using System;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Infrastructure.Files;
using System.Text;
using PasPasPas.Infrastructure.Utils;

namespace PasPasPas.Parsing.Tokenizer {


    public class TokenizerState {

        private TokenizerBase tokenizer;

        private readonly StringBuilder buffer
            = new StringBuilder(100);

        private readonly StackedFileReader input;

        private readonly ILogSource log;

        /// <summary>
        ///     create a new tokenizer state
        /// </summary>
        /// <param name="tokenizer"></param>
        internal TokenizerState(TokenizerBase tokenizer, StackedFileReader input, ILogSource log) {
            this.tokenizer = tokenizer;
            this.log = log;
            this.input = input;
        }

        public int Length {
            get => buffer.Length;
            set => buffer.Length = value;
        }

        /// <summary>
        ///     check if the end of the current input file is reached
        /// </summary>
        public bool AtEof
            => input == null || input.CurrentFile.AtEof;

        /// <summary>
        ///     get the current value
        /// </summary>
        public char CurrentCharacter
            => input.Value;

        /// <summary>
        ///     get the current position
        /// </summary>
        public int CurrentPosition
            => input.Position;

        public void Append(char currentChar)
            => buffer.Append(currentChar);

        public bool BufferEndsWith(char endSequence)
            => buffer.EndsWith(endSequence);

        public bool BufferEndsWith(string endSequence)
            => buffer.EndsWith(endSequence);

        public void Clear()
            => buffer.Clear();

        public void Error(Guid errorId)
            => log.Error(errorId);

        public char GetBufferCharAt(int index)
            => buffer[index];

        public string GetBufferContent()
            => buffer.ToString();

        public bool KeepTokenValue(int tokenId)
            => tokenizer.KeepTokenValue(tokenId);

        public char NextChar(bool append) {
            var result = input.NextChar();
            if (append)
                buffer.Append(result);
            return result;
        }

        public bool PrepareNextToken() {
            var file = input.CurrentFile;

            while (file != null && file.AtEof)
                file = input.FinishCurrentFile();

            return file != null;
        }


        public char PreviousChar()
            => input.PreviousChar();

        public void StartBufferWith(char startValue) {
            buffer.Clear();
            buffer.Append(startValue);
        }
    }

}