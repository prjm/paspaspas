using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     helpers for type kind
    /// </summary>
    public static class TypeKindHelpers {

        /// <summary>
        ///     match all type kinds
        /// </summary>
        /// <param name="kind">kind</param>
        /// <param name="type1">first type kind to be compared</param>
        /// <param name="type2">second type kind to be compared</param>
        /// <returns></returns>
        public static bool All(this CommonTypeKind kind, CommonTypeKind type1, CommonTypeKind type2)
            => kind == type1 && kind == type2;

        /// <summary>
        ///     match one type kind
        /// </summary>
        /// <param name="kind">kind</param>
        /// <param name="type1">first type kind to be compared</param>
        /// <param name="type2">second type kind to be compared</param>
        /// <returns></returns>
        public static bool One(this CommonTypeKind kind, CommonTypeKind type1, CommonTypeKind type2)
            => kind == type1 || kind == type2;


        /// <summary>
        ///     test if the type kind is numerical
        /// </summary>
        /// <param name="kind">kind</param>
        /// <returns><c>true</c> if the type is integer, int64 or float</returns>
        public static bool Numerical(this CommonTypeKind kind)
            => kind == CommonTypeKind.IntegerType || kind == CommonTypeKind.FloatType || kind == CommonTypeKind.Int64Type;

        /// <summary>
        ///     test if the type kind is integral
        /// </summary>
        /// <param name="kind">kind</param>
        /// <returns><c>true</c> if the type is integer, int64 or float</returns>
        public static bool Integral(this CommonTypeKind kind)
            => kind == CommonTypeKind.IntegerType || kind == CommonTypeKind.Int64Type;


    }
}
