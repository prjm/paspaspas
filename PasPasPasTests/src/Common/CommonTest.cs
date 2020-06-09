using System.Collections.Immutable;
using System.Linq;
using PasPasPas.Api;
using PasPasPas.Globals.Environment;
using PasPasPas.Globals.Files;
using PasPasPas.Globals.Options.DataTypes;
using PasPasPas.Globals.Runtime;
using PasPasPas.Globals.Types;
using PasPasPas.Typings.Common;
using PasPasPas.Typings.Structured;
using SharpFloat.FloatingPoint;


namespace PasPasPasTests.Common {

    /// <summary>
    ///     common base class for test cases
    /// </summary>
    public class CommonTest {

        /// <summary>
        ///     get the basic environment
        /// </summary>
        /// <returns></returns>
        protected static IAssemblyBuilderEnvironment CreateEnvironment(NativeIntSize intSize = NativeIntSize.Undefined)
            => Factory.CreateEnvironment(intSize);

        /// <summary>
        ///     create a temporary runtime
        /// </summary>
        protected static IRuntimeValueFactory MakeRuntime()
            => CreateEnvironment().Runtime;

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static IValue GetIntegerValue(sbyte number)
            => MakeRuntime().Integers.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static IValue GetIntegerValue(byte number)
            => MakeRuntime().Integers.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static IValue GetIntegerValue(short number)
            => MakeRuntime().Integers.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static IValue GetIntegerValue(ushort number)
            => MakeRuntime().Integers.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static IValue GetIntegerValue(int number)
            => MakeRuntime().Integers.ToScaledIntegerValue(number);

        /// <summary>
        ///     create a invocation value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="typeKind"></param>
        /// <returns></returns>
        protected static IInvocationResult GetInvocationValue(ITypeDefinition typeId, ITypeDefinition typeKind) {
            var fakeRoutine = new RoutineGroup(default, default);
            var _ = fakeRoutine.AddParameterGroup(RoutineKind.Function, typeKind.Reference);
            return MakeRuntime().Types.MakeInvocationResult(_);
        }

        /// <summary>
        ///     make a invocation value
        /// </summary>
        /// <returns></returns>
        protected static IIntrinsicInvocationResult GetInvocationValue(IntrinsicRoutineId id, ITypeDefinition result, ITypeDefinition param) {
            var e = CreateEnvironment();
            var rg = e.TypeRegistry.SystemUnit.Symbols.Where(t => t is IRoutineGroup x && x.RoutineId == id).FirstOrDefault(); ;
            var _ = CreateEnvironment().Runtime.Types.MakeSignature(result.Reference, param.Reference);
            return MakeRuntime().Types.MakeInvocationResultFromIntrinsic(rg as IRoutineGroup, _);
        }

        /// <summary>
        ///     get the nil value
        /// </summary>
        /// <returns></returns>
        protected static IValue GetNilValue()
            => MakeRuntime().Types.Nil;

        /// <summary>
        ///     get an error value
        /// </summary>
        /// <returns></returns>
        protected static IValue GetErrorValue()
            => MakeRuntime().Strings.Invalid;

        /// <summary>
        ///     get some enumeration values
        /// </summary>
        /// <param name="data">enumeration member names</param>
        /// <returns></returns>
        protected static IValue[] GetEnumValues(params string[] data) {
            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var t = tc.CreateEnumType(string.Empty);
            var v = 0;
            var result = new IValue[data.Length];
            foreach (var value in data) {
                result[v] = t.DefineEnumValue(e.Runtime, value, true, GetIntegerValue(v));
                v++;
            }
            return result;
        }

        /// <summary>
        ///     get a subrange value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static IValue GetSubrangeValue(ITypeDefinition typeId, IValue value)
            => MakeRuntime().Types.MakeSubrangeValue(typeId, value);

        /// <summary>
        ///     make a subragen value
        /// </summary>
        /// <param name="lowerBound"></param>
        /// <param name="upperBound"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static IValue GetSubrangeValue(IValue lowerBound, IValue upperBound, IValue value) {
            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var t = tc.CreateSubrangeType(string.Empty, e.TypeRegistry.SystemUnit.SmallIntType, lowerBound, upperBound);
            return e.Runtime.Types.MakeSubrangeValue(t, value);
        }

        /// <summary>
        ///     make a new pointer value
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        protected static IValue GetPointerValue(IValue address) {
            var e = CreateEnvironment();
            var pt = e.TypeRegistry.SystemUnit.GenericPointerType;
            return MakeRuntime().Types.MakePointerValue(pt, address);
        }

        /// <summary>
        ///     get an UNICODE string value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected static IValue GetUnicodeStringValue(string text)
            => MakeRuntime().Strings.ToUnicodeString(text);

        /// <summary>
        ///     get an array value
        /// </summary>
        /// <param name="typeId">base type id</param>
        /// <param name="baseTypeId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        protected static IValue GetArrayValue(ITypeDefinition typeId, ITypeDefinition baseTypeId, params IValue[] values)
            => MakeRuntime().Structured.CreateArrayValue(typeId, baseTypeId, values.ToImmutableArray());

        /// <summary>
        ///     get an array value
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        protected static IValue GetArrayValue(params IValue[] items) {
            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var baseType = items.Length > 0 ? items[0].TypeDefinition.Reference : e.TypeRegistry.SystemUnit.ErrorType.Reference; ;

            foreach (var v in items)
                baseType = e.TypeRegistry.GetBaseTypeForArrayOrSet(baseType, v).Reference;

            var at = tc.CreateStaticArrayType(baseType.TypeDefinition, string.Empty, e.TypeRegistry.SystemUnit.IntegerType, false);
            return e.Runtime.Structured.CreateArrayValue(at, baseType.TypeDefinition, ImmutableArray.Create(items));
        }

