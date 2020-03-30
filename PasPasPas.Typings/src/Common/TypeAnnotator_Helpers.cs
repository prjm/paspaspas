using PasPasPas.Globals.Parsing;

namespace PasPasPas.Typings.Common {

    public partial class TypeAnnotator {

        private void MarkWithErrorType(ITypedSyntaxPart node)
            => node.TypeInfo = TypeRegistry.SystemUnit.ErrorType;

        private ITypeSymbol GetTypeRefence(ITypedSyntaxPart syntaxNode) {
            if (syntaxNode != default && syntaxNode.TypeInfo != null)
                return syntaxNode.TypeInfo;

            return TypeRegistry.SystemUnit.ErrorType;
        }

    }
}
