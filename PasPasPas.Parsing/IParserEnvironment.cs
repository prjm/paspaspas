using System.Collections.Generic;
using PasPasPas.Infrastructure.Environment;
using PasPasPas.Parsing.Tokenizer.LiteralValues;
using PasPasPas.Parsing.Tokenizer.Patterns;

namespace PasPasPas.Parsing {

    /// <summary>
    ///     parser environment
    /// </summary>
    public interface IParserEnvironment : IBasicEnvironment {
        ILiteralParser IntegerParser { get; }
        ILiteralParser HexNumberParser { get; }
        ICharLiteralConverter CharLiteralConverter { get; }
        IRealConverter RealLiteralConverter { get; }
        ObjectPool<Tokenizer.TokenizerWithLookahead.TokenSequence> TokenSequencePool { get; }
        PatternFactory Patterns { get; }
        ILiteralUnwrapper LiteralUnwrapper { get; }
    }
}
