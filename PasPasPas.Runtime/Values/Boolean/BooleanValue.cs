using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Global.Constants;

namespace PasPasPas.Runtime.Values.Boolean {

    /// <summary>
    ///     boolean value
    /// </summary>
    public class BooleanValue : BooleanValueBase {

        private readonly bool value;

        /// <summary>
        ///     create a new boolean value
        /// </summary>
        /// <param name="aValue"></param>
        public BooleanValue(bool aValue)
            => value = aValue;

        /// <summary>
        ///     type id
        /// </summary>
        public override int TypeId
            => KnownTypeIds.BooleanType;

        /// <summary>
        ///     get the boolean value
        /// </summary>
        public override bool AsBoolean { get; }

        /// <summary>
        ///     format this boolean as string
        /// </summary>
        /// <returns></returns>
        public override string ToString()
            => value.ToString();
    }
}
