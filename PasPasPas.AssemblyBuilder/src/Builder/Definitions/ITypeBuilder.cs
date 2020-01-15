using PasPasPas.Globals.Runtime;

namespace PasPasPas.AssemblyBuilder.Builder.Definitions {

    /// <summary>
    ///     interface for a type builder
    /// </summary>
    public interface ITypeBuilder {

        /// <summary>
        ///     create the type
        /// </summary>
        void CreateType();

        /// <summary>
        ///     start a method definition
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        IMethodBuilder StartClassMethodDefinition(string name);

        /// <summary>
        ///     define a class variable
        /// </summary>
        /// <param name="symbolName">variable name</param>
        /// <param name="typeInfo">type info</param>
        void DefineClassVariable(string symbolName, IOldTypeReference typeInfo);
    }
}