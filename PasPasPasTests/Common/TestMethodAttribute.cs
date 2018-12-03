using System;
using Xunit;

namespace PasPasPasTests.Common {

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class TestMethodAttribute : FactAttribute {

    }
}
