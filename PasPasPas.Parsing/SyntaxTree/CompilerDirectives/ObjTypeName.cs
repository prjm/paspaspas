using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.CompilerDirectives {

    /// <summary>
    ///     object type name directive
    /// </summary>
    public class ObjTypeName : CompilerDirectiveBase {

        /// <summary>
        ///     alias name
        /// </summary>
        public string AliasName { get; set; }

        /// <summary>
        ///     type name in object file
        /// </summary>
        public string TypeName { get; set; }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="startVisitor">start visitor</param>
        /// <param name="endVisitor">end visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptParts(this, visitor);
            visitor.EndVisit(this);
        }


    }
}
