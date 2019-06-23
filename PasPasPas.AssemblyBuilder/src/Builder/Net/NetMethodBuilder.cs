using System;
using System.Reflection.Emit;
using PasPasPas.AssemblyBuilder.Builder.Definitions;
using PasPasPas.Globals.Types;

namespace PasPasPas.AssemblyBuilder.Builder.Net {

    /// <summary>
    ///     net method builder encapsulation
    /// </summary>
    public class NetMethodBuilder : IMethodBuilder {

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
        public int ReturnType { get; set; }
        /// <summary>
        ///     define the method body
        /// </summary>
        public void DefineMethodBody()
            => Generator = InternalBuilder.GetILGenerator();

        /// <summary>
        ///     finish the method definition
        /// </summary>
        public void FinishMethod() {
            if (ReturnType == KnownTypeIds.NoType) {
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