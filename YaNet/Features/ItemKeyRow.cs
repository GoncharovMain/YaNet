using YaNet;

namespace YaNet.Features
{
	public class ItemKeyRow : Row
	{
		private Mark _key;

		public Mark Key => _key;

		public ItemKeyRow(int indent, Row row, Mark key) 
			: base(indent, row, typeof(ItemKeyRow)) => _key = key;

		public ItemKeyRow(int indent, Mark row, Mark key) 
			: base(indent, row, typeof(ItemKeyRow)) => _key = key;

		public override void Info() 
			=> Console.WriteLine($"[{_indent}] row: {_row} type: {Type.Name} item: {_key}");
	}
}