﻿using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Hidden {

    /// <summary>
    ///     create a new void type
    /// </summary>
    public class VoidType : HiddenIntrinsicType {

        /// <summary>
        ///     create a new void type
        /// </summary>
        /// <param name="definingUnit"></param>
        public VoidType(IUnitType definingUnit) : base(definingUnit) {
        }
    }
}
