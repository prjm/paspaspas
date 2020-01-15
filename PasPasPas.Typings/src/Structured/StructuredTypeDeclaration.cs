using System;
using System.Collections.Generic;
using PasPasPas.Globals.Environment;
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
        /// <param name="withId"></param>
        /// <param name="kind"></param>
        public StructuredTypeDeclaration(int withId, StructuredTypeKind kind) : base(withId)
            => StructTypeKind = kind;

        /// <summary>
        ///     structured type kind
        /// </summary>
        public StructuredTypeKind StructTypeKind { get; }

        /// <summary>
        ///     get the type kind
        /// </summary>
        public override CommonTypeKind TypeKind {
            get {
                switch (StructTypeKind) {

                    case StructuredTypeKind.Class:
                        return CommonTypeKind.ClassType;

                    case StructuredTypeKind.ClassHelper:
                        return CommonTypeKind.ClassHelperType;

                    case StructuredTypeKind.DispInterface:
                        return CommonTypeKind.InterfaceType;

                    case StructuredTypeKind.ObjectType:
                        return CommonTypeKind.RecordType;

                    case StructuredTypeKind.Record:
                        return CommonTypeKind.RecordType;

                    case StructuredTypeKind.RecordHelper:
                        return CommonTypeKind.RecordHelperType;

                    case StructuredTypeKind.Interface:
                        return CommonTypeKind.InterfaceType;

                    default:
                        return CommonTypeKind.UnknownType;
                }
            }
        }

        /// <summary>
        ///     base class
        /// </summary>
        public int BaseClassId { get; set; }

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
                        return TypeRegistry.GetTypeByIdOrUndefinedType(KnownTypeIds.NativeInt).TypeSizeInBytes;

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
                    entry = new Reference(ReferenceKind.RefToField, field);
                    return true;
                }
            }

            foreach (var method in Methods)
                if (string.Equals(method.Name, symbolName, StringComparison.OrdinalIgnoreCase)) {
                    entry = new Reference(ReferenceKind.RefToMethod, method);
                    return true;
                }

            var baseClass = TypeRegistry.GetTypeByIdOrUndefinedType(BaseClassId);
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
        public override void ResolveCall(string symbolName, IList<IRoutine> callables, Signature signature) {
            base.ResolveCall(symbolName, callables, signature);

            var baseClass = TypeRegistry.GetTypeByIdOrUndefinedType(BaseClassId);
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

                foreach (var field in Fields)
                    if (!field.SymbolType.IsConstant())
                        return false;

                return true;
            }
        }

        /// <summary>
        ///     create a constant record value from this type declaration
        /// </summary>
        /// <returns></returns>
        public IOldTypeReference MakeConstant() {
            using (var list = GetList<IOldTypeReference>()) {
                foreach (var value in Fields) {
                    list.Add(value.SymbolType);
                    ((Variable)value).SymbolType = TypeRegistry.Runtime.Types.MakeTypeInstanceReference(value.SymbolType.TypeId, value.SymbolType.TypeKind);
                }

                return TypeRegistry.Runtime.Structured.CreateRecordValue(TypeId, TypeRegistry.ListPools.GetFixedArray(list));
            }
        }

        /// <summary>
        ///     format this type as string
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            switch (StructTypeKind) {
                case StructuredTypeKind.Class:
                    return $"class {TypeId}";
                case StructuredTypeKind.ClassHelper:
                    return $"class helper {TypeId}";
                case StructuredTypeKind.DispInterface:
                    return $"disp interface {TypeId}";
                case StructuredTypeKind.Interface:
                    return $"interface {TypeId}";
                case StructuredTypeKind.ObjectType:
                    return $"object {TypeId}";
                case StructuredTypeKind.Record:
                    return $"record {TypeId}";
                case StructuredTypeKind.RecordHelper:
                    return $"record helper {TypeId}";
            }

            return base.ToString();
        }

        /// <summary>
        ///     check if this type inherits from another type
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public bool InheritsFrom(int typeId) {
            var baseClass = this;

            while (baseClass != default) {
                if (baseClass.TypeId == typeId)
                    return true;

                baseClass = TypeRegistry.GetTypeByIdOrUndefinedType(BaseClassId) as StructuredTypeDeclaration;
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
