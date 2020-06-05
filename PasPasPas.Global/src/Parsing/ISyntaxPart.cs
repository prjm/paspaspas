#nullable disable
namespace PasPasPas.Globals.Parsing {

    /// <summary>
    ///     common interface for syntax tree elements
    /// </summary>
    public interface ISyntaxPart {

        /// <summary>
        ///     length (number of characters) of a syntax item
        /// </summary>
        int Length { get; }

        /// <summary>
        ///     accept a visitor object
        /// </summary>
        /// <param name="visitor">visitor</param>
        void Accept(IStartEndVisitor visitor);

    }


    /// <summary>
    ///     helper for syntax parts
    /// </summary>
    public static class SyntaxPartHelper {

        /// <summary>
        ///     get the symbol length
        /// </summary>
        /// <param name="part">part length</param>
        /// <returns>length or <c>0</c></returns>
        public static int GetSymbolLength(this ISyntaxPart part)
            => part == null ? 0 : part.Length;

    }

}