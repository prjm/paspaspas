using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Routines {

    /// <summary>
    ///     base class for intrinsic routines
    /// </summary>
    public abstract class IntrinsicRoutine : IRoutine {

        /// <summary>
        ///     routine name
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        ///     type id
        /// </summary>
        public int TypeId
            => KnownTypeIds.IntrinsicRoutine;

        /// <summary>
        ///     type registry
        /// </summary>
        public ITypeRegistry TypeRegistry { get; set; }

        /// <summary>
        ///     runtime
        /// </summary>
        public IRuntimeValueFactory Runtime
            => TypeRegistry.Runtime;

        /// <summary>
        ///     integer values
        /// </summary>
        public IIntegerOperations Integers
            => TypeRegistry.Runtime.Integers;

        /// <summary>
        ///     chars
        /// </summary>
        public ICharOperations Chars
            => TypeRegistry.Runtime.Chars;

        /// <summary>
        ///     integer values
        /// </summary>
        public IBooleanOperations Booleans
            => TypeRegistry.Runtime.Booleans;

        /// <summary>
        ///     types
        /// </summary>
        public ITypeOperations Types
            => TypeRegistry.Runtime.Types;

        /// <summary>
        ///     strings
        /// </summary>
        public IStringOperations Strings
            => TypeRegistry.Runtime.Strings;

        /// <summary>
        ///     real number values
        /// </summary>
        public IRealNumberOperations RealNumbers
            => TypeRegistry.Runtime.RealNumbers;

        /// <summary>
        ///     make a subrange value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public ITypeReference MakeSubrangeValue(int typeId, ITypeReference value)
            => Runtime?.Types?.MakeSubrangeValue(typeId, value);

        /// <summary>
        ///     make a type instance reference
        /// </summary>
        /// <param name="typeId"></param>
        /// <returns></returns>
        public ITypeReference MakeTypeInstanceReference(int typeId)
            => Runtime?.Types?.MakeTypeInstanceReference(typeId, TypeRegistry.GetTypeKindOf(typeId));

        /// <summary>
        ///     retrieve an ordinal type
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="ordinalType"></param>
        /// <returns></returns>
        public bool IsOrdinalType(int typeId, out IOrdinalType ordinalType) {
            ordinalType = TypeRegistry.GetTypeByIdOrUndefinedType(typeId) as IOrdinalType;

            if (ordinalType != default && ordinalType.TypeKind.IsOrdinal())
                return true;

            ordinalType = default;
            return false;
        }

        /// <summary>
        ///     test if the given type is a short string type
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="shortStringType"></param>
        /// <returns></returns>
        public bool IsShortStringType(int typeId, out IShortStringType shortStringType) {
            shortStringType = TypeRegistry.GetTypeByIdOrUndefinedType(typeId) as IShortStringType;

            if (shortStringType != default && shortStringType.TypeKind == CommonTypeKind.ShortStringType)
                return true;

            shortStringType = default;
            return false;
        }

        /// <summary>
        ///     test if the given type is a subrange type
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="subrangeType"></param>
        /// <returns></returns>
        public bool IsSubrangeType(int typeId, out ISubrangeType subrangeType) {
            subrangeType = TypeRegistry.GetTypeByIdOrUndefinedType(typeId) as ISubrangeType;

            if (subrangeType != default && subrangeType.TypeKind == CommonTypeKind.SubrangeType)
                return true;

            subrangeType = default;
            return false;
        }

        /// <summary>
        ///     test if the given type is a array type
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="arrayType"></param>
        /// <returns></returns>
        public bool IsArrayType(int typeId, out IArrayType arrayType) {
            arrayType = TypeRegistry.GetTypeByIdOrUndefinedType(typeId) as IArrayType;

            if (arrayType != default && arrayType.TypeKind.IsArray())
                return true;

            arrayType = default;
            return false;
        }

        /// <summary>
        ///     resolve a call
        /// </summary>
        /// <param name="callableRoutines"></param>
        /// <param name="signature"></param>
        public virtual void ResolveCall(IList<IParameterGroup> callableRoutines, Signature signature) {
            if (this is IUnaryRoutine unaryRoutine)
                ResolveCall(unaryRoutine, callableRoutines, signature);

            if (this is IVariadicRoutine variadicRoutine)
                ResolveCall(variadicRoutine, callableRoutines, signature);
        }

        private static void ResolveCall(IUnaryRoutine unaryRoutine, IList<IParameterGroup> callableRoutines, Signature signature) {
            if (signature.Length != 1)
                return;

            var parameter = signature[0];

            if (!unaryRoutine.CheckParameter(parameter))
                return;

            var result = new ParameterGroup();
            result.AddParameter("AValue").SymbolType = parameter;
            callableRoutines.Add(result);

            if (unaryRoutine.IsConstant && parameter.IsConstant())
                result.ResultType = unaryRoutine.ExecuteCall(parameter);
            else
                result.ResultType = unaryRoutine.ResolveCall(parameter);
        }


        private static void ResolveCall(IVariadicRoutine variadicRoutine, IList<IParameterGroup> callableRoutines, Signature signature) {

            if (!variadicRoutine.CheckParameter(signature))
                return;

            var result = new ParameterGroup();

            for (var i = 0; i < signature.Length; i++)
                result.AddParameter($"AValue{signature}").SymbolType = signature[0];
            callableRoutines.Add(result);

            if (variadicRoutine.IsConstant && signature.IsConstant && variadicRoutine is IConstantVariadicRoutine cvr)
                result.ResultType = cvr.ExecuteCall(signature);
            else
                result.ResultType = variadicRoutine.ResolveCall(signature);
        }


        /// <summary>
        ///     stub: make an runtime exception
        /// </summary>
        /// <returns></returns>
        protected ITypeReference RuntimeException()
            => Runtime.Types.MakeErrorTypeReference(); // ... to be changed

    }
}
