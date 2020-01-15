using PasPasPas.Globals.Runtime;

namespace PasPasPas.AssemblyBuilder.Builder.Definitions {

    /// <summary>
    ///     method builder
    /// </summary>
    public interface IMethodBuilder {

        /// <summary>
        ///     set the return type
        /// </summary>
        int ReturnType { get; set; }
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