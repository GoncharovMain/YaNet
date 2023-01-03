using System.Reflection;

namespace YaNet.Structures
{
	public static class Types
	{
		public delegate object Quilify(string value);

		public static Dictionary<string, Type> KeyValueType = new ()
		{
			{ "short", typeof(short) },
			{ "ushort", typeof(ushort) },
			{ "byte", typeof(byte) },
			{ "sbyte", typeof(sbyte) },
			{ "int", typeof(int) },
			{ "uint", typeof(uint) },
			{ "long", typeof(long) },
			{ "ulong", typeof(ulong) },
			{ "float", typeof(float) },
			{ "double", typeof(double) },
			{ "decimal", typeof(decimal) },
			{ "string", typeof(string) },
			{ "char", typeof(char) },
			{ "bool", typeof(bool) },

			{ "Int16", typeof(short) },
			{ "UInt16", typeof(ushort) },
			{ "Byte", typeof(byte) },
			{ "SByte", typeof(sbyte) },
			{ "Int32", typeof(int) },
			{ "UInt32", typeof(uint) },
			{ "Int64", typeof(long) },
			{ "UInt64", typeof(ulong) },
			{ "Single", typeof(float) },
			{ "Double", typeof(double) },
			{ "Decimal", typeof(decimal) },
			{ "String", typeof(string) },
			{ "Char", typeof(char) },
			{ "Boolean", typeof(bool) },
		};

		public static object Converter(Type type, string value)
			=> type.Name switch
				{
					"Int16" => (object)Convert.ToInt16(value),
					"UInt16" => (object)Convert.ToUInt16(value),
					"Byte" => (object)Convert.ToByte(value),
					"SByte" => (object)Convert.ToSByte(value),
					"Int32" => (object)Convert.ToInt32(value),
					"UInt32" => (object)Convert.ToUInt32(value),
					"Int64" => (object)Convert.ToInt64(value),
					"UInt64" => (object)Convert.ToUInt64(value),
					"Single" => (object)Convert.ToSingle(value),
					"Double" => (object)Convert.ToDouble(value),
					"Decimal" => (object)Convert.ToDecimal(value),
					"Boolean" => (object)Convert.ToBoolean(value),
					"Char" => (object)Convert.ToChar(value),
					"String" => (object)value,
					_ => Activator.CreateInstance(type)
				};

		public static Dictionary<string, Quilify> Converters = new ()
		{
			{ "Int16", value => (object)Convert.ToInt16(value) },
			{ "UInt16", value => (object)Convert.ToUInt16(value) },
			{ "Byte", value => (object)Convert.ToByte(value) },
			{ "SByte", value => (object)Convert.ToSByte(value) },
			{ "Int32", value => (object)Convert.ToInt32(value) },
			{ "UInt32", value => (object)Convert.ToUInt32(value) },
			{ "Int64", value => (object)Convert.ToInt64(value) },
			{ "UInt64", value => (object)Convert.ToUInt64(value) },
			{ "Single", value => (object)Convert.ToSingle(value) },
			{ "Double", value => (object)Convert.ToDouble(value) },
			{ "Decimal", value => (object)Convert.ToDecimal(value) },
			{ "Boolean", value => (object)Convert.ToBoolean(value) },
			{ "Char", value => (object)Convert.ToChar(value) },
			{ "String", value => (object)value },



			//{ "Int32[]", value => null },
			//{ "Int32[][]", value => null },
			//{ "List`1", value => null },
			//{ "Dictionary`2", value => null },
		};
	}
}