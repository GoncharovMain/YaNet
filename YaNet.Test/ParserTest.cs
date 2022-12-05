using Xunit;
using YaNet;
using Xunit.Abstractions;

namespace YaNet.Test
{
	public class PeekerTest
	{
		private readonly ITestOutputHelper _testOutputHelper;

		public PeekerTest(ITestOutputHelper testOutputHelper)
		{
			_testOutputHelper = testOutputHelper;
		}

		[Fact]
		public void FindSubstringTest()
		{
			_testOutputHelper.WriteLine("Ok");

			char[][] yanet = new char[][]
			{
				new char[] { 'n', 'a', 'm', 'e', ':', ' ', 'J', 'o', 'h', 'n' },
				new char[] { 'a', 'g', 'e', ':', ' ', '1', '8' }
			};

			(char[] symbols, char[] substring, int position)[] testFind = new (char[], char[], int)[]
			{
				(yanet[0], new char[] { ':', ' ', 'J' }, 4),
				(yanet[0], new char[] { 'a', 'm',  }, 1),
				(yanet[0], new char[] { 'h', 'n', 'i' }, -1),
				(yanet[1], new char[] { ':', ' ' }, 3),
				(yanet[1], new char[] { '1', '7' }, -1),
			};

			foreach ((char[] symbols, char[] substring, int position) item in testFind)
			{
				int currentPosition = new Peeker(item.symbols).IndexOf(item.substring);

				Assert.Equal(currentPosition, item.position);
			}
		}

		[Fact]
		public void FindSubstringStringTest()
		{
			string[] yanet = new string[]
			{
				"name: John",
				"age: 18"
			};

			(string symbols, string substring, int position)[] testFind = new (string, string, int)[]
			{
				(yanet[0], ": J", 4),
				(yanet[0], "am", 1),
				(yanet[0], "hni", -1),
				(yanet[1], ": ", 3),
				(yanet[1], "17", -1),
			};

			foreach ((string symbols, string substring, int position) item in testFind)
			{
				int currentPosition = new Peeker(item.symbols).IndexOf(item.substring);

				Assert.Equal(currentPosition, item.position);
			}
		}

		[Fact]
		public void FindSubstringException()
		{
			Assert.Throws<Exception>(new Action(() => 
				new Peeker(new char[] { ':', ' ' }).IndexOf(new char[] { ':', ' ', 'J' })));

			Assert.Throws<Exception>(new Action(() => 
				new Peeker(new char[] { }).IndexOf(new char[] { })));
		}

		[Fact]
		public void CountIndentSpacesTest()
		{
			string indent = "  ";

			(string line, int countIndent)[] yanets = new (string, int)[]
			{
				("    name: John", 2),
				("  name: Bob", 1),
				("     name: Bob", 2),
				("      name: Bob", 3),
				("name: Bob", 0),
				(" name: Bob", 0),
			};

			foreach ((string line, int countIndent) yanet in yanets)
			{
				Assert.Equal(new Peeker(yanet.line).CountIndent(indent), yanet.countIndent);
			}
		}

		[Fact]
		public void CountIndentTabsTest()
		{
			string indent = "\t";

			(string line, int countIndent)[] yanets = new (string, int)[]
			{
				("\t\tname: John", 2),
				("\tname: Bob", 1),
				("\t\t name: Bob", 2),
				("\t\t\tname: Bob", 3),
				("name: Bob", 0),
				(" name: Bob", 0),
			};

			foreach ((string line, int countIndent) yanet in yanets)
			{
				Assert.Equal(new Peeker(yanet.line).CountIndent(indent), yanet.countIndent);
			}
		}

		[Fact]
		public void StartWithTest()
		{
			Assert.True(new Peeker("name: John").StartWith("nam"));
			Assert.True(new Peeker("\tname: John").StartWith("\t"));

			Assert.False(new Peeker("  name: John").StartWith("\t"));
			Assert.False(new Peeker("\tname: John").StartWith("  "));
		}

		[Fact]
		public void CountIndentExceptionTest()
		{
			string indent = "    "; // 4 spaces;

			Assert.Throws<Exception>(new Action(() => new Peeker("   ").CountIndent(indent))); // 3
			Assert.Throws<Exception>(new Action(() => new Peeker("    ").CountIndent(indent)));// 4
		}
	}
}