using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     part of a generic ame
    /// </summary>
    public class GenericNameFragment : AbstractSyntaxPart, ITypeTarget {

        /// <summary>
        ///     symbol name
        /// </summary>
        public SymbolName Name { get; set; }

        /// <summary>
        ///     symbol type
        /// </summary>
        public IList<ITypeSpecification> TypeValues { get; }
            = new List<ITypeSpecification>();

        /// <summary>
        ///     children
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts
        {
            get
            {
                foreach (ITypeSpecification value in TypeValues)
                    yield return value;
            }
        }

        /// <summary>
        ///     type value
        /// </summary>
        public ITypeSpecification TypeValue
        {
            get
            {
                if (TypeValues.Count > 0)
                    return TypeValues[TypeValues.Count - 1];
                else
                    return null;
            }

            set
            {
                TypeValues.Add(value);
            }
        }
    }
}
