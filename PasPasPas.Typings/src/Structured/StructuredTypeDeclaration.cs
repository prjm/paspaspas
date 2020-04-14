using System;
using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     structured type declaration
    /// </summary>
    public class StructuredTypeDeclaration : StructuredTypeBase, IStructuredType {


        /// <summary>
        ///     create a new structured type declaration
        /// </summary>
        /// <param name="definingUnit"></param>
        /// <param name="name"></param>
        /// <param name="kind"></param>
        public StructuredTypeDeclaration(IUnitType definingUnit, string name, StructuredTypeKind kind) : base(definingUnit) {
            Name = name;
            StructTypeKind = kind;
        }


        /// <summary>
        ///     type name
        /// </summary>
        public override string Name { get; }

        /// <summary>
        ///     structured type kind
        /// </summary>
        public StructuredTypeKind StructTypeKind { get; }

        /// <summary>
        ///     base class
        /// </summary>
        public ITypeDefinition BaseClass { get; set; }

        /// <summary>
        ///     list of fields
        /// </summary>
        public List<IVariable> Fields { get; }
            = new List<IVariable>();

        /// <summary>
        ///
        /// </summary>
        public override uint TypeSizeInBytes {
            get {
                switch (StructTypeKind) {

                    case StructuredTypeKind.Class:
                    case StructuredTypeKind.ClassHelper:
                    case StructuredTypeKind.RecordHelper:
                    case StructuredTypeKind.DispInterface:
                    case StructuredTypeKind.Interface:
                        return TypeRegistry.SystemUnit.NativeIntType.TypeSizeInBytes;

                    case StructuredTypeKind.Record:
                    case StructuredTypeKind.ObjectType:
                        return 0; //... currently not supported



                    default:
                        return 0;
                }
            }
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
        /// <param name="flags">flags</param>
        /// <returns></returns>
        public bool TryToResolve(string symbolName, out Reference entry, ResolverFlags flags) {

            foreach (var field in Fields) {

                if (field.Visibility == MemberVisibility.StrictPrivate && flags.MustSkipPrivate())
                    continue;

                if (field.Visibility == MemberVisibility.Private && flags.MustSkipPrivate() && flags.IsResolvingFromAnotherUnit())
                    continue;

                if (field.Visibility == MemberVisibility.StrictProtected && flags.MustSkipProtected())
                    continue;

                if (field.Visibility == MemberVisibility.Protected && flags.MustSkipProtected() && flags.IsResolvingFromAnotherUnit())
                    continue;

                if (!field.ClassItem && (flags & ResolverFlags.RequireClassSymbols) == ResolverFlags.RequireClassSymbols)
                    continue;

                if (string.Equals(field.Name, symbolName, StringComparison.OrdinalIgnoreCase)) {
                    //entry = new Reference(ReferenceKind.RefToField, field);
                    entry = default;
                    return true;
                }
            }

            foreach (var method in Methods)
                if (string.Equals(method.Name, symbolName, StringComparison.OrdinalIgnoreCase)) {
                    //entry = new Reference(ReferenceKind.RefToMethod, method);
                    entry = default;
                    return true;
                }

            var baseClass = BaseClass;
            if (baseClass is StructuredTypeDeclaration baseType)
                return baseType.TryToResolve(symbolName, out entry, flags | ResolverFlags.SkipPrivate);

            entry = null;
            return false;
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="symbolName"></param>
        /// <param name="callables"></param>
        /// <param name="signature"></param>
        public override void ResolveCall(string symbolName, IList<IRoutine> callables, ISignature signature) {
            base.ResolveCall(symbolName, callables, signature);

            var baseClass = BaseClass;
            if (baseClass is StructuredTypeDeclaration baseType)
                baseType.ResolveCall(symbolName, callables, signature);
        }

        /// <summary>
        ///     check if this is a constant record value
        /// </summary>
        public bool IsConstant {
            get {
                if (StructTypeKind != StructuredTypeKind.Record)
                    return false;

                return false;
                /*
                foreach (var field in Fields)
                    if (!field.IsConstant())
                        return false;

                return true;
                */
            }
        }

        /// <summary>
        ///     structured type
        /// </summary>
        public override BaseType BaseType
            => BaseType.Structured;

        /// <summary>
        ///     mangled type name
        /// </summary>
        public override string MangledName
            => string.Empty;

        /// <summary>
        ///     create a constant record value from this type declaration
        /// </summary>
        /// <returns></returns>
        public ITypeSymbol MakeConstant() {
            using (var list = GetList<ITypeSymbol>()) {
                foreach (var value in Fields) {
                    list.Add(value.SymbolType);
                    ((Variable)value).SymbolType = TypeRegistry.Runtime.Types.MakeTypeInstanceReference(value.SymbolType.TypeId, value.SymbolType.TypeKind);
                }

                return TypeRegistry.Runtime.Structured.CreateRecordValue(TypeId, TypeRegistry.ListPools.GetFixedArray(list));
            }
        }

        /// <summary>
        ///     check if this type inherits from another type
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public bool InheritsFrom(ITypeDefinition typeId) {
            var baseClass = this;

            while (baseClass != default) {
                if (baseClass == typeId)
                    return true;

                baseClass = default;
                //baseClass = TypeRegistry.GetTypeByIdOrUndefinedType(BaseClassId) as StructuredTypeDeclaration;
            }

            return false;
        }

        /// <summary>
        ///     find a given method
        /// </summary>
        /// <param name="name"></param>
        /// <param name="classItem"></param>
        /// <returns></returns>
        public IRoutineGroup FindMethod(string name, bool classItem) {
            foreach (var method in Methods) {
                if (!string.Equals(name, method.Name, StringComparison.OrdinalIgnoreCase))
                    continue;

                foreach (var paramGroup in method.Items) {
                    if (paramGroup.IsClassItem != classItem)
                        continue;

                    return method;
                }
            }

            return default;
        }

    }
}
