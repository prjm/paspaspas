using System;

namespace PasPasPas.Runtime.Common {

    /// <summary>
    ///     own implementation of  bit array
    /// </summary>
    /// <remarks>replacement for framework class <c>BitArray</c> which doesn't provide
    /// the required operations</remarks>
    public class Bits {

        private int[] data;
        private int bitSize;

        private const int intSize = 8 * sizeof(int);
        private const int byteSize = 8 * sizeof(byte);

        /// <summary>
        ///     create a new bit array
        /// </summary>
        /// <param name="numberOfBits">number of bits</param>
        public Bits(int numberOfBits) {
            bitSize = numberOfBits;
            data = new int[(numberOfBits + intSize - 1) / intSize];
        }

        /// <summary>
        ///     create a new bit array
        /// </summary>
        /// <param name="fromAnotherBitArray">copied bit array</param>
        public Bits(Bits fromAnotherBitArray) : this(fromAnotherBitArray.Length)
            => Assign(fromAnotherBitArray);

        /// <summary>
        ///     set all bits to <c>true</c>
        /// </summary>
        public void Fill() {
            for (var i = 0; i < data.Length; i++)
                data[i] = GetBits(i, unchecked((int)0xFFFFFFFF));
        }

        /// <summary>
        ///     set all bits to <c>false</c>
        /// </summary>
        public void Clear() {
            for (var i = 0; i < data.Length; i++)
                data[i] = GetBits(i, 0);
        }

        private int GetBits(int index)
            => GetBits(index, data[index]);

        /// <summary>
        ///     get the relevant bits (masked)
        /// </summary>
        /// <param name="index">array index</param>
        /// <param name="value">array value</param>
        /// <returns>masked value</returns>
        private int GetBits(int index, int value) {
            if (index < data.Length - 1)
                return value;

            var mask = unchecked(0xFFFFFFFF >> (intSize - (bitSize % intSize)));
            return unchecked((int)(value & mask));
        }

        /// <summary>
        ///     negate all bits
        /// </summary>
        public void Invert() {
            for (var i = 0; i < data.Length; i++)
                data[i] = GetBits(i, unchecked(~GetBits(i)));
        }

        /// <summary>
        ///     access the least significant byte
        /// </summary>
        public byte LeastSignificantByte {
            get {
                if (data.Length > 0)
                    return unchecked((byte)(GetBits(0) & 0xFF));
                return default;
            }
            set {
                if (data.Length > 0)
                    data[0] = GetBits(0, unchecked((GetBits(0) & unchecked((int)0xFFFFFF00)) | value & 0xFF));
            }
        }

        /// <summary>
        ///     access the least significant signed byte
        /// </summary>
        public sbyte LeastSignificantSignedByte {
            get => unchecked((sbyte)LeastSignificantByte);
            set => LeastSignificantByte = (unchecked((byte)value));
        }

        /// <summary>
        ///     access the least significant word
        /// </summary>
        public ushort LeastSignificantWord {
            get {
                if (data.Length > 0)
                    return unchecked((ushort)(GetBits(0) & 0xFFFF));
                return default;
            }
            set {
                if (data.Length > 0)
                    data[0] = GetBits(0, unchecked((GetBits(0) & unchecked((int)0xFFFF0000)) | (value & 0xFFFF)));
            }
        }

        /// <summary>
        ///     access the least significant word
        /// </summary>
        public short LeastSignificantSignedWord {
            get => unchecked((short)LeastSignificantWord);
            set => LeastSignificantWord = unchecked((ushort)value);
        }

        /// <summary>
        ///     access the least significant double word
        /// </summary>
        public uint LeastSignificantDoubleWord {
            get => unchecked((uint)LeastSignificantSignedDoubleWord);
            set => LeastSignificantSignedDoubleWord = unchecked((int)value);
        }

        /// <summary>
        ///     access the least significant quadruple word
        /// </summary>
        public long LeastSignificantSignedQuadWord {
            get => unchecked((long)LeastSignificantQuadWord);
            set => LeastSignificantQuadWord = unchecked((ulong)value);
        }

