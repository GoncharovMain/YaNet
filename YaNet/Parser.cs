using System.Text;
using YaNet.Features;
using YaNet.Exceptions;

namespace YaNet
{
	public class Parser
	{
		private StringBuilder _buffer;

		public Parser(StringBuilder text)
		{
			_buffer = text;
		}

		public Parser(string text) : this(new StringBuilder(text)) { }


		public void Deserialize()
		{
			
		}
	}
}