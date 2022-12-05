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
		public void CreatePeeker()
		{
			Assert.Throws<Exception>(new Action(() => new Peeker("name: John", 3, 2)));
			Assert.Throws<Exception>(new Action(() => new Peeker("name: John", 13)));

			Assert.Equal(10, new Peeker("name: John", 0, 9).Length);
			Assert.Equal(8, new Peeker("name: John", 1, 8).Length);

			Assert.Equal("ame: Joh", new Peeker("name: John", 1, 8).ToString());
			Assert.Equal("ame: John", new Peeker("name: John", 1, 9).ToString());
			Assert.Equal("ame: John", new Peeker("name: John", 1).ToString());
		}

		[Fact]
		public void FindSubstringCharTest()
		{
			_testOutputHelper.WriteLine("Ok");

			char[][] yanet = new char[][]
			{
				new char[] { 'n', 'a', 'm', 'e', ':', ' ', 'J', 'o', 'h', 'n' },
				new char[] { 'a', 'g', 'e', ':', ' ', '1', '8' }
			};

			(Peeker peeker, char[] substring, int position)[] testFind = new (Peeker, char[], int)[]
			{
				(new Peeker(yanet[0]), new char[] { ':', ' ', 'J' }, 4),
				(new Peeker(yanet[0]), new char[] { 'a', 'm',  }, 1),
				(new Peeker(yanet[0]), new char[] { 'h', 'n', 'i' }, -1),
				(new Peeker(yanet[1]), new char[] { ':', ' ' }, 3),
				(new Peeker(yanet[1]), new char[] { '1', '7' }, -1),

				(new Peeker(yanet[1], 0), new char[] { 'g', 'e' }, 1),
				(new Peeker(yanet[1], 0), new char[] { 'e', ':' }, 2),
				(new Peeker(yanet[1], 0), new char[] { ':', ' ' }, 3),
				(new Peeker(yanet[1], 0), new char[] { ' ', '1' }, 4),

				(new Peeker(yanet[1], 0), new char[] { '1', '8' }, 5),
				(new Peeker(yanet[1], 2), new char[] { '1', '8' }, 5),
				(new Peeker(yanet[1], 4), new char[] { '1', '8' }, 5),
			};

			foreach ((Peeker peeker, char[] substring, int position) item in testFind)
			{
				int currentPosition = item.peeker.IndexOf(item.substring);

				//_testOutputHelper.WriteLine($"{item.peeker.Offset} current: {currentPosition} expected: {item.position}");
				
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

			(Peeker peeker, string substring, int position)[] testFind = new (Peeker, string, int)[]
			{
				(new Peeker(yanet[0]), ": J", 4),
				(new Peeker(yanet[0]), "am", 1),
				(new Peeker(yanet[0]), "hni", -1),
				(new Peeker(yanet[0]), "ohn", 7),
				(new Peeker(yanet[1]), ": ", 3),
				(new Peeker(yanet[1]), "17", -1),

				(new Peeker(yanet[1], 0), ": ", 3),
				(new Peeker(yanet[1], 0), " 1", 4),

				(new Peeker(yanet[1], 0), "18", 5),
				(new Peeker(yanet[1], 2), "1", 5),
				(new Peeker(yanet[1], 4), "18", 5),
				(new Peeker(yanet[1], 5), "18", 5),
			};

			foreach ((Peeker peeker, string substring, int position) item in testFind)
			{
				_testOutputHelper.WriteLine($"yanet: '{item.peeker.ToString()}' offset: {item.peeker.Offset}");

				int currentPosition = item.peeker.IndexOf(item.substring);

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

			(Peeker peeker, int countIndent)[] yanets = new (Peeker, int)[]
			{
				(new Peeker("    name: John"), 2),
				(new Peeker("  name: Bob"), 1),
				(new Peeker("     name: Bob"), 2),
				(new Peeker("      name: Bob"), 3),
				(new Peeker("name: Bob"), 0),
				(new Peeker(" name: Bob"), 0),

				(new Peeker("      name: Bob", 2), 2),
				(new Peeker("      name: Bob", 4), 1),
			};

			foreach ((Peeker peeker, int countIndent) yanet in yanets)
			{
				Assert.Equal(yanet.peeker.CountIndent(indent), yanet.countIndent);
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

			Assert.True(new Peeker("name: John", 2).StartWith("me"));
			Assert.True(new Peeker("name: John", 2, 9).StartWith("me:"));


			Assert.False(new Peeker("\tname: John", 1).StartWith("\t"));
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