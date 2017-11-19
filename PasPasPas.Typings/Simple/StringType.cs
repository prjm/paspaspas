using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Types;
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
        /// <param name="withName"></param>
        public StringTypeBase(int withId, ScopedName withName = null) : base(withId, withName) {
        }

        /// <summary>
        ///     povide scope
        /// </summary>
        /// <param name="completeName"></param>
        /// <param name="scope"></param>
        public override void ProvideScope(string completeName, IScope scope) {
            // (record helpers?)
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
        /// <param name="withName">type name</param>
        public UnicodeStringType(int withId, ScopedName withName = null) : base(withId, withName) {
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
        /// <param name="withName">type name</param>
        public WideStringType(int withId, ScopedName withName = null) : base(withId, withName) {
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
        /// <param name="withName">type name</param>
        public ShortStringType(int withId, ScopedName withName = null) : base(withId, withName) {
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
        /// <param name="withName">type name</param>
        public AnsiStringType(int withId, ScopedName withName = null) : base(withId, withName) {
        }

        /// <summary>
        ///     unicode string type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.ShortStringType;
    }


}
