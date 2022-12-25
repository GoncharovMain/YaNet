using YaNet;

namespace YaNet.Features
{
	public class KeyValueRow : KeyRow
	{
		private Mark _value;

		public Mark Value => _value;


		public KeyValueRow(int indent, Row row, Mark key, Mark value) 
			: base(indent, row, key, typeof(KeyValueRow)) => _value = value;

		public KeyValueRow(int indent, Mark row, Mark key, Mark value) 
			: base(indent, row, key, typeof(KeyValueRow)) => _value = value;

		public override void Info() 
			=> Console.WriteLine($"[{_indent}] row: {_row} type: {Type.Name} key: {_key} value: {_value}");
	}
}