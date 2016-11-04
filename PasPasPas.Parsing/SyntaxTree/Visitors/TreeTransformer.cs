using System.Collections.Generic;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Standard;
using PasPasPas.Parsing.Parser;

namespace PasPasPas.Parsing.SyntaxTree.Visitors {

    /// <summary>
    ///     convert a concrete syntax tree to an abstract one
    /// </summary>
    public class TreeTransformer : SyntaxPartVisitorBase<TreeTransformerOptions> {

        #region Unit

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

        #endregion
        #region Library

        private void BeginVisitItem(Library library, TreeTransformerOptions parameter) {
            var result = CreateTreeNode<CompilationUnit>(null, library);
            result.FileType = CompilationUnitType.Library;
            result.UnitName = ExtractSymbolName(result, library.LibraryName);
            result.Hints = ExtractHints(result, library.Hints);
            result.FilePath = library.FilePath;
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnit = result;
        }

        private void EndVisitItem(Library library, TreeTransformerOptions parameter) {
            parameter.CurrentUnit = null;
        }

        #endregion
        #region Program

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

        private void EndVisitItem(Program program, TreeTransformerOptions parameter) {
            parameter.CurrentUnit = null;
        }

        #endregion
        #region Package

        private void BeginVisitItem(Package package, TreeTransformerOptions parameter) {
            var result = CreateTreeNode<CompilationUnit>(null, package);
            result.FileType = CompilationUnitType.Package;
            result.UnitName = ExtractSymbolName(result, package.PackageName);
            result.FilePath = package.FilePath;
            parameter.Project.Add(result, parameter.LogSource);
            parameter.CurrentUnit = result;
        }

        private void EndVisitItem(Package package, TreeTransformerOptions parameter) {
            parameter.CurrentUnit = null;
        }

        #endregion
        #region UnitInterface

        private void BeginVisitItem(UnitInterface unitInterface, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Interface;
            parameter.BeginDeclare(parameter.CurrentUnit.InterfaceSymbols);
        }


        private void EndVisitItem(UnitInterface unitInterface, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Unknown;
            parameter.EndDeclare(parameter.CurrentUnit.InterfaceSymbols);
        }

        #endregion
        #region UnitImplementation

        private void BeginVisitItem(UnitImplementation unitImplementation, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Implementation;
            parameter.BeginDeclare(parameter.CurrentUnit.ImplementationSymbols);
        }

        private void EndVisitItem(UnitImplementation unit, TreeTransformerOptions parameter) {
            parameter.CurrentUnitMode = UnitMode.Unknown;
            parameter.EndDeclare(parameter.CurrentUnit.ImplementationSymbols);
        }

        #endregion
        #region ConstSection

        private void BeginVisitItem(ConstSection constSection, TreeTransformerOptions parameter) {
            if (constSection.Kind == TokenKind.Const) {
                parameter.CurrentDeclarationMode = DeclarationMode.Const;
            }
            else if (constSection.Kind == TokenKind.Resourcestring) {
                parameter.CurrentDeclarationMode = DeclarationMode.ResourceString;
            }
        }

        private void EndVisitItem(ConstSection constSection, TreeTransformerOptions parameter) {
            parameter.CurrentDeclarationMode = DeclarationMode.Unknown;
        }

        #endregion
        #region ConstDeclaration

        private void BeginVisitItem(ConstDeclaration constDeclaration, TreeTransformerOptions parameter) {
            var declaration = parameter.Declare<ConstantDeclaration>(constDeclaration);
            declaration.Name = ExtractSymbolName(declaration, constDeclaration.Identifier);
            declaration.Mode = parameter.CurrentDeclarationMode;
            declaration.Attributes = ExtractAttributes(declaration, constDeclaration.Attributes, parameter.CurrentUnit);
            declaration.Hints = ExtractHints(constDeclaration, constDeclaration.Hint);
            parameter.CompleteDeclaration(declaration);
            parameter.BeginExpression(declaration);
        }

        private void EndVisitItem(ConstDeclaration constDeclaration, TreeTransformerOptions parameter) {
            parameter.EndExpression();
        }

        #endregion
        #region ConstantExpression

        private void BeginVisitItem(ConstantExpression constExpression, TreeTransformerOptions parameter) {

            if (constExpression.IsArrayConstant) {
                parameter.DefineExpressionValue<ArrayConstant>(constExpression);
            }

            if (constExpression.IsRecordConstant) {
                parameter.DefineExpressionValue<RecordConstant>(constExpression);
            }
        }

        private void EndVisitItem(ConstantExpression constExpression, TreeTransformerOptions parameter) {

            if (constExpression.IsArrayConstant || constExpression.IsRecordConstant) {
                parameter.CompleteExpression();
            }
        }

        #endregion
        #region RecordConstantExpression


        private void BeginVisitItem(RecordConstantExpression constExpression, TreeTransformerOptions parameter) {
            var expression = parameter.DefineExpressionValue<RecordConstantItem>(constExpression);
            expression.Name = ExtractSymbolName(constExpression, constExpression.Name);
        }

