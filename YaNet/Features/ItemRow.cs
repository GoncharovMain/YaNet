using YaNet;

namespace YaNet.Features
{
	public class ItemRow : Row
	{
		private Mark _item;

		public Mark Item => _item;


		public ItemRow(int indent, Row row, Mark item) 
			: base(indent, row, typeof(ItemRow)) => _item = item;

		public ItemRow(int indent, Mark row, Mark item) 
			: base(indent, row, typeof(ItemRow)) => _item = item;

		public override void Info() 
			=> Console.WriteLine($"[{_indent}] row: {_row} type: {Type.Name} item: {_item}");
	}
}