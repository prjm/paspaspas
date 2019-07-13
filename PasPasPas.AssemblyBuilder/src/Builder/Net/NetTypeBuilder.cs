using System.Reflection;
using System.Reflection.Emit;
using PasPasPas.AssemblyBuilder.Builder.Definitions;
using PasPasPas.Globals.Runtime;

namespace PasPasPas.AssemblyBuilder.Builder.Net {

    /// <summary>
    ///     type builder for .net assemblies
    /// </summary>
    public class NetTypeBuilder : ITypeBuilder {

        /// <summary>
        ///     creates a new type builder
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="mapper">type mapper</param>
        public NetTypeBuilder(TypeBuilder builder, TypeMapper mapper) {
            InternalBuilder = builder;
            Mapper = mapper;
        }

        /// <summary>
        ///     type builder
        /// </summary>
        private TypeBuilder InternalBuilder { get; }

        /// <summary>
        ///     type mapper
        /// </summary>
        public TypeMapper Mapper { get; }

        /// <summary>
        ///     create the new type
        /// </summary>
        public void CreateType()
            => InternalBuilder.CreateType();

        /// <summary>
        ///     start a method definition
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public IMethodBuilder StartClassMethodDefinition(string name) {
            var builder = InternalBuilder.DefineMethod(name, MethodAttributes.HideBySig | MethodAttributes.Static);
            return new NetMethodBuilder(builder, Mapper);
        }

        /// <summary>
        ///     define a class variable
        /// </summary>
        /// <param name="symbolName"></param>
        /// <param name="typeInfo"></param>
        public void DefineClassVariable(string symbolName, ITypeReference typeInfo)
            => InternalBuilder.DefineField(symbolName, Mapper.Map(typeInfo.TypeId), FieldAttributes.Static);

    }
}