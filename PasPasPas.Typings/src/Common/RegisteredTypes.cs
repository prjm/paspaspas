using System.Collections.Generic;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Operators;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     common type registry - contains all registered units
    /// </summary>
    public class RegisteredTypes : ITypeRegistry, IEnvironmentItem {

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
        ///     count registered types
        /// </summary>
        public int Count {
            get {
                var result = 0;
                foreach (var unit in units)
                    result += unit.Count;
                return result;
            }
        }

        /// <summary>
        ///     registered units
        /// </summary>
        public IEnumerable<IUnitType> Units
            => units;

        /// <summary>
        ///     create a new type registry
        /// </summary>
        /// <param name="intSize">integer size</param>
        /// <param name="runtime">runtime values</param>
        /// <param name="listPools">list pools</param>
        public RegisteredTypes(IRuntimeValueFactory runtime, IListPools listPools, NativeIntSize intSize) {
            Runtime = runtime;
            ListPools = listPools;
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
        public IOperator GetOperator(OperatorKind operatorKind) {
            if (operators.TryGetValue(operatorKind, out var result))
                return result;
            return null;
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


        /// <summary>
        ///     cast one type to another type
        /// </summary>
        /// <param name="sourceType">source type</param>
        /// <param name="targetType">target type</param>
        /// <returns></returns>
        public int Cast(int sourceType, int targetType) {
            sourceType = ResolveAlias(sourceType);

            if (sourceType == ResolveAlias(targetType))
                return targetType;

            var sourceTypeKind = GetTypeKindOf(sourceType);

            if (sourceTypeKind.IsIntegral())
                return CastIntTo(targetType);

            if (sourceTypeKind.IsChar())
                return CastCharTo(targetType);

            if (sourceTypeKind == CommonTypeKind.BooleanType)
                return CastBooleanTo(targetType);

            if (sourceTypeKind == CommonTypeKind.RecordType)
                return CastRecordTo(sourceType, targetType);

            return Ids.Unused;
        }

        private int CastIntTo(int targetType) {
            var targetTypeKind = GetTypeKindOf(targetType);

            if (targetTypeKind.IsIntegral())
                return targetType;

            if (targetTypeKind.IsChar())
                return targetType;

            if (targetTypeKind == CommonTypeKind.BooleanType)
                return targetType;

            if (targetTypeKind == CommonTypeKind.EnumerationType)
                return targetType;

            if (targetTypeKind == CommonTypeKind.SubrangeType)
                return targetType;

            return Ids.Unused;
        }

        private int CastRecordTo(int sourceType, int targetType) {
            if (this.AreRecordTypesCompatible(sourceType, targetType))
                return targetType;

            return Ids.Unused;
        }

        private int CastBooleanTo(int targetType) {
            var targetTypeKind = GetTypeKindOf(targetType);

            if (targetTypeKind == CommonTypeKind.BooleanType)
                return targetType;

            return Ids.Unused;
        }

        private int CastCharTo(int targetType) {
            var targetTypeKind = GetTypeKindOf(targetType);

            if (targetTypeKind.IsIntegral())
                return targetType;

            if (targetTypeKind.IsChar())
                return targetType;

            if (targetTypeKind.IsString())
                return targetType;

            if (targetTypeKind == CommonTypeKind.BooleanType)
                return targetType;

            if (targetTypeKind == CommonTypeKind.EnumerationType)
                return targetType;

            if (targetTypeKind == CommonTypeKind.SubrangeType)
                return targetType;

            return Ids.Unused;
        }
        */

        /// <summary>
        ///     find an intrinsic routine from the system unit
        /// </summary>
        /// <param name="routineId"></param>
        /// <returns></returns>
        public IRoutineGroup GetIntrinsicRoutine(IntrinsicRoutineId routineId) {
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
