﻿using PasPasPas.Globals.Runtime;

namespace PasPasPas.Runtime.Values {

    /// <summary>
    ///     runtime values: value creation and operations
    /// </summary>
    public partial class RuntimeValueFactory : IRuntimeValueFactory {

        private ITypeReference MakeSubrangeValue(int typeId, ITypeReference value)
            => Types.MakeSubrangeValue(typeId, value);

        /// <summary>
        ///     create a new enumerated value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITypeReference MakeEnumValue(int typeId, ITypeReference value)
            => new EnumeratedValue(typeId, value);
    }
}