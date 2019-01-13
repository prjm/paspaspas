using System.Collections.Immutable;
using PasPasPas.Api;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Infrastructure.ObjectPooling;
using PasPasPas.Options.DataTypes;
using PasPasPas.Runtime.Values;
using PasPasPas.Runtime.Values.FloatValues;
using PasPasPas.Runtime.Values.IntValues;
using PasPasPas.Typings.Common;
using SharpFloat.FloatingPoint;


namespace PasPasPasTests.Common {

    public class CommonTest {

        /// <summary>
        ///     get the basic environment
        /// </summary>
        /// <returns></returns>
        protected static ITypedEnvironment CreateEnvironment(NativeIntSize intSize = NativeIntSize.Undefined)
            => new DefaultEnvironment(intSize);

        /// <summary>
        ///     create a temp runtime
        /// </summary>
        protected static RuntimeValueFactory MakeRuntime()
            => new RuntimeValueFactory(new ListPools());

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(sbyte number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(byte number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(short number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(ushort number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(int number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     make a unkown value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="typeKind"></param>
        /// <returns></returns>
        protected static ITypeReference GetUnkownValue(int typeId, CommonTypeKind typeKind)
            => MakeRuntime().Types.MakeTypeInstanceReference(typeId, typeKind);

        /// <summary>
        ///     get a subrange value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static ITypeReference GetSubrangeValue(int typeId, ITypeReference value)
            => new SubrangeValue(typeId, value);

        /// <summary>
        ///     make a new pointer value
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected static ITypeReference GetPointerValue(ITypeReference address)
            => MakeRuntime().Types.MakePointerValue(KnownTypeIds.UntypedPointer, address);

        /// <summary>
        ///     get the Unicode string value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected static ITypeReference GetUnicodeStringValue(string text)
            => MakeRuntime().Strings.ToUnicodeString(text);

        protected static ITypeReference GetArrayValue(int baseTypeId, params ITypeReference[] values)
            => MakeRuntime().Structured.CreateArrayValue(9999, baseTypeId, values.ToImmutableArray());

        /// <summary>
        ///     get the ANSI string value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected static ITypeReference GetAnsiStringValue(string text)
            => MakeRuntime().Strings.ToAnsiString(text);

        /// <summary>
        ///     get the short string value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected static ITypeReference GetShortStringValue(string text)
            => MakeRuntime().Strings.ToShortString(text);

        /// <summary>
        ///     get the Unicode char value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected static ITypeReference GetWideCharValue(char text)
            => MakeRuntime().Chars.ToWideCharValue(KnownTypeIds.WideCharType, text);

        /// <summary>
        ///     get the ANSI char value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected static ITypeReference GetAnsiCharValue(byte text)
            => MakeRuntime().Chars.ToAnsiCharValue(KnownTypeIds.AnsiCharType, text);



        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(uint number)
            => IntegerValueBase.ToScaledIntegerValue(number);


        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(long number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static ITypeReference GetIntegerValue(ulong number)
            => IntegerValueBase.ToScaledIntegerValue(number);

        /// <summary>
        ///     get extended value
        /// </summary>
        /// <param name="number">number</param>
        /// <returns>number value</returns>
        protected static ITypeReference GetExtendedValue(in ExtF80 number)
            => FloatValueBase.ToExtendedValue(number);

        /// <summary>
        ///     get extended value
        /// </summary>
        /// <param name="number">number</param>
        /// <returns>number value</returns>
        protected static ITypeReference GetExtendedValue(string number)
            => FloatValueBase.ToExtendedValue(ExtF80.TryParse(number, out var d) ? d : ExtF80.Zero);

        /// <summary>
        ///     get a boolean value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static ITypeReference GetBooleanValue(bool value)
            => value ?
                MakeRuntime().Booleans.TrueValue :
                MakeRuntime().Booleans.FalseValue;

        protected static ITypeReference GetByteBooleanValue(byte value)
            => MakeRuntime().Booleans.ToByteBool(value, KnownTypeIds.ByteBoolType);

        protected static ITypeReference GetLongBooleanValue(uint value)
            => MakeRuntime().Booleans.ToLongBool(value, KnownTypeIds.LongBoolType);

        protected static ITypeReference GetWordBooleanValue(ushort value)
            => MakeRuntime().Booleans.ToWordBool(value, KnownTypeIds.WordBoolType);

        protected static ITypeReference GetRecordValue(int typeId, params ITypeReference[] values)
            => MakeRuntime().Structured.CreateRecordValue(typeId, values.ToImmutableArray());
    }
}
