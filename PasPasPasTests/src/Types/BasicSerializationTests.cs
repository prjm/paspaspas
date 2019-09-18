using System.IO;
using PasPasPas.Globals.Log;
using PasPasPas.Infrastructure.Log;
using PasPasPas.Typings.Serialization;
using PasPasPasTests.Common;

namespace PasPasPasTests.Types {

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
            typeWriter.WriteUint(ref v);
            v = Point2;
            typeWriter.WriteUint(ref v);
        }

        internal override void ReadData(uint kind, TypeReader typeReader) {
            if (Kind != kind)
                throw new InvalidDataException();

            Point1 = typeReader.ReadUint();
            Point2 = typeReader.ReadUint();
        }
    }

    public class BasicSerializationTests : SerializationTest {

        [TestMethod]
        public void TestWriteReadInteger() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var i = Constants.MagicNumber;
                w.WriteUint(ref i);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadUint();
                Assert.AreEqual(i, o);
            }
        }

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

        [TestMethod]
        public void TestReadInvalidByte() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var r = CreateReader(env, stream)) {
                Assert.Throws<UnexpectedEndOfFileException>(() => r.ReadByte());
            }
        }

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
                w.WriteUint(ref invalidLen);
                stream.Seek(0, SeekOrigin.Begin);
                Assert.Throws<UnexpectedEndOfFileException>(() => r.ReadString());
            }
        }

        [TestMethod]
        public void TestReadInvalidShortString() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var z = new string('x', r.StringPool.MaximalStringLength / 2);
                w.WriteString(z);
                stream.Seek(0, SeekOrigin.Begin);
                var invalidLen = (uint)(99 * z.Length);
                w.WriteUint(ref invalidLen);
                stream.Seek(0, SeekOrigin.Begin);
                Assert.Throws<UnexpectedEndOfFileException>(() => r.ReadString());
            }
        }

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

        [TestMethod]
        public void TestReadInvalidMagicNumber() {
            var env = CreateEnvironment();
            var l = new ListLogTarget();

            env.Log.RegisterTarget(l);
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var i = Constants.MagicNumber - 20;
                w.WriteUint(ref i);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadUnit();
                Assert.IsNull(o);
                Assert.AreEqual(1, l.Messages.Count);
                Assert.AreEqual(MessageNumbers.InvalidFileFormat, l.Messages[0].MessageID);
            }
        }

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

        [TestMethod]
        public void TestReadWriteStringTag() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(
                env, stream)) {
                var i = new StringTag { Id = 0xAFFE, Value = "_x_" };
                w.WriteTag(i);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadTag(new StringTag());
                Assert.AreEqual(i.Id, o.Id);
                Assert.AreEqual(i.Value, o.Value);
            }
        }

    }
}
