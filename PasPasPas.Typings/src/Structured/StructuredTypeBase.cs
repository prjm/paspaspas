using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     base class for structured types
    /// </summary>
    public abstract class StructuredTypeBase : TypeBase {

        /// <summary>
        ///     create a new structured type
        /// </summary>
        /// <param name="withId"></param>
        protected StructuredTypeBase(int withId) : base(withId) {
        }

        /// <summary>
        ///     resolve a method
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="symbolName">method name</param>
        /// <param name="callables">callable methods</param>
        /// <returns></returns>
        public virtual void ResolveCall(string symbolName, IList<ParameterGroup> callables, Signature signature) {

            foreach (var method in Methods)
                if (string.Equals(method.Name, symbolName, StringComparison.OrdinalIgnoreCase))
                    method.ResolveCall(callables, signature);
        }

        /// <summary>
        ///     list of routines
        /// </summary>
        public IList<Routine> Methods { get; }
            = new List<Routine>();

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
        ///     add a generic parameter
        /// </summary>
        /// <param name="typeId"></param>
        public void AddGenericParameter(int typeId) {
            GenericParameters.Add(typeId);
        }

        /// <summary>
        ///     list of generic parameter
        /// </summary>
        public virtual List<int> GenericParameters { get; } = new List<int>();

        /// <summary>
        ///     bind generic type
        /// </summary>
        /// <param name="typeIds"></param>
        /// <returns></returns>
        public virtual Reference Bind(ImmutableArray<int> typeIds)
            => default;

        /// <summary>
        ///     number of type parameters
        /// </summary>
        public virtual int NumberOfTypeParameters
            => GenericParameters.Count;

    }
}
