using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Infrastructure.Utils;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     structured type declaration
    /// </summary>
    public class StructuredTypeDeclaration : StructuredTypeBase {

        private readonly StructuredTypeKind typeKind;

        /// <summary>
        ///     create a new structured type declaration
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="kind"></param>
        /// <param name="withName"></param>
        public StructuredTypeDeclaration(int withId, StructuredTypeKind kind, ScopedName withName = null) : base(withId, withName)
            => typeKind = kind;

        /// <summary>
        ///     get the type kind
        /// </summary>
        public override CommonTypeKind TypeKind {
            get {
                switch (typeKind) {

                    case StructuredTypeKind.Class:
                        return CommonTypeKind.ClassType;

                    case StructuredTypeKind.ClassHelper:
                        return CommonTypeKind.ClassHelperType;

                    case StructuredTypeKind.DispInterface:
                        return CommonTypeKind.InterfaceType;

                    case StructuredTypeKind.Object:
                        return CommonTypeKind.RecordType;

                    case StructuredTypeKind.Record:
                        return CommonTypeKind.RecordType;

                    case StructuredTypeKind.RecordHelper:
                        return CommonTypeKind.RecordHelperType;

                    default:
                        return CommonTypeKind.UnknownType;
                }
            }
        }

        /// <summary>
        ///     base class
        /// </summary>
        public ITypeDefinition BaseClass { get; set; }
    }
}
