using YaNet.Rows;

namespace YaNet
{
	public class Mark
	{
		public int Start { get; set; }
		public int End { get; set; }
		public int Length => End - Start + 1;

		public Mark(int start, int end)
		{
			Start = start;
			End = end;
		}

		public static implicit operator String(Mark offset)
			=> offset.ToString();

		public static implicit operator Mark((int start, int end) offset)
			=> new Mark(offset.start, offset.end);

		public override string ToString() => $"Start: {Start} end: {End} length: {Length}";
	}
}