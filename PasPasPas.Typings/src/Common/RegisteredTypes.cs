using System.Collections.Generic;
using System.Text;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Operators;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     common type registry - contains all registered units
    /// </summary>
    public class RegisteredTypes : ITypeRegistry {

        private readonly List<IUnitType> units
            = new List<IUnitType>();

        private readonly IDictionary<OperatorKind, IOperator> operators
            = new Dictionary<OperatorKind, IOperator>();

        /// <summary>
        ///     system unit
        /// </summary>
        public ISystemUnit SystemUnit { get; }

        /// <summary>
        ///     runtime constant values
        /// </summary>
        public IRuntimeValueFactory Runtime { get; }

        /// <summary>
        ///     list pools
        /// </summary>
        public IListPools ListPools { get; }

        /// <summary>
        ///     string builder pool
        /// </summary>
        public IStringPool StringPool { get; }

        /// <summary>
        ///     string builder pool
        /// </summary>
        public IObjectPool<StringBuilder> StringBuilderPool { get; }

        /// <summary>
        ///     registered units
        /// </summary>
        public IEnumerable<IUnitType> Units
            => units;

        /// <summary>
        ///     create a new type registry
        /// </summary>
        /// <param name="intSize">integer size</param>
        /// <param name="spool"></param>
        /// <param name="sbpool"></param>
        /// <param name="runtime">runtime values</param>
        /// <param name="listPools">list pools</param>
        public RegisteredTypes(IRuntimeValueFactory runtime, IListPools listPools, NativeIntSize intSize, IStringPool spool, IObjectPool<StringBuilder> sbpool) {
            Runtime = runtime;
            ListPools = listPools;
            StringPool = spool;
            StringBuilderPool = sbpool;
            SystemUnit = new SystemUnit(this, intSize);

            RegisterUnit(SystemUnit);
            RegisterCommonOperators();
        }

        /// <summary>
        ///     register a unit
        /// </summary>
        /// <param name="unitType"></param>
        private void RegisterUnit(IUnitType unitType)
            => units.Add(unitType);

        /// <summary>
        ///     create a new unit type
        /// </summary>
        /// <returns></returns>
        public IUnitType CreateUnitType(string name) {
            var result = new UnitType(name, this);
            RegisterUnit(result);
            return result;
        }

        /// <summary>
        ///     register common operators
        /// </summary>
        private void RegisterCommonOperators() {
            LogicalOperator.RegisterOperators(this);
            ArithmeticOperator.RegisterOperators(this);
            RelationalOperator.RegisterOperators(this);
            StringOperator.RegisterOperators(this);
            SetOperator.RegisterOperators(this);
            ClassOperators.RegisterOperators(this);
            OtherOperator.RegisterOperators(this);
        }

        /// <summary>
        ///     register an operator
        /// </summary>
        /// <param name="newOperator">operator to register</param>
        public void RegisterOperator(IOperator newOperator) {
            operators.Add(newOperator.Kind, newOperator);
            if (newOperator is OperatorBase baseOperator) {
                baseOperator.TypeRegistry = this;
            }
        }

        /// <summary>
        ///     gets an registered operator
        /// </summary>
        /// <param name="operatorKind">operator kind</param>
        /// <returns></returns>
        public IOperator? GetOperator(OperatorKind operatorKind) {
            if (operators.TryGetValue(operatorKind, out var result))
                return result;
            return default;
        }

        /*
        private void RegisterTObject() {
            var def = new StructuredTypeDeclaration(Ids.TObject, StructuredTypeKind.Class);
            //var meta = new MetaStructuredTypeDeclaration(KnownTypeIds.TClass, KnownTypeIds.TObject);
            //var alias = TypeCreator.CreateTypeAlias(KnownTypeIds.TClass, false, KnownTypeIds.TClassAlias);
            RegisterSystemType(def, "TObject");
            //RegisterSystemType(meta, "TObject");
            //RegisterSystemType(alias, "TClass");
            //def.MetaType = Runtime.Types.MakeTypeInstanceReference(meta.TypeId, CommonTypeKind.MetaClassType);
            //def.AddOrExtendMethod("Create", ProcedureKind.Constructor).AddParameterGroup();
            //def.AddOrExtendMethod("Free", ProcedureKind.Procedure).AddParameterGroup();
            //def.AddOrExtendMethod("DisposeOf", ProcedureKind.Procedure).AddParameterGroup();
            //def.AddOrExtendMethod("CleanupInstance", ProcedureKind.Procedure).AddParameterGroup();
            //def.AddOrExtendMethod("ClassType", ProcedureKind.Function).AddParameterGroup(KnownTypeIds.TClass);
            //def.AddOrExtendMethod("FieldAddress", ProcedureKind.Function).AddParameterGroup(//
            //   "Name",
            //    KnownTypeIds.ShortStringType, //
            //    KnownTypeIds.GenericPointer)[0].ConstantParam = true;
        }

        */

        /// <summary>
        ///     cast one type to another type
        /// </summary>
        /// <param name="fromType">source type</param>
        /// <param name="toType">target type</param>
        /// <returns></returns>
        public ITypeSymbol Cast(ITypeSymbol fromType, ITypeSymbol toType) {

            if (fromType.IsConstant(out var value))
                return Runtime.Cast(this, value, toType.TypeDefinition);

            var source = fromType.TypeDefinition.ResolveAlias();
            var target = toType.TypeDefinition.ResolveAlias();

            if (fromType == toType)
                return Runtime.Types.MakeCastResult(fromType, toType.TypeDefinition);

            var sourceTypeKind = source.BaseType;

            if (sourceTypeKind == BaseType.Integer)
                return CastIntTo(fromType, target);

            if (sourceTypeKind == BaseType.Char)
                return CastCharTo(fromType, target);

            if (sourceTypeKind == BaseType.Boolean)
                return CastBooleanTo(fromType, target);

            if (sourceTypeKind == BaseType.Structured && fromType is IStructuredType structType && structType.StructTypeKind == StructuredTypeKind.Record)
                return CastRecordTo(fromType, target);

            return Runtime.Types.MakeCastResult(fromType, SystemUnit.ErrorType);
        }

        private ITypeSymbol CastIntTo(ITypeSymbol fromType, ITypeDefinition targetType) {
            var targetTypeKind = targetType.BaseType;

            if (targetTypeKind == BaseType.Char)
                return Runtime.Types.MakeCastResult(fromType, targetType);

            if (targetTypeKind == BaseType.Integer)
                return Runtime.Types.MakeCastResult(fromType, targetType);

            if (targetTypeKind == BaseType.Boolean)
                return Runtime.Types.MakeCastResult(fromType, targetType);

            if (targetTypeKind == BaseType.Enumeration)
                return Runtime.Types.MakeCastResult(fromType, targetType);

            if (targetTypeKind == BaseType.Subrange)
                return Runtime.Types.MakeCastResult(fromType, targetType);

            return Runtime.Types.MakeCastResult(fromType, SystemUnit.ErrorType);
        }

        private ITypeSymbol CastRecordTo(ITypeSymbol sourceType, ITypeDefinition targetType) {
            if (this.AreRecordTypesCompatible(sourceType.TypeDefinition, targetType))
                return Runtime.Types.MakeCastResult(sourceType, targetType);

            return Runtime.Types.MakeCastResult(sourceType, SystemUnit.ErrorType);
        }

        private ITypeSymbol CastBooleanTo(ITypeSymbol fromType, ITypeDefinition targetType) {
            var targetTypeKind = targetType.BaseType;

            if (targetTypeKind == BaseType.Boolean)
                return Runtime.Types.MakeCastResult(fromType, targetType);

            return Runtime.Types.MakeCastResult(fromType, SystemUnit.ErrorType);
        }

        private ITypeSymbol CastCharTo(ITypeSymbol fromType, ITypeDefinition targetType) {
            var targetTypeKind = targetType.BaseType;

            if (targetTypeKind == BaseType.Integer)
                return Runtime.Types.MakeCastResult(fromType, targetType);

            if (targetTypeKind == BaseType.Char)
                return Runtime.Types.MakeCastResult(fromType, targetType);

            if (targetTypeKind == BaseType.String)
                return Runtime.Types.MakeCastResult(fromType, targetType);

            if (targetTypeKind == BaseType.Boolean)
                return Runtime.Types.MakeCastResult(fromType, targetType);

            if (targetTypeKind == BaseType.Enumeration)
                return Runtime.Types.MakeCastResult(fromType, targetType);

            if (targetTypeKind == BaseType.Subrange)
                return Runtime.Types.MakeCastResult(fromType, targetType);

            return Runtime.Types.MakeCastResult(fromType, SystemUnit.ErrorType);
        }

        /// <summary>
        ///     find an intrinsic routine from the system unit
        /// </summary>
        /// <param name="routineId"></param>
        /// <returns></returns>
        public IRoutineGroup? GetIntrinsicRoutine(IntrinsicRoutineId routineId) {
            var system = SystemUnit as IUnitType;

            foreach (var reference in system.Symbols) {
                if (!(reference is IRoutineGroup routine))
                    continue;

                if (routine.RoutineId == routineId)
                    return routine;
            }

            return default;
        }

        /// <summary>
        ///     create a new type factory
        /// </summary>
        /// <param name="unitType"></param>
        /// <returns></returns>
        public ITypeCreator CreateTypeFactory(IUnitType unitType)
            => new CommonTypeCreator(this, unitType);
    }
}
