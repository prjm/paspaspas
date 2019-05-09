using PasPasPas.Globals.Log;
using PasPasPas.Globals.Runtime;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     declaration of a simple value type
    /// </summary>
    public class EnumTypeCollection : SymbolTableBaseCollection<EnumTypeValue>, ITypeSpecification, ITypedSyntaxNode {

        /// <summary>
        ///     type information
        /// </summary>
        public ITypeReference TypeInfo { get; set; }

        /// <summary>
        ///     log duplicate enumeration items
        /// </summary>
        /// <param name="newDuplicate"></param>
        /// <param name="logSource"></param>
        protected override void LogDuplicateSymbolError(EnumTypeValue newDuplicate, ILogSource logSource) {
            base.LogDuplicateSymbolError(newDuplicate, logSource);
            logSource.LogError(StructuralErrors.RedeclaredEnumName, newDuplicate);
        }

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">node visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart<EnumTypeCollection, EnumTypeValue>(this, visitor);
            visitor.EndVisit(this);
        }

    }
}