        /// <summary>
        ///     access least significant double word
        /// </summary>
        public int LeastSignificantSignedDoubleWord {
            get {
                if (data.Length < 1)
                    return default;
                return GetBits(0);
            }
            set {
                if (data.Length < 1)
                    return;
                data[0] = GetBits(0, value);
            }
        }

        /// <summary>
        ///     access the least significant quadruple word
        /// </summary>
        public ulong LeastSignificantQuadWord {
            get {
                if (data.Length < 1)
                    return default;

                var result = unchecked((GetBits(0) & 0xFFFFFFFF));

                if (data.Length < 2)
                    return unchecked((ulong)result);

                result = unchecked(result | (((long)GetBits(1)) << intSize));
                return unchecked((ulong)result);
            }

            set {
                if (data.Length < 1)
                    return;

                data[0] = GetBits(0, unchecked((int)(value & 0xFFFFFFFF)));

                if (data.Length < 2)
                    return;

                data[1] = GetBits(1, unchecked((int)((value >> intSize) & 0xFFFFFFFF)));
            }
        }

        /// <summary>
        ///     gets this value as byte array
        /// </summary>
        /// <returns></returns>
        public byte[] AsByteArray {
            get {
                var result = new byte[(bitSize + byteSize - 1) / byteSize];
                var slice = intSize / byteSize;

                for (var index = 0; index < result.Length; index++) {
                    var offset = 8 * (index % slice);
                    result[index] = unchecked((byte)(((uint)GetBits(index / slice) >> offset) & 0xFF));
                }

                return result;
            }
        }

        /// <summary>
        ///     assign the value of another number to his number
        /// </summary>
        /// <param name="number"></param>
        public void Assign(Bits number) {
            if (number.Length != Length)
                throw new InvalidOperationException();

            for (var i = 0; i < data.Length; i++)
                data[i] = number.GetBits(i);
        }

        /// <summary>
        ///     number of bits
        /// </summary>
        public int Length
            => bitSize;

        /// <summary>
        ///     check for equality
        /// </summary>
        /// <param name="obj">object to check</param>
        /// <returns></returns>
        public override bool Equals(object obj) {
            if (obj is Bits b) {
                if (b.bitSize != bitSize)
                    return false;

                for (var i = 0; i < data.Length; i++)
                    if (GetBits(i) != b.GetBits(i))
                        return false;

                return true;
            }

            return false;
        }

        /// <summary>
        ///     add an value to this bit array
        /// </summary>
        /// <param name="value">value to add</param>
        public void Add(Bits value) {
            var carry = 0UL;

            for (var j = 0; j < data.Length && j < value.Length; j++) {

                var left = unchecked((ulong)GetBits(j) & 0xFFFFFFFF);
                var right = unchecked((ulong)value.GetBits(j) & 0xFFFFFFFF);
                var sum = unchecked(left + right + carry);

                data[j] = GetBits(j, unchecked((int)(sum & 0xFFFFFFFF)));
                carry = sum >> intSize;
            }
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17;

            for (var i = 0; i < data.Length; i++)
                result = unchecked(result * 31 + GetBits(0));

            return result;
        }

        /// <summary>
        ///     access a bit by index
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public bool this[int index] {
            get {
                if (index < 0 || index >= bitSize)
                    throw new IndexOutOfRangeException();

                var qword = GetBits(index / intSize);
                return unchecked((1 << (index % bitSize)) & qword) != 0;
            }

            set {
                if (index < 0 || index >= bitSize)
                    throw new IndexOutOfRangeException();

                var qword = GetBits(index / intSize);

                if (value)
                    qword = unchecked(qword | (1 << index % bitSize));
                else
                    qword = unchecked(qword & ~(1 << index % bitSize));

                data[index / intSize] = qword;
            }
        }

        /// <summary>
        ///     format this number as hex string
        /// </summary>
        /// <returns>hex string</returns>
        public override string ToString() {
            var result = "";
            for (var i = 0; i < data.Length; i++) {
                var intNumber = GetBits(i);
                result = intNumber.ToString("X8") + result;
            }
            return "0x" + result;
        }
    }
}
