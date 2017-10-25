using PasPasPas.Parsing.SyntaxTree.Abstract;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     basic type definition
    /// </summary>
    public class TypeBase : ITypeDefinition {

        private int typeId;

        /// <summary>
        ///     create a new type definiton
        /// </summary>
        /// <param name="withId">type id</param>
        public TypeBase(int withId)
            => typeId = withId;

        /// <summary>
        ///     get the type id
        /// </summary>
        public int TypeId => typeId;

    }
}
