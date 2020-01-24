using System.Numerics;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     interface for subrange types
    /// </summary>
    public interface ISubrangeType : IOrdinalType {

        /// <summary>
        ///     base type definition
        /// </summary>
        IOrdinalType SubrangeOfType { get; }

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
