using System.Reflection;
using System.Text;
using YaNet.Features;

namespace YaNet.Structures
{
	public class ObjectType : Structure
	{
		private KeyRow _keyRow;

		private object _instance;

		public object Instance => _instance;

		public ObjectType(StringBuilder buffer, Row row, object obj)
			: base(new Marker(buffer), obj)
		{
			_keyRow = row as KeyRow;

			string propertyName = _marker.Buffer(_keyRow.Key);

			_propertyInfo = _obj.GetType().GetProperty(propertyName);
		}

		public override void Init()
		{
			string key = _marker.Buffer(_keyRow.Key);

			object value = Types.Converter(_propertyInfo.PropertyType, "${Person.Address}");

			// init object in object

			_instance = value;

			_propertyInfo.SetValue(_obj, value);
		}
	}
}