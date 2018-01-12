using System;

namespace PasPasPas.Runtime.Common {

    /// <summary>
    ///     implementation of bit array and common operations
    /// </summary>
    /// <remarks>
    ///     replacement for framework class <c>BitArray</c> which doesn't provide
    ///     the required operations
    /// </remarks>
    public class Bits {

        private uint[] data;
        private int bitSize;

        private const int intSize = 8 * sizeof(int);
        private const int byteSize = 8 * sizeof(byte);

        /// <summary>
        ///     create a new bit array
        /// </summary>
        /// <param name="numberOfBits">number of bits</param>
        /// <remarks>initially, all bits are unset</remarks>
        public Bits(int numberOfBits) {
            bitSize = numberOfBits;
            data = new uint[(numberOfBits + intSize - 1) / intSize];
        }

        /// <summary>
        ///     create a new bit array
        /// </summary>
        /// <param name="fromAnotherBitArray">initial value for this bit array</param>
        public Bits(Bits fromAnotherBitArray) : this(fromAnotherBitArray.Length)
            => Assign(fromAnotherBitArray);

        /// <summary>
        ///     <c>true</c> if all bits are unset
        /// </summary>
        public bool IsCleared {
            get {
                for (var index = 0; index < data.Length; index++)
                    if (GetBits(index) != GetBits(index, 0))
                        return false;
                return true;
            }
        }

        /// <summary>
        ///     <c>true</c> if all bits are set
        /// </summary>
        public bool IsFilled {
            get {
                for (var index = 0; index < data.Length; index++)
                    if (GetBits(index) != GetBits(index, 0xFFFFFFFF))
                        return false;
                return true;
            }
        }

        /// <summary>
        ///     get a trimmed byte array
        /// </summary>
        /// <param name="customLength">number of bytes</param>
        /// <returns></returns>
        public byte[] GetTrimmedByteArray(int customLength) {
            var len = (bitSize + byteSize - 1) / byteSize;
            var slice = intSize / byteSize;

            if (customLength > 0)
                len = customLength;

            var result = new byte[len];

            for (var index = 0; index < result.Length; index++) {
                var offset = 8 * (index % slice);
                result[index] = unchecked((byte)(GetBits(index / slice) >> offset & 0xFF));
            }

            return result;
        }

        /// <summary>
        ///     find the last index of a given bit value
        /// </summary>
        /// <param name="value">search value</param>
        /// <returns>last index or <c>-1</c> if the value was not found</returns>
        public int LastIndexOf(bool value) {
            for (var index = bitSize - 1; index >= 0; index--)
                if (this[index] == value)
                    return index;
            return -1;
        }

        /// <summary>
        ///     helper function: create a new bit array which is the two-complement of another bit array
        /// </summary>
        /// <param name="bits"></param>
        /// <returns></returns>
        public static Bits CreateTwoComplement(Bits bits) {
            var result = new Bits(bits);
            bits.TwoComplement();
            return result;
        }


        /// <summary>
        ///     set all bits to <c>true</c>
        /// </summary>
        public void Fill() {
            for (var i = 0; i < data.Length; i++)
                data[i] = GetBits(i, 0xFFFFFFFF);
        }

        /// <summary>
        ///     set all bits to <c>false</c>
        /// </summary>
        public void Clear() {
            for (var i = 0; i < data.Length; i++)
                data[i] = GetBits(i, 0);
        }

        private uint GetBits(int index)
            => GetBits(index, data[index]);

        /// <summary>
        ///     get the relevant bits (masked)
        /// </summary>
        /// <param name="index">array index</param>
        /// <param name="value">array value</param>
        /// <returns>masked value</returns>
        private uint GetBits(int index, uint value) {
            if (index < data.Length - 1)
                return value;

            var mask = unchecked(0xFFFFFFFF >> (intSize - (bitSize % intSize)));
            return unchecked(value & mask);
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
                    data[0] = GetBits(0, unchecked((GetBits(0) & 0xFFFFFF00) | ((uint)value & 0xFF)));
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
                    data[0] = GetBits(0, unchecked((GetBits(0) & 0xFFFF0000)) | ((uint)value & 0xFFFF));
            }
        }

        /// <summary>
        ///     fill bits from the right side (least significant byte side)
        /// </summary>
        /// <param name="fillBits">fill bits</param>
        /// <param name="margin">bits to skip</param>
        public void FillFromRight(Bits fillBits, int margin = 0) {
            for (var i = 0; i - margin < fillBits.Length && i < Length; i++) {
                if (i < margin)
                    this[i] = false;
                else
                    this[i] = fillBits[i - margin];
            }
        }

