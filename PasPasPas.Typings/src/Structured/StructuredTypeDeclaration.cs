using System;
using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Structured {

    /// <summary>
    ///     structured type declaration
    /// </summary>
    public class StructuredTypeDeclaration : StructuredTypeBase, IStructuredType {

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

                    case StructuredTypeKind.ObjectType:
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
        public ITypeReference BaseClass { get; set; }

        /// <summary>
        ///     list of fields
        /// </summary>
        public IList<Variable> Fields { get; }
            = new List<Variable>();

        /// <summary>
        ///     meta type
        /// </summary>
        public ITypeReference MetaType { get; set; }

        /// <summary>
        ///
        /// </summary>
        public override uint TypeSizeInBytes {
            get {
                switch (typeKind) {

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

            var metaType = TypeRegistry.GetTypeByIdOrUndefinedType(MetaType.TypeId) as MetaStructuredTypeDeclaration;
            if (metaType != default) {
                foreach (var classVar in metaType.Fields)
                    if (string.Equals(classVar.Name, symbolName, StringComparison.OrdinalIgnoreCase)) {
                        entry = new Reference(ReferenceKind.RefToMetaClassField, classVar);
                        return true;
                    }
            }

            if (BaseClass != null && BaseClass is StructuredTypeDeclaration baseType)
                return baseType.TryToResolve(symbolName, out entry);

            entry = null;
            return false;
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="symbolName"></param>
        /// <param name="callables"></param>
        /// <param name="signature"></param>
        public override void ResolveCall(string symbolName, IList<ParameterGroup> callables, Signature signature) {
            base.ResolveCall(symbolName, callables, signature);

            if (BaseClass != null && BaseClass is StructuredTypeDeclaration baseType)
                baseType.ResolveCall(symbolName, callables, signature);
        }

        /// <summary>
        ///     check if this is a constant record value
        /// </summary>
        public bool IsConstant {
            get {
                if (typeKind != StructuredTypeKind.Record)
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
        public ITypeReference MakeConstant() {
            using (var list = GetList<ITypeReference>()) {
                foreach (var value in Fields) {
                    list.Add(value.SymbolType);
                    value.SymbolType = TypeRegistry.Runtime.Types.MakeTypeInstanceReference(value.SymbolType.TypeId, value.SymbolType.TypeKind);
                }

                return TypeRegistry.Runtime.Structured.CreateRecordValue(TypeId, TypeRegistry.ListPools.GetFixedArray(list));
            }
        }

        /// <summary>
        ///     format this type as string
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            switch (typeKind) {
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
    }
}
