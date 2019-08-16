using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using PasPasPas.Globals.Parsing;

namespace PasPasPas.Parsing.SyntaxTree.Standard {

    /// <summary>
    ///     namespace name
    /// </summary>
    public class NamespaceNameSymbol : VariableLengthSyntaxTreeBase<SyntaxPartBase> {

        /// <summary>
        ///     create a new namespace name
        /// </summary>
        /// <param name="items"></param>
        /// <param name="comma"></param>
        public NamespaceNameSymbol(ImmutableArray<SyntaxPartBase> items, Terminal comma) : base(items)
            => Comma = comma;

        /// <summary>
        ///     unit name
        /// </summary>
        public string Name
            => Items == null || Items.Length < 1 ? null : IdentifierValue(Items[Items.Length - 1]);

        /// <summary>
        ///     namespace name
        /// </summary>
        public IEnumerable<string> Namespace {
            get {
                if (Items == null || Items.Length < 2)
                    yield break;

                for (var i = 0; i <= Items.Length - 2; i++) {
                    var part = Items[i];
                    if (!(part is IdentifierSymbol))
                        continue;
                    yield return IdentifierValue(part);
                }
            }
        }

        /// <summary>
        ///     complete name
        /// </summary>
        public string CompleteName {
            get {
                StringBuilder sb = new StringBuilder();
                for (var i = 0; i < Items.Length; i++) {
                    var part = Items[i];
                    if (part is IdentifierSymbol symbol)
                        sb.Append(IdentifierValue(part));
                    else if (part is Terminal terminal)
                        sb.Append(terminal.Value);
                }
                return sb.ToString();
            }
        }

        /// <summary>
        ///     symbol length
        /// </summary>
        public override int Length
            => ItemLength + Comma.GetSymbolLength();

        /// <summary>
        ///     accept visitor
        /// </summary>
        /// <param name="visitor">visitor</param>
        public override void Accept(IStartEndVisitor visitor) {
            visitor.StartVisit(this);
            AcceptPart(this, visitor);
            AcceptPart(this, Comma, visitor);
            visitor.EndVisit(this);
        }

        /// <summary>
        ///     comma
        /// </summary>
        public Terminal Comma { get; }

    }

}