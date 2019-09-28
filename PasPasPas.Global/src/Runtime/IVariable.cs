using PasPasPas.Globals.Types;

namespace PasPasPas.Globals.Runtime {

    /// <summary>
    ///     interface for variables
    /// </summary>
    public interface IVariable : IRefSymbol {
        ITypeReference SymbolType { get; set; }
    }
}