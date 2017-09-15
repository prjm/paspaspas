using System;
using System.Collections.Generic;
using System.Linq;

namespace PasPasPas.Infrastructure.Utils {

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Comparer2<T> : Comparer<T> {
        private readonly Comparison<T> _compareFunction;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="comparison"></param>
        public Comparer2(Comparison<T> comparison)
            => _compareFunction = comparison ?? throw new ArgumentNullException("comparison");

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg1"></param>
        /// <param name="arg2"></param>
        /// <returns></returns>
        public override int Compare(T arg1, T arg2)
            => _compareFunction(arg1, arg2);
    }
}