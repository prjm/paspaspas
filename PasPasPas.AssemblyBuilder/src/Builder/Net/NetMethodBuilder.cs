#nullable disable
using System;
using System.Reflection.Emit;
using PasPasPas.AssemblyBuilder.Builder.Definitions;
using PasPasPas.Globals.CodeGen;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.AssemblyBuilder.Builder.Net {

    /// <summary>
    ///     net method builder encapsulation
    /// </summary>
    public class NetMethodBuilder : IMethodBuilder {

        /// <summary>
        ///     parameters
        /// </summary>
        public IRoutine Parameters { get; set; }

        /// <summary>
        ///     internal method builder
        /// </summary>
        private MethodBuilder InternalBuilder { get; }

        /// <summary>
        ///     generate the method body
        /// </summary>
        private ILGenerator Generator { get; set; }

        /// <summary>
        ///     type mapper
        /// </summary>
        private TypeMapper Mapper { get; }

        /// <summary>
        ///     create a new method builder
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="mapper">type mapper</param>
        public NetMethodBuilder(MethodBuilder builder, TypeMapper mapper) {
            InternalBuilder = builder;
            Mapper = mapper;
        }

        /// <summary>
        ///     set the return type
        /// </summary>
        public ITypeDefinition ReturnType { get; set; }

        /// <summary>
        ///     define the method body
        /// </summary>
        public void DefineMethodBody() {
            Generator = InternalBuilder.GetILGenerator();

            if (Parameters == default)
                return;

            foreach (var instruction in Parameters.Code) {

                Generator.Emit(OpCodes.Nop);

                switch (instruction.Id) {

                    case OpCodeId.Call:
                        EmitCall(instruction);
                        break;


                    default:
                        throw new InvalidOperationException();

                }

            }
        }

        private void EmitCall(Globals.CodeGen.OpCode instruction) {
            var c = Type.GetType("System.Console, mscorlib");
            var m = c.GetMethod("WriteLine", Array.Empty<Type>());
            Generator.Emit(OpCodes.Call, m);
        }

        /// <summary>
        ///     finish the method definition
        /// </summary>
        public void FinishMethod() {
            if (ReturnType is INoType) {
                Generator.Emit(OpCodes.Ret);
                return;
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        ///     load a constant string
        /// </summary>
        /// <param name="value"></param>
        public void LoadConstantString(string value)
            => Generator.Emit(OpCodes.Ldstr, value);

    }
}