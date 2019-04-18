using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method group
    /// </summary>
    public class MethodGroup : DeclaredSymbol {

        private readonly List<MethodImplementation>
            methods = new List<MethodImplementation>();

        /// <summary>
        ///     accept a visitor
        /// </summary>
        /// <param name="visitor"></param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            for (var i = 0; i < methods.Count; i++)
                AcceptPart(this, methods[i], visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     try to add another method
        /// </summary>
        /// <param name="newEntry"></param>
        /// <returns></returns>
        internal bool TryToAdd(MethodImplementation newEntry) {

            if ((newEntry.Flags & MethodImplementationFlags.ForwardDeclaration) == MethodImplementationFlags.ForwardDeclaration)
                return true;

            for (var i = 0; i < methods.Count; i++) {
                var method = methods[i];

                if ((method.Flags & MethodImplementationFlags.ForwardDeclaration) == MethodImplementationFlags.ForwardDeclaration)
                    continue;

                return false;
            }

            methods.Add(newEntry);
            return true;
        }

        /// <summary>
        ///     add a method entry
        /// </summary>
        /// <param name="entry"></param>
        internal void Add(MethodImplementation entry)
            => methods.Add(entry);
    }
}