        /// <summary>
        ///     get the ANSI string value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected static IValue GetAnsiStringValue(string text)
            => MakeRuntime().Strings.ToAnsiString(text);

        /// <summary>
        ///     get the empty string value
        /// </summary>
        /// <returns></returns>
        protected static IValue GetEmptyStringValue()
            => MakeRuntime().Strings.EmptyString;

        /// <summary>
        ///     get the short string value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected static IValue GetShortStringValue(string text)
            => MakeRuntime().Strings.ToShortString(text);

        /// <summary>
        ///     get the Unicode char value
        /// </summary>
        /// <param name="character"></param>
        /// <returns></returns>
        protected static IValue GetWideCharValue(char character)
            => MakeRuntime().Chars.ToWideCharValue(character);

        /// <summary>
        ///     get the ANSI char value
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected static IValue GetAnsiCharValue(byte text)
            => MakeRuntime().Chars.ToAnsiCharValue(text);



        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static IValue GetIntegerValue(uint number)
            => MakeRuntime().Integers.ToScaledIntegerValue(number);


        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static IValue GetIntegerValue(long number)
            => MakeRuntime().Integers.ToScaledIntegerValue(number);

        /// <summary>
        ///     convert a value
        /// </summary>
        /// <param name="number"></param>
        /// <returns></returns>
        protected static IValue GetIntegerValue(ulong number)
            => MakeRuntime().Integers.ToScaledIntegerValue(number);

        /// <summary>
        ///     get extended value
        /// </summary>
        /// <param name="number">number</param>
        /// <returns>number value</returns>
        protected static IValue GetExtendedValue(in ExtF80 number)
            => MakeRuntime().RealNumbers.ToExtendedValue(number);

        /// <summary>
        ///     get extended value
        /// </summary>
        /// <param name="number">number</param>
        /// <returns>number value</returns>
        protected static IValue GetExtendedValue(string number)
            => MakeRuntime().RealNumbers.ToExtendedValue(ExtF80.TryParse(number, out var d) ? d : ExtF80.Zero);

        /// <summary>
        ///     get a boolean value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static IValue GetBooleanValue(bool value)
            => value ?
                MakeRuntime().Booleans.TrueValue :
                MakeRuntime().Booleans.FalseValue;

        /// <summary>
        ///     get a byte sized boolean value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static IValue GetByteBooleanValue(byte value) {
            var e = CreateEnvironment();
            var bt = e.TypeRegistry.SystemUnit.ByteBoolType;
            return e.Runtime.Booleans.ToByteBool(value, bt);
        }

        /// <summary>
        ///     get a long boolean value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static IValue GetLongBooleanValue(uint value) {
            var e = CreateEnvironment();
            var bt = e.TypeRegistry.SystemUnit.LongBoolType;
            return e.Runtime.Booleans.ToLongBool(value, bt);
        }

        /// <summary>
        ///     get a word sized boolean value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        protected static IValue GetWordBooleanValue(ushort value) {
            var e = CreateEnvironment();
            var bt = e.TypeRegistry.SystemUnit.WordBoolType;
            return e.Runtime.Booleans.ToWordBool(value, bt);
        }

        /// <summary>
        ///     get a constant record value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        protected static IValue GetRecordValue(ITypeDefinition typeId, params IValue[] values)
            => MakeRuntime().Structured.CreateRecordValue(typeId, values.ToImmutableArray());

        protected static IValue GetRecordValue(params (string, IValue)[] values) {
            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var rt = tc.CreateStructuredType(string.Empty, StructuredTypeKind.Record);

            foreach ((string name, IValue value) in values) {
                var fieldDef = new Variable();
                fieldDef.Name = name;
                fieldDef.TypeDefinition = value.TypeDefinition;
                rt.AddField(fieldDef);
            }

            var v = values.Select(t => t.Item2).ToImmutableArray();
            return e.Runtime.Structured.CreateRecordValue(rt, v);
        }

        /// <summary>
        ///     get a set value
        /// </summary>
        /// <param name="typeId"></param>
        /// <param name="values"></param>
        /// <returns></returns>
        protected static IValue GetSetValue(ITypeDefinition typeId, params IValue[] values)
            => MakeRuntime().Structured.CreateSetValue(typeId, values.ToImmutableArray());

        /// <summary>
        ///     get a set of enumeration values
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        protected static IValue GetSetValue(params string[] names) {
            var e = CreateEnvironment();
            var tc = e.TypeRegistry.CreateTypeFactory(e.TypeRegistry.SystemUnit);
            var et = tc.CreateEnumType("");
            var vals = new IValue[names.Length];

            for (var i = 0; i < names.Length; i++) {
                vals[i] = et.DefineEnumValue(e.Runtime, names[i], true, GetIntegerValue(i));
            }

            return e.Runtime.Structured.CreateSetValue(et, ImmutableArray.Create(vals));
        }

        /// <summary>
        ///     create a resolver
        /// </summary>
        /// <param name="path"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        protected IInputResolver CreateResolver(IFileReference path, string content)
            => CommonApi.CreateResolverForSingleString(path, content);

        /// <summary>
        ///     create a resolver
        /// </summary>
        /// <returns></returns>
        protected IInputResolver CreateResolver()
            => CommonApi.CreateAnyFileResolver();

    }
}
