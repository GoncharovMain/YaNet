using YaNet;

namespace YaNet.Features
{
	public class ItemKeyValueRow : Row
	{
		private Mark _key;
		private Mark _value;

		public Mark Key => _key;
		public Mark Value => _value;

		public ItemKeyValueRow(int indent, Row row, Mark key, Mark value)
			: base(indent, row, typeof(ItemKeyValueRow))
		{
			_key = key;
			_value = value;
		}

		public ItemKeyValueRow(int indent, Mark row, Mark key, Mark value) 
			: base(indent, row, typeof(ItemKeyValueRow))
		{
			_key = key;
			_value = value;
		}

		public override void Info() 
			=> Console.WriteLine($"[{_indent}] row: {_row} type: {Type.Name} key: {_key} value: {_value}");
	}
}