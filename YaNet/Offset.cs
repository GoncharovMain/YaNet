namespace YaNet
{
	public class Offset
	{
		public int Start { get; set; }
		public int End { get; set; }

		public Offset(int start, int end)
		{
			Start = start;
			End = end;
		}
	}
}