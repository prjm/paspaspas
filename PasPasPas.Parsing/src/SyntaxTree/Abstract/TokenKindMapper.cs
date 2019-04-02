using System;
using PasPasPas.Globals.Types;
using PasPasPas.Parsing.SyntaxTree.Standard;

namespace PasPasPas.Parsing.SyntaxTree.Abstract {

    /// <summary>
    ///     maps token kinds to enums
    /// </summary>
    public static class TokenKindMapper {

        /// <summary>
        ///     convert expression kid
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static ExpressionKind ForExpression(int kind) {

            switch (kind) {
                case TokenKind.LessThen:
                    return ExpressionKind.LessThen;

                case TokenKind.LessThenEquals:
                    return ExpressionKind.LessThenEquals;

                case TokenKind.GreaterThen:
                    return ExpressionKind.GreaterThen;

                case TokenKind.GreaterThenEquals:
                    return ExpressionKind.GreaterThenEquals;

                case TokenKind.NotEquals:
                    return ExpressionKind.NotEquals;

                case TokenKind.EqualsSign:
                    return ExpressionKind.EqualsSign;

                case TokenKind.In:
                    return ExpressionKind.In;

                case TokenKind.As:
                    return ExpressionKind.As;

                case TokenKind.Plus:
                    return ExpressionKind.Plus;

                case TokenKind.Minus:
                    return ExpressionKind.Minus;

                case TokenKind.Or:
                    return ExpressionKind.Or;

                case TokenKind.Xor:
                    return ExpressionKind.Xor;

                case TokenKind.Div:
                    return ExpressionKind.Div;

                case TokenKind.Times:
                    return ExpressionKind.Times;

                case TokenKind.Slash:
                    return ExpressionKind.Slash;

                case TokenKind.Mod:
                    return ExpressionKind.Mod;

                case TokenKind.And:
                    return ExpressionKind.And;

                case TokenKind.Shl:
                    return ExpressionKind.Shl;

                case TokenKind.Shr:
                    return ExpressionKind.Shr;

                case TokenKind.Is:
                    return ExpressionKind.Is;

                default:
                    return ExpressionKind.Undefined;
            }

        }


        /// <summary>
        ///     method kind
        /// </summary>
        /// <param name="methodKind"></param>
        /// <returns></returns>
        public static ProcedureKind MapMethodKind(int methodKind) {
            switch (methodKind) {
                case TokenKind.Function:
                    return ProcedureKind.Function;
                case TokenKind.Procedure:
                    return ProcedureKind.Procedure;
                case TokenKind.Constructor:
                    return ProcedureKind.Constructor;
                case TokenKind.Destructor:
                    return ProcedureKind.Destructor;
                case TokenKind.Operator:
                    return ProcedureKind.Operator;
                default:
                    return ProcedureKind.Unknown;
            }
        }


        /// <summary>
        ///     map accessor lind
        /// </summary>
        /// <param name="kind"></param>
        /// <returns>mapped kind</returns>
        public static StructurePropertyAccessorKind ForPropertyAccessor(int kind) {

            switch (kind) {

                case TokenKind.Read:
                    return StructurePropertyAccessorKind.Read;

                case TokenKind.Write:
                    return StructurePropertyAccessorKind.Write;

                case TokenKind.Add:
                    return StructurePropertyAccessorKind.Add;

                case TokenKind.Remove:
                    return StructurePropertyAccessorKind.Remove;

                default:
                    return StructurePropertyAccessorKind.Remove;

            }
        }


        /// <summary>
        ///     map a constraint kind
        /// </summary>
        /// <returns></returns>
        public static GenericConstraintKind ForGenericConstraint(ConstrainedGenericSymbol constraint) {

            if (constraint.RecordConstraint)
                return GenericConstraintKind.Record;
            else if (constraint.ClassConstraint)
                return GenericConstraintKind.Class;
            else if (constraint.ConstructorConstraint)
                return GenericConstraintKind.Constructor;
            else if (constraint.ConstraintIdentifier != null)
                return GenericConstraintKind.Identifier;
            else
                return GenericConstraintKind.Unknown;
        }

