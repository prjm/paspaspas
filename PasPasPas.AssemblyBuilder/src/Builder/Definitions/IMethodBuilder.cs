#nullable disable
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.AssemblyBuilder.Builder.Definitions {

    /// <summary>
    ///     method builder
    /// </summary>
    public interface IMethodBuilder {

        /// <summary>
        ///     set the return type
        /// </summary>
        ITypeDefinition ReturnType { get; set; }

        /// <summary>
        ///     parameters
        /// </summary>
        IRoutine Parameters { get; set; }

        /// <summary>
        ///     define a method body
        /// </summary>
        void DefineMethodBody();

        /// <summary>
        ///     finish the method
        /// </summary>
        void FinishMethod();

        /// <summary>
        ///     load a constant string to the working stack
        /// </summary>
        /// <param name="value"></param>
        void LoadConstantString(string value);

    }
}