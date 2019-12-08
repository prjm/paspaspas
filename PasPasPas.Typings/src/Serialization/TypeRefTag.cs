using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;

namespace PasPasPas.Typings.Serialization {

    /// <summary>
    ///     type reference kind
    /// </summary>
    internal enum TypeRefKind : byte {
        Undef = 0,
        Boolean = 1,
        ByteBool = 2,
        WordBool = 3,
        LongBool = 4
    }

    /// <summary>
    ///     tag for type reference
    /// </summary>
    internal class TypeRefTag : Tag {

        public override uint Kind
            => Constants.TypeRefTag;

        public ITypeReference TypeReference { get; private set; }

        public TypeRefKind ToTypeRefKind(byte value)
            => (TypeRefKind)value;

        public byte ToByte(TypeRefKind value)
            => (byte)value;

        internal void Initialize(ITypeReference value)
            => TypeReference = value;

        internal override void ReadData(uint kind, TypeReader typeReader) {
            var tkr = ToTypeRefKind(typeReader.ReadByte());
            var typeId = (int)typeReader.ReadUint();

            switch (tkr) {
                case TypeRefKind.Boolean:
                    ReadBoolean(typeReader, typeId);
                    break;

                case TypeRefKind.ByteBool:
                    ReadByteBool(typeReader, typeId);
                    break;

                case TypeRefKind.WordBool:
                    ReadWordBool(typeReader, typeId);
                    break;

                case TypeRefKind.LongBool:
                    ReadLongBool(typeReader, typeId);
                    break;

                default:
                    throw new TypeReaderWriteException();
            }
        }

        private void ReadLongBool(TypeReader typeReader, int typeId) {
            var value = typeReader.ReadUint();
            TypeReference = typeReader.Types.Runtime.Booleans.ToLongBool(value, typeId);
        }

        private void ReadWordBool(TypeReader typeReader, int typeId) {
            var value = typeReader.ReadUshort();
            TypeReference = typeReader.Types.Runtime.Booleans.ToWordBool(value, typeId);
        }

        private void ReadByteBool(TypeReader typeReader, int typeId) {
            var value = typeReader.ReadByte();
            TypeReference = typeReader.Types.Runtime.Booleans.ToByteBool(value, typeId);
        }

        private void ReadBoolean(TypeReader typeReader, int typeId) {
            var value = typeReader.ReadByte();
            TypeReference = typeReader.Types.Runtime.Booleans.ToBoolean(value != 0, typeId);
        }

        internal override void WriteData(TypeWriter typeWriter) {

            if (TypeReference.ReferenceKind != TypeReferenceKind.ConstantValue)
                throw new TypeReaderWriteException();

            var trk = GetTypeRefKind(typeWriter);
            var typeId = (uint)TypeReference.TypeId;
            typeWriter.WriteByte(ToByte(trk));
            typeWriter.WriteUint(typeId);

            switch (trk) {

                case TypeRefKind.Boolean:
                    WriteBoolean(typeWriter);
                    break;

                case TypeRefKind.ByteBool:
                    WriteByteBool(typeWriter);
                    break;

                case TypeRefKind.WordBool:
                    WriteWordBool(typeWriter);
                    break;

                case TypeRefKind.LongBool:
                    WriteLongBool(typeWriter);
                    break;

                default:
                    throw new TypeReaderWriteException();
            }
        }

        private void WriteLongBool(TypeWriter typeWriter) {
            var i = ((IBooleanValue)TypeReference).AsUint;
            typeWriter.WriteUint(i);
        }

        private void WriteByteBool(TypeWriter typeWriter) {
            var b = (byte)((IBooleanValue)TypeReference).AsUint;
            typeWriter.WriteByte(b);
        }

        private void WriteWordBool(TypeWriter typeWriter) {
            var w = (ushort)((IBooleanValue)TypeReference).AsUint;
            typeWriter.WriteUShort(w);
        }

        private void WriteBoolean(TypeWriter writer) {
            var b = (byte)(((IBooleanValue)TypeReference).AsBoolean ? 1 : 0);
            writer.WriteByte(b);
        }

        private TypeRefKind GetTypeRefKind(TypeWriter typeWriter) {
            var typeDef = typeWriter.RegisteredTypes.GetTypeByIdOrUndefinedType(TypeReference.TypeId);

            switch (typeDef) {
                case IBooleanType booleanType:
                    switch (booleanType.Kind) {
                        case BooleanTypeKind.Boolean:
                            return TypeRefKind.Boolean;
                        case BooleanTypeKind.ByteBool:
                            return TypeRefKind.ByteBool;
                        case BooleanTypeKind.WordBool:
                            return TypeRefKind.WordBool;
                        case BooleanTypeKind.LongBool:
                            return TypeRefKind.LongBool;
                        default:
                            return TypeRefKind.Undef;
                    }

                default:
                    return TypeRefKind.Undef;
            }

        }
    }
}