        /// <summary>
        ///     convert a string type kind
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static MetaTypeKind ForMetaType(int kind) {
            switch (kind) {
                case TokenKind.StringKeyword:
                    return MetaTypeKind.StringType;
                case TokenKind.AnsiString:
                    return MetaTypeKind.AnsiString;
                case TokenKind.ShortString:
                    return MetaTypeKind.ShortString;
                case TokenKind.WideString:
                    return MetaTypeKind.WideString;
                case TokenKind.UnicodeString:
                    return MetaTypeKind.UnicodeString;
                default:
                    return MetaTypeKind.Undefined;
            }
        }

        /// <summary>
        ///     map directive kind
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static MethodDirectiveSpecifierKind ForMethodDirective(int kind) {
            switch (kind) {

                case TokenKind.Name:
                    return MethodDirectiveSpecifierKind.Name;

                case TokenKind.Index:
                    return MethodDirectiveSpecifierKind.Index;

                case TokenKind.Delayed:
                    return MethodDirectiveSpecifierKind.Delayed;

                case TokenKind.Dependency:
                    return MethodDirectiveSpecifierKind.Dependency;

                default:
                    return MethodDirectiveSpecifierKind.Unknown;
            }
        }

        /// <summary>
        ///     map visibility
        /// </summary>
        /// <param name="visibility"></param>
        /// <param name="strict"></param>
        /// <returns></returns>
        public static MemberVisibility ForVisibility(int visibility, bool strict) {

            switch (visibility) {
                case TokenKind.Private:
                    return strict ? MemberVisibility.StrictPrivate : MemberVisibility.Private;

                case TokenKind.Protected:
                    return strict ? MemberVisibility.StrictProtected : MemberVisibility.Protected;

                case TokenKind.Public:
                    return MemberVisibility.Public;

                case TokenKind.Published:
                    return MemberVisibility.Published;

                case TokenKind.Automated:
                    return MemberVisibility.Automated;

            }

            return MemberVisibility.Undefined;
        }

        /// <summary>
        ///     map parameter reference kind
        /// </summary>
        /// <param name="parameterType"></param>
        /// <returns></returns>
        public static ParameterReferenceKind ForParameterReferenceKind(int parameterType) {
            switch (parameterType) {
                case TokenKind.Const:
                    return ParameterReferenceKind.Const;
                case TokenKind.Var:
                    return ParameterReferenceKind.Var;
                case TokenKind.Out:
                    return ParameterReferenceKind.Out;
                default:
                    return ParameterReferenceKind.Undefined;
            }
        }

        /// <summary>
        ///     map token kind to method kind
        /// </summary>
        /// <param name="kind"></param>
        /// <returns></returns>
        public static StructureMethodResolutionKind ForMethodResolutionKind(int kind) {
            switch (kind) {
                case TokenKind.Function:
                    return StructureMethodResolutionKind.Function;

                case TokenKind.Procedure:
                    return StructureMethodResolutionKind.Procedure;

                default:
                    return StructureMethodResolutionKind.Undefined;

            }
        }

        /// <summary>
        ///     map asm byte pointer kind
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ExpressionKind ForAsmBytePointerKind(string value) {

            if (string.IsNullOrWhiteSpace(value))
                return ExpressionKind.Undefined;

            if (value.Equals("byte", StringComparison.OrdinalIgnoreCase))
                return ExpressionKind.AsmBytePointerByte;
            else if (value.Equals("word", StringComparison.OrdinalIgnoreCase))
                return ExpressionKind.AsmBytePointerWord;
            else if (value.Equals("dword", StringComparison.OrdinalIgnoreCase))
                return ExpressionKind.AsmBytePointerDWord;
            else if (value.Equals("qword", StringComparison.OrdinalIgnoreCase))
                return ExpressionKind.AsmBytePointerQWord;
            else if (value.Equals("tbyte", StringComparison.OrdinalIgnoreCase))
                return ExpressionKind.AsmBytePointerTByte;

            return ExpressionKind.Undefined;
        }


    }
}
