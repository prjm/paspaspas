using System;

namespace PasPasPasTests.Common {

    /// <summary>
    ///     test method attribute wrapper
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TestMethodAttribute : Xunit.FactAttribute {

    }
}
