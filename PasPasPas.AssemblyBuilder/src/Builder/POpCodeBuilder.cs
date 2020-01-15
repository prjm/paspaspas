using System.Collections.Generic;
using System.Collections.Immutable;
using PasPasPas.Globals.CodeGen;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Parsing;
using PasPasPas.Globals.Runtime;
using PasPasPas.Parsing.SyntaxTree.Abstract;
using PasPasPas.Parsing.SyntaxTree.Visitors;
using PasPasPas.Typings.Structured;

namespace PasPasPas.AssemblyBuilder.Builder {

    /// <summary>
    ///     op code builder
    /// </summary>
    public class POpCodeBuilder :
        IEndVisitor<ConstantValue>,
        IEndVisitor<StructuredStatement> {



        /// <summary>
        ///     create a new opcode builder
        /// </summary>
        /// <param name="environment"></param>
        public POpCodeBuilder(ITypedEnvironment environment) {
            Environment = environment;
            visitor = new ChildVisitor(this);
            encoder = new ConstantEncoder(environment);
        }

        /// <summary>
        ///     environment
        /// </summary>
        public ITypedEnvironment Environment { get; }

        /// <summary>
        ///     current routine
        /// </summary>
        public Routine CurrentRoutine { get; private set; }

        private readonly ChildVisitor visitor;

        private readonly ConstantEncoder encoder;

        /// <summary>
        ///     visitor
        /// </summary>
        /// <returns></returns>
        public IStartEndVisitor AsVisitor()
            => visitor;

        private readonly List<OpCode> code
            = new List<OpCode>();

        /// <summary>
        ///     apply this builder
        /// </summary>
        /// <param name="items"></param>
        public void Apply(List<(Routine, BlockOfStatements)> items) {
            foreach (var (r, b) in items) {
                code.Clear();
                CurrentRoutine = r;
                b.Accept(AsVisitor());
                r.Code = ImmutableArray.Create(code.ToArray());
            }
        }

        /// <summary>
        ///     visit a statement
        /// </summary>
        /// <param name="element">statement to check</param>
        public void EndVisit(StructuredStatement element) {
            if (element.Kind == StructuredStatementKind.Assignment) {
                EndVisitAsignment(element);
            }
        }

        private void EndVisitAsignment(StructuredStatement element) {

            // has to be extended later
            if (element.Expressions[0] is SymbolReference r) {
                if (r.SymbolParts.Count == 1) {
                    var p = r.SymbolParts[0].TypeInfo;
                    code.Add(new OpCode(OpCodeId.Store));
                }
            }
        }

        /// <summary>
        ///     visit a constant value
        /// </summary>
        /// <param name="element"></param>
        public void EndVisit(ConstantValue element) {
            var value = element.TypeInfo;

            if (value.ReferenceKind != TypeReferenceKind.ConstantValue)
                return; //??

            code.Add(new OpCode(OpCodeId.LoadConstant, encoder.Encode(value)));
        }
    }
}
