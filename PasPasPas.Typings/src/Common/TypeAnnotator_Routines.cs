﻿using System.Collections.Generic;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Typings.Structured;

namespace PasPasPas.Typings.Common {

    public partial class TypeAnnotator {

        private readonly Stack<IRoutine> currentMethodImplementation
            = new Stack<IRoutine>();


        /// <summary>
        ///     visit a parameter type definition
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ParameterTypeDefinition element) {
            if (element.TypeValue != default && element.TypeValue.TypeInfo != default) {

                if (currentMethodParameters.Count < 1)
                    return;

                var parms = currentMethodParameters.Peek();

                foreach (var name in element.Parameters) {
                    var param = parms.AddParameter(name.Name.CompleteName, element.TypeValue.TypeInfo);
                }
            }
        }

        /// <summary>
        ///     start visiting a method declaration
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(MethodDeclaration element) {
            if (element.Name == null)
                return;

            var method = default(IRoutineGroup);
            var f = RoutineFlags.None;

            if (element is GlobalMethod gm) {
                if (CurrentUnit.InterfaceSymbols != default) {
                    var unitType = CurrentUnitType;
                    method = new RoutineGroup(unitType, element.SymbolName);
                    unitType.Register(method);
                }
            }
            else {
                var v = currentTypeDefinition.Peek();
                var m = element as StructureMethod;
                var classMethod = m?.ClassItem ?? false;
                var genericTypeId = SystemUnit.ErrorType;
                var d = v;
                /*
                if (d is RoutineType rt) {
                    d =
                }
                */

                var typeDef = v != null ? d as StructuredTypeDeclaration : null;

                if (m != default && m.Generics != default && m.Generics.Count > 0) {
                    var functionType = TypeCreator.CreateRoutineType(string.Empty);
                    genericTypeId = functionType;
                }

                if (classMethod)
                    f = f | RoutineFlags.ClassItem;

                method = typeDef.AddOrExtendMethod(element.Name.CompleteName, genericTypeId);
            }

            var parameters = ((RoutineGroup)method).AddParameterGroup(element.Kind, NoType.Reference);
            //currentTypeDefinition.Push((RoutineGroup)method);

            //parameters.IsClassItem = f.IsClassItem();
            currentMethodParameters.Push(parameters);
        }

        /// <summary>
        ///     end visiting a method declaration
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(MethodDeclaration element) {
            if (element.Name == default)
                return;

            var method = currentTypeDefinition.Pop() as RoutineGroup;

            if (element.Kind == RoutineKind.Function) {

                if (currentTypeDefinition.Count < 1)
                    return;

                var v = currentTypeDefinition.Peek();
                var typeDef = v != null ? v as IStructuredType : null;
                var methodParams = currentMethodParameters.Pop();
                /*
                if (element.TypeValue != null && element.TypeValue.TypeInfo != null)
                    methodParams.ResultType = element.TypeValue.TypeInfo;
                else
                    methodParams.ResultType = SystemUnit.ErrorType;
                    */
            }
        }

        /// <summary>
        ///     start visiting the procedure heading marker
        /// </summary>
        /// <param name="element"></param>
        public void StartVisit(ProcedureHeadingMarker element) {
            if (currentMethodImplementation.Count < 1)
                return;

            var routine = currentMethodImplementation.Peek();

            var parameters = routine.Parameters;
            if (parameters != default) {
                foreach (var parameter in parameters) {
                    resolver.AddToScope(parameter.Name, ReferenceKind.RefToParameter, parameter);
                }
            }

            if (routine.RoutineGroup.DefiningType != SystemUnit.UnspecifiedType) {
                var baseTypeDef = routine.RoutineGroup.DefiningType as IStructuredType;

                if (baseTypeDef != default && !routine.IsClassItem())
                    resolver.AddToScope("Self", ReferenceKind.RefToSelf, baseTypeDef.Reference);

                if (baseTypeDef != default && routine.IsClassItem()) {
                    resolver.AddToScope("Self", ReferenceKind.RefToSelfClass, baseTypeDef.Reference);
                }
            }
        }

        /// <summary>
        ///     end visiting a method implementation
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(MethodImplementation element) {
            var isForward = element.Flags.HasFlag(MethodImplementationFlags.ForwardDeclaration);
            var isClassMethod = element.Declaration?.DefiningType != default;

            resolver.CloseScope();

            if (!isClassMethod) {
                var parameters = currentMethodParameters.Pop();
                if (element.TypeValue != null && element.TypeValue.TypeInfo != null)
                    parameters.ResultType = element.TypeValue.TypeInfo;
                else
                    parameters.ResultType = ErrorReference;

            }

            currentMethodImplementation.Pop();
        }

    }
}
