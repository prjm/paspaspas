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

        public override uint Length
            => 8;

        public uint Point1 { get; set; }
            = 0;

        public uint Point2 { get; set; }
            = 0;

        public override void WriteData(TypeWriter typeWriter) {
            var v = Point1;
            typeWriter.WriteUint(ref v);
            v = Point2;
            typeWriter.WriteUint(ref v);
        }

        internal override void ReadData(uint kind, uint length, TypeReader typeReader) {
            if (Kind != kind)
                throw new InvalidDataException();
            if (Length != 8)
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
        public void TestWriteReadLongString() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var z = "AABBCCDDEEFFGGHHIIJJLLMMNNOOPPQQRRSSTTUUVVWWXXYYZZ";
                var i = string.Empty;
                for (var j = 0; j < 10; j++)
                    i += z;

                w.WriteString(i);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadString();
                Assert.AreEqual(i, o);
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
        public void ReadWriteTag() {
            var env = CreateEnvironment();
            using (var stream = new MemoryStream())
            using (var w = CreateWriter(env, stream))
            using (var r = CreateReader(env, stream)) {
                var i = new SampleTag(SampleTag.Data1, SampleTag.Data2);
                w.WriteTag(i);
                stream.Seek(0, SeekOrigin.Begin);
                var o = r.ReadTag<SampleTag>();
                Assert.AreEqual(i.Point1, o.Point1);
                Assert.AreEqual(i.Point2, o.Point2);
            }
        }

    }
}
