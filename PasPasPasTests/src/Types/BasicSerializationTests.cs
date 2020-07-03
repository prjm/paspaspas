using System.IO;
using System.Linq;
using PasPasPas.Globals.CodeGen;
using PasPasPas.Globals.Log;
using PasPasPas.Globals.Runtime;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Typings.Serialization;
using PasPasPas.Typings.Structured;
using PasPasPasTests.Common;
using PK = PasPasPas.Globals.Types.RoutineKind;

namespace PasPasPasTests.Types {

    /// <summary>
    ///     test tag
    /// </summary>
    internal class SampleTag : Tag {

        public const uint TagKind = 0xAFAFE;
        public const uint Data1 = 0xFE00A00;
        public const uint Data2 = 0x3234245;

        public SampleTag(uint data1, uint data2) {
            Point1 = data1;
            Point2 = data2;
        }

        public SampleTag() { }

        public override uint Kind
            => TagKind;

        public uint Point1 { get; set; }
            = 0;

        public uint Point2 { get; set; }
            = 0;

        internal override void WriteData(TypeWriter typeWriter) {
            var v = Point1;
            typeWriter.WriteUint(v);
            v = Point2;
            typeWriter.WriteUint(v);
        }

        internal override void ReadData(uint kind, TypeReader typeReader) {
            Point1 = typeReader.ReadUint();
            Point2 = typeReader.ReadUint();
        }
    }

    /// <summary>
    ///     test type reader / writer
    /// </summary>
    public class BasicSerializationTests : SerializationTest {

