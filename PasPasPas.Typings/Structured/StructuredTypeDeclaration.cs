using System;
using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

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
        public StructuredTypeDeclaration(int withId, StructuredTypeKind kind) : base(withId)
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

        /// <summary>
        ///     list of routines
        /// </summary>
        public IList<Routine> Methods { get; }
            = new List<Routine>();

        /// <summary>
        ///     list of fieds
        /// </summary>
        public IList<Variable> Fields { get; }
            = new List<Variable>();

        /// <summary>
        ///     meta type
        /// </summary>
        public MetaStructuredTypeDeclaration MetaType { get; set; }

        /// <summary>
        ///     add a method definition
        /// </summary>
        /// <param name="completeName">method name</param>
        /// <param name="kind">method kind</param>
        public Routine AddOrExtendMethod(string completeName, ProcedureKind kind) {
            foreach (var method in Methods)
                if (string.Equals(method.Name, completeName, StringComparison.OrdinalIgnoreCase))
                    return method;

            if (TypeRegistry == null)
                throw new InvalidOperationException();

            var newMethod = new Routine(TypeRegistry, completeName, kind);
            Methods.Add(newMethod);
            return newMethod;
        }

        /// <summary>
        ///     check if this type has a specific method
        /// </summary>
        /// <param name="methodName">methodName to find</param>
        /// <returns></returns>
        public bool HasMethod(string methodName) {
            foreach (var method in Methods)
                if (string.Equals(method.Name, methodName, StringComparison.OrdinalIgnoreCase))
                    return true;
            return false;
        }

        /// <summary>
        ///     add a field definition
        /// </summary>
        /// <param name="variable">variable name</param>
        public void AddField(Variable variable)
            => Fields.Add(variable);

        /// <summary>
        ///     resolve a symbol
        /// </summary>
        /// <param name="symbolName"></param>
        /// <param name="entry"></param>
        /// <returns></returns>
        public bool TryToResolve(string symbolName, out Reference entry) {

            foreach (var field in Fields)
                if (string.Equals(field.Name, symbolName, StringComparison.OrdinalIgnoreCase)) {
                    entry = new Reference(ReferenceKind.RefToField, field);
                    return true;
                }

            foreach (var method in Methods)
                if (string.Equals(method.Name, symbolName, StringComparison.OrdinalIgnoreCase)) {
                    entry = new Reference(ReferenceKind.RefToMethod, method);
                    return true;
                }

            if (BaseClass != null && BaseClass is StructuredTypeDeclaration baseType)
                return baseType.TryToResolve(symbolName, out entry);

            entry = null;
            return false;
        }

        /// <summary>
        ///     resolve a method
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="symbolName">method name</param>
        /// <param name="callables">callables</param>
        /// <returns></returns>
        public void ResolveCall(string symbolName, IList<ParameterGroup> callables, Signature signature) {

            foreach (var method in Methods)
                if (string.Equals(method.Name, symbolName, StringComparison.OrdinalIgnoreCase))
                    method.ResolveCall(callables, signature);


            if (BaseClass != null && BaseClass is StructuredTypeDeclaration baseType)
                baseType.ResolveCall(symbolName, callables, signature);
        }
    }
}
