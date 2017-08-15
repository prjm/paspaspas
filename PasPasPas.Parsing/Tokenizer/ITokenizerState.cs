using System;

namespace PasPasPas.Parsing.Tokenizer {
    public interface ITokenizerState {


        bool AtEof { get; }
        char NextChar(bool append);
        char PreviousChar();
        int CurrentPosition { get; }
        char CurrentCharacter { get; }

        void Clear();
        void StartBufferWith(char startValue);
        void Append(char currentChar);
        char GetBufferCharAt(int index);
        string GetBufferContent();
        bool BufferEndsWith(char endSequence);
        bool BufferEndsWith(string endSequence);
        int Length { get; set; }

        void Error(Guid errorId);
        bool KeepTokenValue(int tokenId);

        bool PrepareNextToken();
    }
}