        /// <summary>
        ///     read write integer
        /// </summary>
        [TestMethod]
        public void TestWriteReadInteger() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var i = Constants.MagicNumber;
                w.WriteUint(i);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadUint();
                Assert.AreEqual(i, o);
            }
        }

        /// <summary>
        ///     test read an invalid integer
        /// </summary>
        [TestMethod]
        public void TestReadInvalidInteger() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                w.WriteByte(0xA0);
                w.WriteByte(0x0A);
                stream.Seek(0, SeekOrigin.Begin);
                Assert.Throws<UnexpectedEndOfFileException>(() => r.ReadUint());
            }
        }


        /// <summary>
        ///     test read / write a byte
        /// </summary>
        [TestMethod]
        public void TestReadWriteByte() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                byte i = 0xAF;
                w.WriteByte(i);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadByte();
                Assert.AreEqual(i, o);
            }
        }

        /// <summary>
        ///     test read write a long value
        /// </summary>
        [TestMethod]
        public void TestReadWriteLong() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var i = -0xAD_F0_DF_FF_CA_24_76;
                w.WriteLong(i);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadLong();
                Assert.AreEqual(i, o);
            }
        }

        /// <summary>
        ///     test read write a long value
        /// </summary>
        [TestMethod]
        public void TestReadWriteInvalidLong() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var i = Constants.MagicNumber;
                w.WriteUint(i);
                stream.Seek(0, SeekOrigin.Begin);
                Assert.Throws<UnexpectedEndOfFileException>(() => r.ReadLong());
            }
        }

        /// <summary>
        ///     test to read an invalid byte
        /// </summary>
        [TestMethod]
        public void TestReadInvalidByte() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var r = CreateReader(env, stream)) {
                Assert.Throws<UnexpectedEndOfFileException>(() => r.ReadByte());
            }
        }

        /// <summary>
        ///     test to write a short string
        /// </summary>
        [TestMethod]
        public void TestWriteReadShortString() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var i = "AABBCCDDEEFFGGHHIIJJLLMMNNOOPPQQRRSSTTUUVVWWXXYYZZ";
                w.WriteString(i);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadString();
                Assert.AreEqual(i, o);
            }
        }

        /// <summary>
        ///     write an not pooled string
        /// </summary>
        [TestMethod]
        public void TestWriteReadUnPooledString() {
            var env = CreateEnvironment();
            var pool = env.StringPool;
            var i = "_A_";
            Assert.IsFalse(pool.ContainsString(i));
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                w.WriteString(i);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadString();
                Assert.AreEqual(i, o);
                Assert.IsTrue(pool.ContainsString(i));
            }
        }

        /// <summary>
        ///     write an invalid long string
        /// </summary>
        [TestMethod]
        public void TestReadInvalidLongString() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var z = new string('x', 2 * r.StringPool.MaximalStringLength);
                w.WriteString(z);
                stream.Seek(0, SeekOrigin.Begin);
                var invalidLen = (uint)(99 * z.Length);
                w.WriteUint(invalidLen);
                stream.Seek(0, SeekOrigin.Begin);
                Assert.Throws<UnexpectedEndOfFileException>(() => r.ReadString());
            }
        }

        /// <summary>
        ///     write an invalid short string
        /// </summary>
        [TestMethod]
        public void TestReadInvalidShortString() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var z = new string('x', 3);
                w.WriteString(z);
                stream.Seek(0, SeekOrigin.Begin);
                var invalidLen = 60u;
                w.WriteUint(invalidLen);
                stream.Seek(0, SeekOrigin.Begin);
                Assert.Throws<UnexpectedEndOfFileException>(() => r.ReadString());
            }
        }

        /// <summary>
        ///     test to read / write a long string
        /// </summary>
        [TestMethod]
        public void TestWriteReadLongString() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var z = new string('x', 2 * r.StringPool.MaximalStringLength);
                w.WriteString(z);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadString();
                Assert.AreEqual(z, o);
            }
        }

        /// <summary>
        ///     test to read / write an empty string
        /// </summary>
        [TestMethod]
        public void TestWriteReadWriteEmptyString() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var z = string.Empty;
                w.WriteString(z);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadString();
                Assert.AreEqual(z, o);
            }
        }

        /// <summary>
        ///     test to read and write the empty string
        /// </summary>
        [TestMethod]
        public void TestReadInvalidMagicNumber() {
            var env = CreateEnvironment();
            var l = new ListLogTarget();

            env.Log.RegisterTarget(l);
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var i = Constants.MagicNumber - 20;
                w.WriteUint(i);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadUnit();
                Assert.IsNull(o);
                Assert.AreEqual(1, l.Messages.Count);
                Assert.AreEqual(MessageNumbers.InvalidFileFormat, l.Messages[0].MessageID);
            }
        }

        /// <summary>
        ///     test to read / write a tag
        /// </summary>
        [TestMethod]
        public void TestReadWriteTag() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var i = new SampleTag(SampleTag.Data1, SampleTag.Data2);
                w.WriteTag(i);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadTag(new SampleTag());
                Assert.AreEqual(i.Point1, o.Point1);
                Assert.AreEqual(i.Point2, o.Point2);
            }
        }

        /// <summary>
        ///     test to read / write a tag
        /// </summary>
        [TestMethod]
        public void TestReadWriteInvalidTag() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var i = new StringTag {
                    Id = 923,
                    Value = "ddd"
                };

                w.WriteTag(i);
                w.WriteTag(i);
                w.WriteTag(i);
                w.WriteTag(i);
                stream.Seek(0, SeekOrigin.Begin);
                Assert.Throws<TypeReaderWriteException>(() => r.ReadTag(new SampleTag()));
            }
        }



        /// <summary>
        /// test to read / write a string tag
        /// </summary>
        [TestMethod]
        public void TestReadWriteStringTag() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var i = new StringTag { Id = 0xAFFE, Value = "_x_" };
                w.WriteTag(i);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadTag(new StringTag());
                Assert.AreEqual(i.Id, o.Id);
                Assert.AreEqual(i.Value, o.Value);
            }
        }


        /// <summary>
        ///     test writing / reading of references
        /// </summary>
        [TestMethod]
        public void TestWriteReadReference() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var rf = new Reference();
                var n = 3u;
                var offset = 2 * sizeof(uint) + sizeof(long);

                Assert.IsFalse(rf.HasAddress);
                w.WriteReferenceAddress(rf);
                Assert.IsFalse(rf.HasAddress);
                w.WriteUint(n);
                w.WriteUint(n);
                w.WriteReferenceValue(rf);
                Assert.AreEqual(offset, stream.Position);
                Assert.IsTrue(rf.HasAddress);
                w.WriteUint(n);
                Assert.AreEqual(offset, rf.Address);
                w.WriteOpenReferences();

                stream.Seek(0, SeekOrigin.Begin);
                var rf2 = new Reference();
                r.ReadReference(rf2);
                Assert.IsTrue(rf2.HasAddress);
                Assert.AreEqual(offset, rf2.Address);
            }
        }

        /// <summary>
        ///     test writing / reading of references
        /// </summary>
        [TestMethod]
        public void TestWritReferenceTwice() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var rf = new Reference();
                var n = 3u;

                Assert.IsFalse(rf.HasAddress);
                w.WriteReferenceAddress(rf);
                Assert.IsFalse(rf.HasAddress);
                w.WriteUint(n);
                w.WriteUint(n);
                w.WriteReferenceValue(rf);
                Assert.Throws<TypeReaderWriteException>(() => w.WriteReferenceValue(rf));
            }
        }

        /// <summary>
        ///     test writing / reading of references
        /// </summary>
        [TestMethod]
        public void TestWriteOpenReferenceWithoutAddress() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var rf = new Reference();
                Assert.IsFalse(rf.HasAddress);
                w.WriteReferenceAddress(rf);
                Assert.IsFalse(rf.HasAddress);
                Assert.Throws<TypeReaderWriteException>(() => w.WriteOpenReferences());
            }
        }

        /// <summary>
        ///     test reading / write parameter groups
        /// </summary>
        [TestMethod]
        public void TestWriteReadParameterGroup() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var nt = env.TypeRegistry.SystemUnit.NoType;
                var d = new RoutineGroup(env.TypeRegistry.SystemUnit, "_");
                var pg = new Routine(d, PK.Procedure);
                pg.ResultType = nt.Reference;
                var t = new ParameterGroupTag();

                t.Initialize(pg);
                w.WriteTag(t);

                stream.Seek(0, SeekOrigin.Begin);
                t = new ParameterGroupTag();
                d = new RoutineGroup(env.TypeRegistry.SystemUnit, "_");
                r.ReadTag(t);
                t.AddToRoutine(d);

                pg = d.Items[0] as Routine;
                Assert.AreEqual(pg.Kind, PK.Procedure);
            }
        }

        /// <summary>
        ///     test write / read boolean constant values
        /// </summary>
        [TestMethod]
        public void TestWriteReadBooleanValue() {
            var env = CreateEnvironment();
            var rt = env.Runtime.Booleans;
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var su = env.TypeRegistry.SystemUnit;
                var b1 = rt.ToBoolean(true, su.BooleanType);
                var b2 = rt.ToByteBool(2, su.ByteBoolType);
                var b3 = rt.ToWordBool(4433, su.WordBoolType);
                var b4 = rt.ToLongBool(23, su.LongBoolType);
                var t = new TypeRefTag();

                foreach (var b in new[] { b1, b2, b3, b4 }) {
                    t.Initialize(b);
                    w.WriteTag(t);
                }

                stream.Seek(0, SeekOrigin.Begin);
                foreach (var b in new[] { b1, b2, b3, b4 }) {
                    t = new TypeRefTag();
                    r.ReadTag(t);
                    Assert.AreEqual(b, t.TypeReference);
                }
            }
        }

        /// <summary>
        ///     test write / read a call instruction
        /// </summary>
        [TestMethod]
        public void TestWriteReadCallInstruction() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var u = env.TypeRegistry.SystemUnit;
                var rr = u.Symbols.Where(tx => string.Equals(tx.Name, "writeln", System.StringComparison.OrdinalIgnoreCase)).FirstOrDefault() as IRoutineGroup;
                var parms = new Routine(rr, PK.Procedure);
                parms.ResultType = env.TypeRegistry.SystemUnit.NoType.Reference;
                var callInfo = env.Runtime.Types.MakeInvocationResultFromIntrinsic(rr, env.TypeRegistry.Runtime.Types.MakeSignature(env.TypeRegistry.SystemUnit.NoType.Reference));
                var op = new OpCode(OpCodeId.Call, parms.Encode());
                var t = new OpCodeTag();
                t.Initialize(op);
                w.WriteTag(t);

                stream.Seek(0, SeekOrigin.Begin);
                t = new OpCodeTag();
                r.ReadTag(t);

                Assert.AreEqual(t.OpCode.Id, OpCodeId.Call);
                var c = new Routine(rr, PK.Procedure); // .. t.OpCode.Params,
                Assert.AreEqual(c.RoutineGroup.RoutineId, IntrinsicRoutineId.WriteLn);
            }
        }


        /// <summary>
        ///     read write integer
        /// </summary>
        [TestMethod]
        public void TestWriteReadUShort() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var i = (ushort)23332;
                w.WriteUShort(i);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadUshort();
                Assert.AreEqual(i, o);
            }
        }

        /// <summary>
        ///     test read an invalid integer
        /// </summary>
        [TestMethod]
        public void TestReadInvalidUShort() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                w.WriteByte(0xA0);
                stream.Seek(0, SeekOrigin.Begin);
                Assert.Throws<UnexpectedEndOfFileException>(() => r.ReadUshort());
            }
        }

    }
}