        private void EndVisitItem(RecordConstantExpression constExpression, TreeTransformerOptions parameter) {
            parameter.CompleteExpression();
        }

        #endregion

        /*
        private void BeginVisitItem(Expression simpleExpression, TreeTransformerOptions parameter) {
            if (simpleExpression.LeftOperand != null && simpleExpression.RightOperand != null) {
                var currentExpression = parameter.CurrentExpressionScope.Peek();
                var comparison = CreateLeafNode<ComparisonExpression>(currentExpression, simpleExpression);
                comparison.Kind = ComparisonExpression.ConvertKind(simpleExpression.Kind);
                currentExpression.Value = comparison;
            }
        }

        private void BeginVisitChildItem(Expression simpleExpression, TreeTransformerOptions parameter, ISyntaxPart child) {
            if (simpleExpression.LeftOperand != null && simpleExpression.RightOperand != null) {
                var currentExpression = (ComparisonExpression)parameter.CurrentExpressionScope.Peek().Value;
                var operand = CreateLeafNode<ExpressionOperand>(currentExpression, child);

                if (currentExpression.LeftOperand == null) {
                    currentExpression.LeftOperand = operand;
                }
                else if (currentExpression.RightOperand == null) {
                    currentExpression.RightOperand = operand;
                }
                parameter.CurrentExpressionScope.Push(operand);
            }
        }

        private void EndVisitChildItem(Expression simpleExpression, TreeTransformerOptions parameter, ISyntaxPart child) {
            if (simpleExpression.LeftOperand != null && simpleExpression.RightOperand != null) {
                parameter.CurrentExpressionScope.Pop();
            }
        }

        */

        private void BeginVisitItem(Factor factor, TreeTransformerOptions parameter) {
            if (factor.IsNil) {
                var value = CreateLeafNode<ConstantValue>(parameter.LastExpression, factor);
                value.Kind = ConstantValueKind.Nil;
                parameter.DefineExpressionValue(value);
                return;
            }
        }

        private IEnumerable<SymbolAttribute> ExtractAttributes(object parent, UserAttributes attributes, CompilationUnit parentUnit) {
            if (attributes == null || attributes.PartList.Count < 1)
                return EmptyCollection<SymbolAttribute>.Instance;

            var result = new List<SymbolAttribute>();

            foreach (var part in attributes.Parts) {
                var attribute = part as UserAttributeDefinition;
                var isAssemblyAttribute = false;

                if (attribute == null) {
                    var assemblyAttribute = part as AssemblyAttributeDeclaration;
                    if (assemblyAttribute == null)
                        continue;

                    attribute = assemblyAttribute.Attribute;
                    isAssemblyAttribute = true;
                }

                var userAttribute = CreateLeafNode<SymbolAttribute>(parent, attribute);
                userAttribute.Name = ExtractSymbolName(userAttribute, attribute.Name);

                if (!isAssemblyAttribute)
                    result.Add(userAttribute);
                else
                    parentUnit.AddAssemblyAttribute(userAttribute);
            }

            return result;
        }

        private void BeginVisitItem(UsesClause unit, TreeTransformerOptions parameter) {
            if (unit.UsesList == null)
                return;

            foreach (var part in unit.UsesList.Parts) {
                var name = part as NamespaceName;
                if (name == null)
                    continue;

                var unitName = CreateLeafNode<RequiredUnitName>(unit, part);
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

        private static SymbolName ExtractSymbolName(object parent, Identifier name) {
            var result = CreateLeafNode<SymbolName>(parent, name);
            result.Name = name.FirstTerminalToken.Value;
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

            if (hints == null || hints.PartList.Count < 1)
                return result;

            foreach (var part in hints.Parts) {
                var hint = part as HintingInformation;
                if (hint == null) continue;
                result.SymbolIsDeprecated = result.SymbolIsDeprecated || hint.Deprecated;
                result.DeprecatedInformation = (result.DeprecatedInformation ?? string.Empty) + hint.DeprecatedComment?.UnquotedValue;
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
        ///     start visiting a child item
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="visitorParameter"></param>
        /// <param name="child"></param>
        public override void BeginVisitChild(ISyntaxPart parent, TreeTransformerOptions visitorParameter, ISyntaxPart child) {
            dynamic part = parent;
            BeginVisitChildItem(part, visitorParameter, child); ;
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
        ///     end visiting a child
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="visitorParameter"></param>
        /// <param name="child"></param>
        public override void EndVisitChild(ISyntaxPart parent, TreeTransformerOptions visitorParameter, ISyntaxPart child) {
            dynamic part = parent;
            EndVisitChildItem(part, visitorParameter, child);
        }



        private void BeginVisitItem(ISyntaxPart part, TreeTransformerOptions parameter) {
            //..
        }

        private void BeginVisitChildItem(ISyntaxPart part, TreeTransformerOptions parameter, ISyntaxPart child) {
            //..
        }


        private void EndVisitItem(ISyntaxPart part, TreeTransformerOptions parameter) {
            //..
        }

        private void EndVisitChildItem(ISyntaxPart part, TreeTransformerOptions parameter, ISyntaxPart child) {
            //..
        }


    }
}
