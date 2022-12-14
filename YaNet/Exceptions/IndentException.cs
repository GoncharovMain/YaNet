using YaNet;

namespace YaNet.Exceptions
{
	public class SyntaxException : Exception
	{
		public SyntaxException(string message) : base(message) { }
	}

	public class IndentException : SyntaxException
	{
		public IndentException(string message) : base(message) { }
	}
}