using System.Collections.Generic;
using PasPasPas.AssemblyBuilder.Builder.Definitions;
using PasPasPas.AssemblyBuilder.Builder.Net;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Visitors;

namespace PasPasPas.AssemblyBuilder.Builder {

    /// <summary>
    ///     visitor to build an assembly
    /// </summary>
    public class ProjectAssemblyBuilder :

        IStartVisitor<ProjectItemCollection>, IEndVisitor<ProjectItemCollection>,
        IStartVisitor<CompilationUnit>, IEndVisitor<CompilationUnit>,
        IStartVisitor<BlockOfStatements>, IEndVisitor<BlockOfStatements>,
        IStartVisitor<VariableDeclaration>,
        IEndVisitor<ConstantValue> {

        private readonly IStartEndVisitor visitor;

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
        private CompilationUnit CurrentUnit { get; set; }

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
            visitor = new ChildVisitor(this);
            Environment = environment;
            LogSource = environment.Log.CreateLogSource(MessageGroups.AssemblyBuilder);
            Builder = new NetAssemblyBuilder(environment.TypeRegistry);
            CurrentMethod = new Stack<IMethodBuilder>();
        }

        /// <summary>
        ///     helper object
        /// </summary>
        /// <returns></returns>
        public IStartEndVisitor AsVisitor()
            => visitor;

        /// <summary>
        ///     start visiting a project
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ProjectItemCollection element) {
            var projectName = element.ProjectName;

            if (string.IsNullOrWhiteSpace(projectName)) {
                LogError(BuilderErrorMessages.UndefinedProjectName);
                projectName = element[element.Count - 1].SymbolName;
            }
            Builder.StartAssembly(projectName);
        }

        private void LogError(uint messageNumber)
            => LogSource.LogError(messageNumber);

        /// <summary>
        ///     end visiting a project
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ProjectItemCollection element)
            => Builder.EndAssembly();

        /// <summary>
        ///     create a new assembly reference
        /// </summary>
        /// <returns></returns>
        public IAssemblyReference CreateAssemblyReference()
            => Builder.CreateAssemblyReference();

        /// <summary>
        ///     start visiting a block of statements
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(BlockOfStatements element) {
            if (CurrentUnit.FileType == CompilationUnitType.Program && CurrentMethod.Count < 1) {
                var mainMethod = UnitType.StartClassMethodDefinition("Main");
                mainMethod.ReturnType = KnownTypeIds.NoType;
                mainMethod.DefineMethodBody();
                CurrentMethod.Push(mainMethod);
            }
        }

        /// <summary>
        ///     end visiting a block of statements
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(BlockOfStatements element) {
            if (CurrentUnit.FileType == CompilationUnitType.Program && CurrentMethod.Count == 1) {
                var mainMethod = CurrentMethod.Pop();
                mainMethod.FinishMethod();
            }
        }

        /// <summary>
        ///     start visiting a unit
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(CompilationUnit element) {
            CurrentUnit = element;
            UnitType = Builder.StartUnit(element.SymbolName);
        }

        /// <summary>
        ///     end visiting unit
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(CompilationUnit element) {
            UnitType.CreateType();
            CurrentUnit = default;
            UnitType = default;
        }

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

        /// <summary>
        ///     define a variable
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(VariableDeclaration element) {
            if (CurrentUnit.FileType == CompilationUnitType.Program && CurrentMethod.Count < 1) {

                foreach (var name in element.Names) {
                    UnitType.DefineClassVariable(name.SymbolName, element.TypeInfo);
                }
            }
        }
    }
}
