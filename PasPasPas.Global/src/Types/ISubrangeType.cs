using System.Numerics;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for subrange types
    /// </summary>
    public interface ISubrangeType : IOrdinalType {

        /// <summary>
        ///     base type id
        /// </summary>
        int BaseTypeId { get; }

        /// <summary>
        ///     base type definition
        /// </summary>
        IOrdinalType BaseType { get; }

        /// <summary>
        ///     test the validity of this subrange type
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        ///     cardinality
        /// </summary>
        BigInteger Cardinality { get; }
    }
}
