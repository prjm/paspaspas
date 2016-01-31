using PasPasPas.Api;
using System.Collections.Generic;

namespace PasPasPas.Internal.Parser.Syntax {

    /// <summary>
    ///     class method definition
    /// </summary>
    public class ClassMethod : SyntaxPartBase {

        /// <summary>
        ///     create a new syntax tree element
        /// </summary>
        /// <param name="informationProvider">current parser state</param>
        public ClassMethod(IParserInformationProvider informationProvider) : base(informationProvider) { }

        /// <summary>
        /// additional directives
        /// </summary>
        public IList<int> Directives { get; }
            = new List<int>();

        /// <summary>
        ///     generic definition
        /// </summary>
        public GenericDefinition GenericDefinition { get; internal set; }

        /// <summary>
        ///     method identifier
        /// </summary>
        public PascalIdentifier Identifier { get; internal set; }

        /// <summary>
        ///     expression for a message method
        /// </summary>
        public ExpressionBase MessageExpression { get; internal set; }

        /// <summary>
        ///     method kind
        /// </summary>
        public int MethodKind { get; internal set; }

        /// <summary>
        ///     formal parameters
        /// </summary>
        public FormalParameters Parameters { get; internal set; }

        /// <summary>
        ///     Result type attributes
        /// </summary>
        public UserAttributes ResultAttributes { get; internal set; }

        /// <summary>
        ///     parse a type specification
        /// </summary>
        public TypeSpecification ResultType { get; internal set; }

        /// <summary>
        ///     format method declaration
        /// </summary>
        /// <param name="result"></param>
        public override void ToFormatter(PascalFormatter result) {
            switch (MethodKind) {

                case PascalToken.Function:
                    result.Keyword("function");
                    break;

                case PascalToken.Procedure:
                    result.Keyword("procedure");
                    break;

                case PascalToken.Constructor:
                    result.Keyword("constructor");
                    break;

                case PascalToken.Destructor:
                    result.Keyword("destructor");
                    break;

            }

            result.Space();
            result.Identifier(Identifier.Value);

            if (GenericDefinition != null)
                GenericDefinition.ToFormatter(result);

            if (Parameters != null) {
                result.Punct("(");
                Parameters.ToFormatter(result);
                result.Punct(")");
            }

            if (ResultType != null) {
                result.Punct(":");
                result.Space();

                if (ResultAttributes.Count > 0) {
                    ResultAttributes.ToFormatter(result);
                    result.Space();

                }


                ResultType.ToFormatter(result);
            }

            result.Punct(";");

            foreach (var directive in Directives) {
                result.Space();
                switch (directive) {
                    case PascalToken.Reintroduce:
                        result.Keyword("reintroduce");
                        break;

                    case PascalToken.Overload:
                        result.Keyword("overload");
                        break;

                    case PascalToken.Message:
                        result.Keyword("message");
                        result.Space();
                        MessageExpression.ToFormatter(result);
                        break;

                    case PascalToken.Static:
                        result.Keyword("static");
                        break;

                    case PascalToken.Dynamic:
                        result.Keyword("dynamic");
                        break;

                    case PascalToken.Override:
                        result.Keyword("override");
                        break;

                    case PascalToken.Virtual:
                        result.Keyword("virtual");
                        break;

                    case PascalToken.Final:
                        result.Keyword("final");
                        break;

                    case PascalToken.Inline:
                        result.Keyword("inline");
                        break;

                    case PascalToken.Assembler:
                        result.Keyword("assembler");
                        break;

                    case PascalToken.Cdecl:
                        result.Keyword("cdecl");
                        break;

                    case PascalToken.Pascal:
                        result.Keyword("pascal");
                        break;

                    case PascalToken.Register:
                        result.Keyword("register");
                        break;

                    case PascalToken.Stdcall:
                        result.Keyword("stdcall");
                        break;

                    case PascalToken.Safecall:
                        result.Keyword("safecall");
                        break;

                    case PascalToken.Export:
                        result.Keyword("export");
                        break;


                    case PascalToken.Far:
                        result.Keyword("far");
                        break;


                    case PascalToken.Local:
                        result.Keyword("local");
                        break;


                    case PascalToken.Near:
                        result.Keyword("near");
                        break;




                };
                result.Punct(";");
            }
        }
    }
}