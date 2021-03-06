﻿using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Hidden {

    /// <summary>
    ///     unspecified type (placeholder)
    /// </summary>
    internal class UnspecifiedType : HiddenIntrinsicType, IUnspecifiedType {

        /// <summary>
        ///     create a new unspecified type
        /// </summary>
        /// <param name="definingUnit"></param>
        public UnspecifiedType(IUnitType definingUnit) : base(definingUnit) {
        }

        public override bool Equals(ITypeDefinition? other)
            => other is IUnspecifiedType;

        public override int GetHashCode()
            => 0;
    }
}
