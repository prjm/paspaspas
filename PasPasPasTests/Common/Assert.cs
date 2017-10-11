using A = Xunit.Assert;
using System;

namespace PasPasPasTests.Common {

    public static class Assert {

        public static void AreEqual(object expected, object actual, string message = "")
            => A.Equal(expected, actual);

        public static void AreEqual(double expected, double actual, string message = "")
                    => A.Equal(expected, actual, 10);

        public static void IsTrue(bool o)
            => A.True(o);

        public static void IsFalse(bool o)
            => A.False(o);

        public static void AreNotEqual(object notExpected, object actual)
            => A.NotEqual(notExpected, actual);

        public static void IsNotNull(object o)
            => A.NotNull(o);

        public static void IsNull(object o)
            => A.Null(o);

        public static void Throws<T>(Action testCode) where T : Exception
            => A.Throws<T>(testCode);

    }
}