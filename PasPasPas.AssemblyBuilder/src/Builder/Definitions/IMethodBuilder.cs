namespace PasPasPas.AssemblyBuilder.Builder.Definitions {

    /// <summary>
    ///     method builder
    /// </summary>
    public interface IMethodBuilder {

        /// <summary>
        ///     set the return type
        /// </summary>
        int ReturnType { get; set; }

        /// <summary>
        ///     define a method body
        /// </summary>
        void DefineMethodBody();

        /// <summary>
        ///     finish the method
        /// </summary>
        void FinishMethod();
    }
}