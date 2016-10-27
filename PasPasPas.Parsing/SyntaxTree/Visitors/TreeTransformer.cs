﻿using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Standard;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     convert a concrete syntax tree to an abstract one
    /// </summary>
    public class TreeTransformer : SyntaxPartVisitorBase<TreeTransformerOptions> {

        /// <summary>
        ///     visit a syntax node
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool BeginVisit(ISyntaxPart syntaxPart, TreeTransformerOptions parameter) {
            dynamic part = syntaxPart;
            BeginVisitItem(part, parameter);
            return true;
        }

        /// <summary>
        ///     end visiting a syntax node
        /// </summary>
        /// <param name="syntaxPart"></param>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public override bool EndVisit(ISyntaxPart syntaxPart, TreeTransformerOptions parameter) {
            dynamic part = syntaxPart;
            EndVisitItem(part, parameter);
            return true;
        }

        /// <summary>
        ///     visit a unit
        /// </summary>
        /// <param name="unit"></param>
        /// <param name="parameter"></param>
        private void BeginVisitItem(Unit unit, TreeTransformerOptions parameter) {
            var result = CreateTreeNode<CompilationUnit>(null, unit);
            result.FileType = CompilationUnitType.Unit;
            result.UnitName = ExtractSymbolName(result, unit.UnitName);
            result.Hints = ExtractHints(result, unit.Hints);
            result.FilePath = unit.FilePath;
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnit = result;
        }

        private void EndVisitItem(Unit unit, TreeTransformerOptions parameter) {
            parameter.CurrentUnit = null;
        }

        /// <summary>
        ///     visit a library
        /// </summary>
        /// <param name="library"></param>
        /// <param name="parameter"></param>
        private void BeginVisitItem(Library library, TreeTransformerOptions parameter) {
            var result = CreateTreeNode<CompilationUnit>(null, library);
            result.FileType = CompilationUnitType.Library;
            result.UnitName = ExtractSymbolName(result, library.LibraryName);
            result.Hints = ExtractHints(result, library.Hints);
            result.FilePath = library.FilePath;
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnit = result;
        }

        private void EndVisitItem(Library unit, TreeTransformerOptions parameter) {
            parameter.CurrentUnit = null;
        }

        /// <summary>
        ///     visit a program
        /// </summary>
        /// <param name="program"></param>
        /// <param name="parameter"></param>
        private void BeginVisitItem(Program program, TreeTransformerOptions parameter) {
            var result = CreateTreeNode<CompilationUnit>(null, program);
            result.FileType = CompilationUnitType.Program;
            result.UnitName = ExtractSymbolName(result, program.ProgramName);
            result.FilePath = program.FilePath;
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnit = result;
        }

        private void EndVisitItem(Program unit, TreeTransformerOptions parameter) {
            parameter.CurrentUnit = null;
        }

        /// <summary>
        ///     visit a package
        /// </summary>
        /// <param name="package"></param>
        /// <param name="parameter"></param>
        private void BeginVisitItem(Package package, TreeTransformerOptions parameter) {
            var result = CreateTreeNode<CompilationUnit>(null, package);
            result.FileType = CompilationUnitType.Package;
            result.UnitName = ExtractSymbolName(result, package.PackageName);
            result.FilePath = package.FilePath;
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnit = result;
        }

        private void EndVisitItem(Package unit, TreeTransformerOptions parameter) {
            parameter.CurrentUnit = null;
        }

        private void BeginVisitItem(UnitInterface unit, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Interface;
        }


        private void EndVisitItem(UnitInterface unit, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Unknown;
        }

        private void BeginVisitItem(UnitImplementation unit, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Implementation;
        }


        private void EndVisitItem(UnitImplementation unit, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Unknown;
        }

        private void BeginVisitItem(UsesClause unit, TreeTransformerOptions parameter) {
            if (unit.UsesList == null)
                return;

            foreach (var part in unit.UsesList.Parts) {
                var name = part as NamespaceName;
                if (name == null)
                    continue;

                var unitName = new UnitName();
                unitName.Name = ExtractSymbolName(unitName, name);
                unitName.Mode = parameter.CurrentUnitMode;
                parameter.CurrentUnit.RequiredUnits.Add(unitName, parameter.LogSource);
            }
        }


        /// <summary>
        ///     extract the name of a symbol
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        private static SymbolName ExtractSymbolName(object parent, NamespaceName name) {
            var result = CreateLeafNode<SymbolName>(parent, name);
            result.Name = name?.Name;
            result.Namespace = name?.Namespace;
            return result;
        }

        /// <summary>
        ///     extract symbol hints
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="hints"></param>
        /// <returns></returns>
        private static SymbolHints ExtractHints(object parent, HintingInformationList hints) {
            var result = CreateLeafNode<SymbolHints>(parent, hints);

            if (hints == null || hints.Parts.Count < 1)
                return result;

            foreach (var part in hints.Parts) {
                var hint = part as HintingInformation;
                if (hint == null) continue;
                result.SymbolIsDeprecated = result.SymbolIsDeprecated || hint.Deprecated;
                result.DeprecratedInformation = (result.DeprecratedInformation ?? string.Empty) + hint.DeprecatedComment?.UnquotedValue;
                result.SymbolInLibrary = result.SymbolInLibrary || hint.Library;
                result.SymbolIsPlatformSpecific = result.SymbolIsPlatformSpecific || hint.Platform;
                result.SymbolIsExperimental = result.SymbolIsExperimental || hint.Experimental;
            }

            return result;
        }

        private static T CreateTreeNode<T>(ISyntaxPart parent, ISyntaxPart element) where T : ISyntaxPart, new() {
            var result = new T();
            result.Parent = parent;
            return result;
        }


        private static T CreateLeafNode<T>(object parent, ISyntaxPart element) where T : new() {
            var result = new T();
            return result;
        }

        private void BeginVisitItem(ISyntaxPart part, TreeTransformerOptions parameter) {
            //..
        }


        private void EndVisitItem(ISyntaxPart part, TreeTransformerOptions parameter) {
            //..
        }


    }
}
