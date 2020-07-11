﻿using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     base class for string types
    /// </summary>
    internal abstract class StringTypeBase : CompilerDefinedType, IStringType {

        /// <summary>
        ///     create a new string type declaration
        /// </summary>
        /// <param name="definingUnit"></param>
        protected StringTypeBase(IUnitType definingUnit) : base(definingUnit) {
        }

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(ITypeDefinition? other)
            => other is StringTypeBase s &&
                s.Kind == Kind &&
                KnownNames.SameIdentifier(Name, s.Name);

        /// <summary>
        ///     string type kind
        /// </summary>
        public abstract StringTypeKind Kind { get; }

        /*
        /// <summary>
        ///     check if this type can be assigned from another type
        /// </summary>
        /// <param name="otherType"></param>
        /// <returns></returns>
        public override bool CanBeAssignedFrom(ITypeDefinition otherType) {

            if (otherType.TypeKind.IsString()) {
                return true;
            }

            if (otherType.TypeKind.IsChar()) {
                return true;
            }

            return base.CanBeAssignedFrom(otherType);
        }
*/
    }


}
