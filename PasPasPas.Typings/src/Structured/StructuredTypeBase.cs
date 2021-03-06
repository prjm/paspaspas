﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     base class for structured types
    /// </summary>
    internal abstract class StructuredTypeBase : TypeDefinitionBase {

        /// <summary>
        ///     create a new structured type
        /// </summary>
        /// <param name="defininingUnit"></param>
        protected StructuredTypeBase(IUnitType defininingUnit) : base(defininingUnit) {
        }

        /// <summary>
        ///     resolve a method
        /// </summary>
        /// <param name="signature"></param>
        /// <param name="symbolName">method name</param>
        /// <param name="callables">callable methods</param>
        /// <returns></returns>
        public virtual void ResolveCall(string symbolName, IList<IRoutineResult> callables, ISignature signature) {

            foreach (var method in Methods)
                if (string.Equals(method.Name, symbolName, StringComparison.OrdinalIgnoreCase))
                    method.ResolveCall(callables, signature);
        }

        /// <summary>
        ///     list of routines
        /// </summary>
        public List<IRoutineGroup> Methods { get; }
            = new List<IRoutineGroup>();

        /// <summary>
        ///     add a method definition
        /// </summary   >
        /// <param name="completeName">method name</param>
        /// <param name="genericTypeId">generic type id</param>
        public IRoutineGroup AddOrExtendMethod(string completeName, ITypeDefinition genericTypeId) {
            foreach (var method in Methods)
                if (string.Equals(method.Name, completeName, StringComparison.OrdinalIgnoreCase))
                    return method;

            if (TypeRegistry == null)
                throw new InvalidOperationException();

            var newMethod = new RoutineGroup(this, completeName, genericTypeId);
            Methods.Add(newMethod);
            return newMethod;
        }

        /// <summary>
        ///     add a generic parameter
        /// </summary>
        /// <param name="typeId"></param>
        public void AddGenericParameter(ITypeDefinition typeId)
            => GenericParameters.Add(typeId);

        /// <summary>
        ///     list of generic parameter
        /// </summary>
        public virtual List<ITypeDefinition> GenericParameters { get; }
            = new List<ITypeDefinition>();

        /// <summary>
        ///     bind generic type
        /// </summary>
        /// <param name="typeIds"></param>
        /// <param name="typeCreator"></param>
        /// <returns></returns>
        public virtual ITypeDefinition Bind(ImmutableArray<ITypeDefinition> typeIds, ITypeCreator typeCreator)
            => throw new InvalidOperationException();

        /// <summary>
        ///     number of type parameters
        /// </summary>
        public virtual int NumberOfTypeParameters
            => GenericParameters.Count;

    }
}
