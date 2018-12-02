using System;
using Xunit;

namespace PasPasPasTests.Common {

    [AttributeUsage(AttributeTargets.Method)]
    public class TestMethodAttribute : FactAttribute {

    }
}
