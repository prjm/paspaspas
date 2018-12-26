﻿using PasPasPas.Globals.Runtime;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     invalid / error type
    /// </summary>
    public class ErrorType : TypeBase {

        /// <summary>
        ///     creates a new invalid / error type
        /// </summary>
        /// <param name="withId"></param>
        public ErrorType(int withId) : base(withId) { }

        /// <summary>
        ///     unknown type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.UnknownType;

        /// <summary>
        ///     invalid size
        /// </summary>
        public override uint TypeSizeInBytes
            => 0;
    }
}
