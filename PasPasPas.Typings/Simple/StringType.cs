﻿using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     base class for string types
    /// </summary>
    public abstract class StringTypeBase : TypeBase {

        /// <summary>
        ///     create a new string type declaration
        /// </summary>
        /// <param name="withId"></param>
        public StringTypeBase(int withId) : base(withId) {
        }

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
    }

    /// <summary>
    ///     string type definition
    /// </summary>
    public class UnicodeStringType : StringTypeBase {

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="withId">type id</param>
        public UnicodeStringType(int withId) : base(withId) {
        }

        /// <summary>
        ///     unicode string type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.UnicodeStringType;
    }


    /// <summary>
    ///     string type definition
    /// </summary>
    public class WideStringType : StringTypeBase {

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="withId">type id</param>
        public WideStringType(int withId) : base(withId) {
        }

        /// <summary>
        ///     unicode string type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.WideStringType;
    }

    /// <summary>
    ///     string type definition
    /// </summary>
    public class ShortStringType : StringTypeBase {

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="withId">type id</param>
        public ShortStringType(int withId) : base(withId) {
        }

        /// <summary>
        ///     unicode string type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.ShortStringType;
    }

    /// <summary>
    ///     string type definition
    /// </summary>
    public class AnsiStringType : StringTypeBase {

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="withId">type id</param>
        public AnsiStringType(int withId) : base(withId) {
        }

        /// <summary>
        ///     unicode string type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.ShortStringType;
    }


}
