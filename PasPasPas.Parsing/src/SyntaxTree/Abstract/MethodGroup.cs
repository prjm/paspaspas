using System.Collections.Generic;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method group
    /// </summary>
    public class MethodGroup : DeclaredSymbol {

        private readonly List<IMethodImplementation>
            methods = new List<IMethodImplementation>();

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
        internal bool TryToAdd(IMethodImplementation newEntry) {

            if (newEntry.IsForwardDeclaration)
                return true;

            if (newEntry.IsExportedMethod)
                return true;

            for (var i = 0; i < methods.Count; i++) {
                var method = methods[i];

                if (method.IsForwardDeclaration)
                    continue;

                if (method.IsExportedMethod)
                    return true;

                return false;
            }

            methods.Add(newEntry);
            return true;
        }

        /// <summary>
        ///     add a method entry
        /// </summary>
        /// <param name="entry"></param>
        internal void Add(IMethodImplementation entry)
            => methods.Add(entry);
    }
}
