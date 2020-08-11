using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using SharpFloat.FloatingPoint;
using A = Xunit.Assert;

namespace PasPasPasTests.Common {

    /// <summary>
    ///     common assertion library / alias
    /// </summary>
    internal static class Assert {

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="expected">expected value</param>
        /// <param name="actual">test result value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static bool AreEqual<T>(T expected, T actual) {
            A.Equal<T>(expected, actual);
            return true;
        }

        /// <summary>
        ///     check equality of strings
        /// </summary>
        /// <param name="expected">expected value</param>
        /// <param name="actual">test result value</param>
        /// <param name="comp">string comparer to use</param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AreEqual(string expected, string actual, StringComparer? comp = default) {
            if (comp == default)
                comp = StringComparer.OrdinalIgnoreCase;

            A.Equal(expected, actual, comp);
            return true;
        }

        /// <summary>
        ///     check equality of extended float values
        /// </summary>
        /// <param name="expected">expected value</param>
        /// <param name="actual">test result value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void AreEqual(in ExtF80 expected, in ExtF80 actual)
            => A.Equal(expected, actual);

        /// <summary>
        ///     check equality
        /// </summary>
        /// <param name="expected">expected value</param>
        /// <param name="actual">test result value</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void AreEqual(double expected, double actual)
            => A.Equal(expected, actual, 10);

        /// <summary>
        ///     check if an expression is true
        /// </summary>
        /// <param name="value">value to test</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void IsTrue(bool value)
            => A.True(value);

        /// <summary>
        ///     check if an expression is false
        /// </summary>
        /// <param name="value">value to test</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void IsFalse(bool value)
            => A.False(value);

        /// <summary>
        ///     check inequality of two values
        /// </summary>
        /// <param name="notExpected">unexpected result</param>
        /// <param name="actual">test result</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void AreNotEqual<T>(T notExpected, T actual)
            => A.NotEqual<T>(notExpected, actual);

        /// <summary>
        ///     check that an object is not null
        /// </summary>
        /// <param name="value">value to test</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void IsNotNull([NotNull] object? value)
#pragma warning disable CS8777 // Parameter must have a non-null value when exiting.
            => A.NotNull(value);
#pragma warning restore CS8777 // Parameter must have a non-null value when exiting.

        /// <summary>
        ///     check that an object is null
        /// </summary>
        /// <param name="value">value to test</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void IsNull(object? value)
            => A.Null(value);

        /// <summary>
        ///     check that an exception is thrown
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="testCode">test code</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void Throws<T>(Action testCode) where T : Exception
            => A.Throws<T>(testCode);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void AreEqualSequences<T>(IEnumerable<T> expected, IEnumerable<T> actual)
            => A.Equal(expected, actual);

    }
}