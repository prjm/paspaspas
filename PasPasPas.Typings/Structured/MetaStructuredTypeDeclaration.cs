﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     class of / meta class declaration
    /// </summary>
    public class MetaStructuredTypeDeclaration : StructuredTypeBase {

        /// <summary>
        ///     base type id
        /// </summary>
        private int baseTypeId;

        /// <summary>
        ///     create a meta type declaration
        /// </summary>
        /// <param name="withId"></param>
        /// <param name="baseType"></param>
        public MetaStructuredTypeDeclaration(int withId, int baseType) : base(withId)
            => baseTypeId = baseType;

        /// <summary>
        ///     type kind
        /// </summary>
        public override CommonTypeKind TypeKind
            => CommonTypeKind.ClassReferenceType;

        /// <summary>
        ///     base type id
        /// </summary>
        public int BaseType
            => baseTypeId;

        /// <summary>
        ///     list of fieds
        /// </summary>
        public IList<Variable> Fields { get; }
            = new List<Variable>();

        /// <summary>
        ///     add a field definition
        /// </summary>
        /// <param name="variable">variable name</param>
        public void AddField(Variable variable)
            => Fields.Add(variable);

        /// <summary>
        ///     resolve a name is this meta type
        /// </summary>
        /// <param name="symbolName"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool TryToResolve(string symbolName, out Reference entry) {
            foreach (var field in Fields)
                if (string.Equals(field.Name, symbolName, StringComparison.OrdinalIgnoreCase)) {
                    entry = new Reference(ReferenceKind.RefToClassField, field);
                    return true;
                }

            entry = null;
            return false;
        }
    }
}
