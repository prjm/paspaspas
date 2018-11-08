﻿using System;
using System.Collections.Generic;
using SharpFloat.FloatingPoint;
using A = Xunit.Assert;

namespace PasPasPasTests.Common {

    public static class Assert {

        public static void AreEqual(object expected, object actual)
            => A.Equal(expected, actual);

        public static void AreEqual(string expected, string actual, StringComparer comp)
            => A.Equal<string>(expected, actual, comp);

        public static void AreEqual(in ExtF80 expected, in ExtF80 actual)
            => A.Equal(expected, actual);

        public static void AreEqual(double expected, double actual)
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

        internal static void AreEqualSequences<T>(IEnumerable<T> expected, IEnumerable<T> actual)
            => A.Equal(expected, actual);

    }
}