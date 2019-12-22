using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Simple {

    /// <summary>
    ///     base class for string types
    /// </summary>
    public abstract class StringTypeBase : TypeBase, IStringType {

        /// <summary>
        ///     create a new string type declaration
        /// </summary>
        /// <param name="withId"></param>
        protected StringTypeBase(int withId) : base(withId) {
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
        ///     Unicode string type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.UnicodeStringType;

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.GetTypeByIdOrUndefinedType(KnownTypeIds.NativeInt).TypeSizeInBytes;

        /// <summary>
        ///     type name
        /// </summary>
        public override string LongName
            => KnownNames.UnicodeString;

        /// <summary>
        ///      short string
        /// </summary>
        public override string ShortName
            => KnownNames.SystemAtUnicodeString;
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
        ///     UNICODE string type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.WideStringType;

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.GetTypeByIdOrUndefinedType(KnownTypeIds.NativeInt).TypeSizeInBytes;

        /// <summary>
        ///     long string name
        /// </summary>
        public override string LongName
            => KnownNames.WideString;

        /// <summary>
        ///     short string name
        /// </summary>
        public override string ShortName
            => KnownNames.SystemAtWideString;
    }

    /// <summary>
    ///     string type definition
    /// </summary>
    public class AnsiStringType : StringTypeBase {

        /// <summary>
        ///     default system code page
        /// </summary>
        public const ushort DefaultSystemCodePage = 0;

        /// <summary>
        ///     no code page
        /// </summary>
        public const ushort NoCodePage = 0xffff;

        /// <summary>
        ///     create a new string type
        /// </summary>
        /// <param name="withId">type id</param>
        /// <param name="withCodePage">code page</param>
        /// <param name="longTypeName">long type name</param>
        public AnsiStringType(string longTypeName, int withId, ushort withCodePage) : base(withId) {
            WithCodePage = withCodePage;
            LongName = longTypeName;
        }

        /// <summary>
        ///      string type
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.LongStringType;

        /// <summary>
        ///     type size in bytes
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.GetTypeByIdOrUndefinedType(KnownTypeIds.NativeInt).TypeSizeInBytes;

        /// <summary>
        ///     code page
        /// </summary>
        public ushort WithCodePage { get; }

        /// <summary>
        ///     long type name
        /// </summary>
        public override string LongName { get; }

        /// <summary>
        ///     short type name
        /// </summary>
        public override string ShortName
            => $"%AnsiStringT$us$i{WithCodePage}$%";

    }


}
