using System;
using System.Collections.Generic;
using PasPasPas.AssemblyBuilder.Builder.Definitions;
using PasPasPas.AssemblyBuilder.Builder.Net;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;

namespace PasPasPas.AssemblyBuilder.Builder {

    /// <summary>
    ///     generic tool to build an assembly
    /// </summary>
    public class ProjectAssemblyBuilder {

        /// <summary>
        ///     environment
        /// </summary>
        public IAssemblyBuilderEnvironment Environment { get; }

        /// <summary>
        ///     log source
        /// </summary>
        public ILogSource LogSource { get; }

        /// <summary>
        ///     module builder
        /// </summary>
        private IAssemblyBuilder Builder { get; }

        /// <summary>
        ///     current unit
        /// </summary>
        private IUnitType CurrentUnit { get; set; }

        /// <summary>
        ///     unit type
        /// </summary>
        private ITypeBuilder UnitType { get; set; }

        /// <summary>
        ///     current method definition
        /// </summary>
        private Stack<IMethodBuilder> CurrentMethod { get; }

        /// <summary>
        ///     create a new builder
        /// </summary>
        /// <param name="environment"></param>
        public ProjectAssemblyBuilder(IAssemblyBuilderEnvironment environment) {
            Environment = environment;
            LogSource = environment.Log.CreateLogSource(MessageGroups.AssemblyBuilder);
            Builder = new NetAssemblyBuilder(environment.TypeRegistry);
            CurrentMethod = new Stack<IMethodBuilder>();
        }

        /// <summary>
        ///     prepare to create an assembly
        /// </summary>
        /// <param name="projectName">project name</param>
        public void PrepareAssembly(string projectName) {
            if (string.IsNullOrWhiteSpace(projectName)) {
                LogError(BuilderErrorMessages.UndefinedProjectName);
                return;
            }

            Builder.StartAssembly(projectName);

            foreach (var typeDef in Environment.TypeRegistry.RegisteredTypeDefinitions) {
                if (!(typeDef is IUnitType unit))
                    continue;

                if (string.Equals(unit.Name, "System", StringComparison.OrdinalIgnoreCase))
                    continue;

                PrepareUnit(unit);
            }

            Builder.EndAssembly();
        }

        private void PrepareUnit(IUnitType unit) {
            CurrentUnit = unit;
            UnitType = Builder.StartUnit(unit.Name);

            foreach (var symbol in unit.Symbols.Values) {

                if (symbol.Kind == ReferenceKind.RefToGlobalRoutine) {
                    PrepareGlobalMethod(symbol.Symbol as IRoutine);
                }
                else if (symbol.Kind == ReferenceKind.RefToVariable) {
                    PrepareVariable(symbol.Symbol as IVariable);
                }

            }

            UnitType.CreateType();
            CurrentUnit = default;
            UnitType = default;
        }

        private void PrepareVariable(IVariable variable) {
            if (CurrentMethod.Count < 1) {
                UnitType.DefineClassVariable(variable.Name, variable.SymbolType);
            }
        }

        private void PrepareGlobalMethod(IRoutine routine) {
            foreach (var p in routine.Parameters) {
                var globalMethod = UnitType.StartClassMethodDefinition(routine.Name);
                globalMethod.Parameters = p;
                globalMethod.ReturnType = KnownTypeIds.NoType;

                globalMethod.DefineMethodBody();
                CurrentMethod.Push(globalMethod);

                var mainMethod = CurrentMethod.Pop();
                mainMethod.FinishMethod();
            }
        }

        private void LogError(uint messageNumber)
            => LogSource.LogError(messageNumber);

        /// <summary>
        ///     create a new assembly reference
        /// </summary>
        /// <returns></returns>
        public IAssemblyReference CreateAssemblyReference()
            => Builder.CreateAssemblyReference();

        /// <summary>
        ///     visit a constant value
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ConstantValue element) {
            if (CurrentMethod.Count < 1)
                return;

            var method = CurrentMethod.Peek();

            if (element.Kind == ConstantValueKind.QuotedString) {
                var stringValue = element.TypeInfo as IStringValue;
                method.LoadConstantString(stringValue.AsUnicodeString);
            }
        }

    }
}
