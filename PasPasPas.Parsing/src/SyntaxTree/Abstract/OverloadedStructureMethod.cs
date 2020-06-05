#nullable disable
using PasPasPas.Globals.Parsing;
using PasPasPas.Parsing.SyntaxTree.Utils;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     overloaded structure method
    /// </summary>
    public class OverloadedStructureMethod : StructureMethod {


        /// <summary>
        ///     overloaded methods
        /// </summary>
        public ISyntaxPartCollection<StructureMethod> Overloads { get; }
            = new SyntaxPartCollection<StructureMethod>();

        /// <summary>
        ///     accept a visitor
        /// </summary>
        /// <param name="visitor"></param>
        protected override void AcceptBaseParts(IStartEndVisitor visitor) {
            base.AcceptBaseParts(visitor);
            if (Overloads != null)
                AcceptPart(this, Overloads, visitor);
        }



        /// <summary>
        ///     add an overloaded methods
        /// </summary>
        /// <param name="entry"></param>
        public void AddOverload(StructureMethod entry)
            => Overloads.Add(entry);

    }
}
