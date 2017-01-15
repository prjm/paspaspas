﻿using System.Collections.Generic;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     method
    /// </summary>
    public class StructureMethod : MethodDeclaration, IParameterTarget, ITypeTarget {

        /// <summary>
        ///     directives
        /// </summary>
        public IList<MethodDirective> Directives { get; }
            = new List<MethodDirective>();

        /// <summary>
        ///     generic method parameter
        /// </summary>
        public GenericTypes Generics { get; set; }

        /// <summary>
        ///     parts
        /// </summary>
        public override IEnumerable<ISyntaxPart> Parts {
            get {
                foreach (ISyntaxPart part in base.Parts)
                    yield return part;
                foreach (ISyntaxPart genericType in Generics)
                    yield return genericType;
                foreach (MethodDirective directive in Directives)
                    yield return directive;
            }
        }

        /// <summary>
        ///     symbol hints
        /// </summary>
        public SymbolHints Hints { get; set; }

        /// <summary>
        ///     <c>true</c> if class method
        /// </summary>
        public bool ClassItem { get; set; }

    }
}