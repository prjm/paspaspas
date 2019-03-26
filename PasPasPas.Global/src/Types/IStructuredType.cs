﻿using PasPasPas.Globals.Runtime;

namespace PasPasPas.Globals.Types {

    /// <summary>
    ///     structured type definition
    /// </summary>
    public interface IStructuredType : ITypeDefinition {

        /// <summary>
        ///     base class
        /// </summary>
        ITypeReference BaseClass { get; set; }

        /// <summary>
        ///     meta class
        /// </summary>
        ITypeReference MetaType { get; set; }


    }
}
