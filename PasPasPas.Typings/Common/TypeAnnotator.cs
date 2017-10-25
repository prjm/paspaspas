using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Simple;

namespace PasPasPas.Typings.Common {

    /// <summary>
    ///     visitor to annotate typs
    /// </summary>
    public class TypeAnnotator :

        IEndVisitor<ConstantValue> {

        private readonly IStartEndVisitor visitor;
        private readonly ITypedEnvironment environment;

        /// <summary>
        ///     as common visitor
        /// </summary>
        /// <returns></returns>
        public IStartEndVisitor AsVisitor() =>
            visitor;

        /// <summary>
        ///     create a new type annotator
        /// </summary>
        /// <param name="env">typed environment</param>
        public TypeAnnotator(ITypedEnvironment env) {
            visitor = new Visitor(this);
            environment = env;
        }

        /// <summary>
        ///     determine the type of a constant value
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ConstantValue element) =>
            element.TypeInfo = environment.TypeRegistry.GetTypeOrUndef(LiteralValues.GetTypeFor(element.LiteralValue));
    }
}
