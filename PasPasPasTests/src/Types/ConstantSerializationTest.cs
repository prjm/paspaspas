﻿using PasPasPas.Globals.Types;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test the serialization of constants
    /// </summary>
    public class ConstantSerializationTest : SerializationTest {

        /// <summary>
        ///     test unit meta data
        /// </summary>
        [TestMethod]
        public void TestUnitMetadata() {
            void tester(IUnitType u) =>
                Assert.AreEqual("a", u.Name);
            var prg = "unit a; interface implementation end.";
            TestUnitSerialization(prg, tester);
        }

        /// <summary>
        ///     test integer types
        /// </summary>
        [TestMethod]
        public void TestIntegerTypes() {
            //AssertSerializedConstant("A = 5", GetIntegerValue((short)5));
        }
    }
}
