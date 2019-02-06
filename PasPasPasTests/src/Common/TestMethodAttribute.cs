using System;

namespace PasPasPasTests.Common {

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TestMethodAttribute : Xunit.FactAttribute {

    }
}