        /// <summary>
        ///     fill bits from the left side (most significant byte side)
        /// </summary>
        /// <param name="fillBits">fill bits</param>
        /// <param name="margin">addition margin</param>
        public void FillFromLeft(Bits fillBits, int margin = 0) {
            var offset = Math.Max(0, Length - fillBits.Length - margin);
            for (var i = 0; i - margin < fillBits.Length && offset + i < Length; i++) {
                if (i >= fillBits.Length)
                    this[offset + i] = false;
                else
                    this[offset + i] = fillBits[i];
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
        ///     compute a twos complement
        /// </summary>
        public void TwoComplement() {
            var one = new Bits(Length);
            one[0] = true;
            Invert();
            Add(one);
        }

        /// <summary>
        ///     access least significant double word
        /// </summary>
        public int LeastSignificantSignedDoubleWord {
            get {
                if (data.Length < 1)
                    return default;
                return unchecked((int)GetBits(0));
            }
            set {
                if (data.Length < 1)
                    return;
                data[0] = GetBits(0, unchecked((uint)value));
            }
        }

        /// <summary>
        ///     access the least significant quadruple word
        /// </summary>
        public ulong LeastSignificantQuadWord {
            get {
                if (data.Length < 1)
                    return default;

                var lsb = unchecked((GetBits(0) & 0xFFFFFFFF));

                if (data.Length < 2)
                    return lsb;

                var result = unchecked(lsb | (((ulong)GetBits(1)) << intSize));
                return unchecked((ulong)result);
            }

            set {
                if (data.Length < 1)
                    return;

                data[0] = GetBits(0, unchecked((uint)(value & 0xFFFFFFFF)));

                if (data.Length < 2)
                    return;

                data[1] = GetBits(1, unchecked((uint)((value >> intSize) & 0xFFFFFFFF)));
            }
        }

        /// <summary>
        ///     gets this value as byte array
        /// </summary>
        /// <returns></returns>
        public byte[] AsByteArray {

            get => GetTrimmedByteArray(-1);

            set {
                var slice = intSize / byteSize;

                for (var index = 0; index < value.Length; index++) {
                    var offset = index / slice;

                    if (offset >= data.Length)
                        break;

                    var mask = unchecked(0xFFU << 8 * (index % slice));

                    data[offset] = GetBits(offset, unchecked(//
                        (GetBits(offset) & ~mask) |
                        ((uint)value[index] << (8 * (index % slice))) & mask));
                }
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
        public int Length {
            get {
                return bitSize;
            }
            set {
                bitSize = value;
                Array.Resize(ref data, (value + intSize - 1) / intSize);
                for (var index = 0; index < data.Length; index++)
                    data[index] = GetBits(index);
            }
        }

        /// <summary>
        ///     access the most significant bit
        /// </summary>
        public bool MostSignificantBit {
            get => this[bitSize - 1];
            set => this[bitSize - 1] = value;
        }

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

                data[j] = GetBits(j, unchecked((uint)(sum & 0xFFFFFFFF)));
                carry = sum >> intSize;
            }
        }

        /// <summary>
        ///     multiply two binary values
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Bits Multiply(Bits value) {
            var size = Length + value.Length + 2;
            var a = new Bits(size);
            var s = new Bits(size);
            var p = new Bits(size);
            var mn = new Bits(this);

            a.FillFromLeft(this, 1);
            mn.TwoComplement();
            s.FillFromLeft(mn, 1);
            p.FillFromRight(value, 1);

            a.MostSignificantBit = MostSignificantBit;
            s.MostSignificantBit = !MostSignificantBit;

            for (var position = 1; position <= value.Length; position++) {
                var p0 = p[0];
                var p1 = p[1];

                if (!p1 && p0) {
                    p.Add(a);
                }
                else if (p1 && !p0) {
                    p.Add(s);
                }

                p.ArithmeticShiftRight(1);
            }

            var r = new Bits(p.Length - 2);

            for (var index = 0; index < r.Length; index++)
                r[index] = p[1 + index];

            return r;

        }

        /// <summary>
        ///     perform a binary division
        /// </summary>
        /// <param name="givenDivisor">divisor</param>
        /// <returns>integer division result</returns>
        /// <remarks>if divisor or dividend is zero, zero is returned</remarks>
        public Bits Divide(Bits givenDivisor) {
            var negate = false;
            var length = Length + givenDivisor.Length;
            Bits dividend;
            Bits divisor;

            if (givenDivisor.MostSignificantBit && MostSignificantBit) {
                dividend = CreateTwoComplement(this);
                divisor = CreateTwoComplement(givenDivisor);
            }
            else if (givenDivisor.MostSignificantBit) {
                dividend = new Bits(this);
                divisor = CreateTwoComplement(givenDivisor);
                negate = true;
            }
            else if (MostSignificantBit) {
                dividend = CreateTwoComplement(this);
                divisor = new Bits(givenDivisor);
                negate = true;
            }
            else {
                dividend = new Bits(this);
                divisor = new Bits(givenDivisor);
            }

            if (dividend.IsCleared || givenDivisor.IsCleared)
                return new Bits(length);

            dividend.Length = length;
            divisor.Length = length;

            var result = DivideInternal(dividend, divisor);

            if (negate)
                result.TwoComplement();

            return result;
        }

        /// <summary>
        ///     division of two
        /// </summary>
        /// <param name="dividend"></param>
        /// <param name="divisor"></param>
        /// <returns></returns>
        private Bits DivideInternal(Bits dividend, Bits divisor) {
            var alignPos = dividend.LastIndexOf(true);
            var length = dividend.Length;
            var numberOfShifts = alignPos - divisor.LastIndexOf(true);
            var result = new Bits(length);

            if (numberOfShifts < 0)
                return result;

            divisor.ArithmeticShiftLeft(numberOfShifts);

            while (!dividend.IsCleared) {
                var t = new Bits(dividend);
                t.Subtract(divisor);

                if (!t.MostSignificantBit) {
                    result.LeastSignificantBit = true;
                    dividend.Assign(t);
                }

                dividend.ArithmeticShiftLeft(1);
                result.ArithmeticShiftLeft(1);
            }

            if (numberOfShifts > 0)
                result.ArithmeticShiftRight(numberOfShifts - 1);
            return result;
        }

        private void Subtract(Bits subtrahend) {
            var s = new Bits(subtrahend);
            s.TwoComplement();
            Add(s);
        }

        private void ArithmeticShiftLeft(int numberOfBits) {
            if (numberOfBits >= Length) {
                Clear();
                return;
            }

            for (var position = Length - 1; position >= 0; position--) {
                if (position - numberOfBits >= 0)
                    this[position] = this[position - numberOfBits];
                else
                    this[position] = false;
            }
        }

        /// <summary>
        ///     shift arithmetically right
        /// </summary>
        /// <param name="numberOfBits"></param>
        public void ArithmeticShiftRight(int numberOfBits) {
            var one = MostSignificantBit;

            if (numberOfBits <= 0)
                return;

            if (numberOfBits >= Length) {
                if (one)
                    Fill();
                else
                    Clear();
                return;
            }

            for (var position = 0; position < Length; position++) {
                if (position + numberOfBits < Length)
                    this[position] = this[position + numberOfBits];
                else
                    this[position] = one;
            }
        }

        /// <summary>
        ///     compute a hash code
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() {
            var result = 17U;

            for (var i = 0; i < data.Length; i++)
                result = unchecked(result * 31 + GetBits(0));

            return unchecked((int)result);
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
                    qword = unchecked(qword | (1U << index % bitSize));
                else
                    qword = unchecked(qword & ~(1U << index % bitSize));

                data[index / intSize] = qword;
            }
        }

        /// <summary>
        ///     test if this is the most negative number for the given bit length
        /// </summary>
        public bool IsMostNegative {
            get {
                var len = data.Length;

                for (var i = 0; i < len; i++) {
                    var bits = GetBits(i);

                    if (i < len - 1 && bits != 0)
                        return false;
                    else if (i == len - 1 && bits != (1 << (bitSize % intSize) - 1))
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        ///     test if this is the most positive number for the given bit length
        /// </summary>
        public bool IsMostPositive {
            get {
                var len = data.Length;

                for (var i = 0; i < len; i++) {
                    var bits = GetBits(i);

                    if (i < len - 1 && bits != 0xFFFFFFFF)
                        return false;
                    else if (i == len - 1 && bits != GetBits(i, 0xFFFFFFFF) >> 1)
                        return false;
                }
                return true;
            }
        }

        /// <summary>
        ///     access the least significant bit
        /// </summary>
        public bool LeastSignificantBit {
            get => this[0];
            set => this[0] = value;
        }


        /// <summary>
        ///     format this bit array as string
        /// </summary>
        /// <returns>hex string</returns>
        public override string ToString() {
            var result = "";
            /*
            for (var i = 0; i < data.Length; i++) {
                var intNumber = GetBits(i);
                result = intNumber.ToString("X8") + result;
            }*/
            for (var i = 0; i < Length; i++) {
                if (this[i])
                    result = "1" + result;
                else
                    result = "0" + result;
            }
            return "0x" + result;
        }
    }
}
