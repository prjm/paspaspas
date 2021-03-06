﻿using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     procedural type
    /// </summary>
    internal class RoutineType : TypeDefinitionBase, IRoutineType {

        /// <summary>
        ///     create a new routine type
        /// </summary>
        /// <param name="definingType"></param>
        /// <param name="name"></param>
        public RoutineType(IUnitType definingType, string name) : base(definingType)
            => Name = name;

        /// <summary>
        ///     base type kind
        /// </summary>
        public override BaseType BaseType
            => BaseType.Routine;

        /// <summary>
        ///     undefined type size
        /// </summary>
        public override uint TypeSizeInBytes
            => TypeRegistry.GetPointerSize();

        /// <summary>
        ///     generic parameters
        /// </summary>
        public List<ITypeDefinition> GenericParameters
            => new List<ITypeDefinition>();

        /// <summary>
        ///     number of generic type parameters
        /// </summary>
        public int NumberOfTypeParameters
            => GenericParameters.Count;

        /// <summary>
        ///     type name
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///     add a generic parameter
        /// </summary>
        /// <param name="typeId"></param>
        public void AddGenericParameter(ITypeDefinition typeId)
            => GenericParameters.Add(typeId);

        /// <summary>
        ///     bind the generic type parameter
        /// </summary>
        /// <param name="typeIds"></param>
        /// <param name="typeCreator"></param>
        /// <returns></returns>
        public ITypeDefinition Bind(ImmutableArray<ITypeDefinition> typeIds, ITypeCreator typeCreator)
            => throw new InvalidOperationException();

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public override bool Equals(ITypeDefinition? other)
            => other is IRoutineType;

        public override int GetHashCode()
            => 0;

    }
}
