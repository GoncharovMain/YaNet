using System.Reflection;
using YaNet.Features;
using System.Text;

namespace YaNet.Structures
{
	public class Scalar : Structure
	{
		private KeyValueRow _keyValueRow;

		private List<string> _scalarTypes = new()
		{
			"Int16", "UInt16",
			"Byte", "SByte",
			"Int32", "UInt32",
			"Int64", "UInt64",
			"Single", "Double", "Decimal",
			"Boolean",
			"Char", "String",
		};

		public Scalar(StringBuilder buffer, Row row, object obj)
			: base(new Marker(buffer), obj)
		{
			_keyValueRow = row as KeyValueRow;

			string propertyName = _marker.Buffer(_keyValueRow.Key);

			_propertyInfo = _obj.GetType().GetProperty(propertyName);
		}

		public override void Init()
		{
			string substring = _marker.Buffer(_keyValueRow.Value);

			object value = Types.Converter(_propertyInfo.PropertyType, substring);

			_propertyInfo.SetValue(_obj, value);
		}
	}
}