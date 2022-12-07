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

				Assert.Equal(currentPosition, item.position);
			}
		}

		[Fact]
		public void FindSubstringStringTest()
		{
			(Peeker peeker, string substring, int position)[] testFind = new (Peeker, string, int)[]
			{
				(new Peeker("name: John"), ": J", 4),
				(new Peeker("name: John"), "am", 1),
				(new Peeker("name: John"), "hni", -1),
				(new Peeker("name: John"), "ohn", 7),
				(new Peeker("age: 18"), ": ", 3),
				(new Peeker("age: 18"), "17", -1),

				(new Peeker("age: 18", 0), ": ", 3),
				(new Peeker("age: 18", 0), " 1", 4),

				(new Peeker("age: 18", 0), "18", 5),
				(new Peeker("age: 18", 2), "1", 5),
				(new Peeker("age: 18", 4), "18", 5),
				(new Peeker("age: 18", 5), "18", 5),
				(new Peeker("age: 18", 2), "ge", -1),

				(new Peeker("personName: John", 4), "Name", 6),
			};

			foreach ((Peeker peeker, string substring, int position) item in testFind)
			{
				int currentPosition = item.peeker.IndexOf(item.substring);

				Assert.Equal(currentPosition, item.position);
			}
		}

		[Fact]
		public void FindIndexOfException()
		{
			Assert.Throws<Exception>(new Action(() => 
				new Peeker(new char[] { ':', ' ' }).IndexOf(new char[] { ':', ' ', 'J' })));

			Assert.Throws<Exception>(new Action(() => 
				new Peeker(new char[] { }).IndexOf(new char[] { })));

			Assert.Throws<Exception>(new Action(() => 
				new Peeker("name: Bob", 7).IndexOf("Bob")));
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

			(Peeker peeker, int countIndent)[] yanets = new (Peeker, int)[]
			{
				(new Peeker("\t\tname: John"), 2),
				(new Peeker("\tname: Bob"), 1),
				(new Peeker("\t\t name: Bob"), 2),
				(new Peeker("\t\t\tname: Bob"), 3),
				(new Peeker("name: Bob"), 0),
				(new Peeker(" name: Bob"), 0),
			};

			foreach ((Peeker peeker, int countIndent) yanet in yanets)
			{
				Assert.Equal(yanet.peeker.CountIndent(indent), yanet.countIndent);
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

		[Fact]
		public void ComparePeekers()
		{
			Assert.True(new Peeker("person: John") == "person: John");
			Assert.True(new Peeker("person: John", 0) == "person: John");
			Assert.True(new Peeker("person: John", 1) == "erson: John");
			Assert.True(new Peeker("person: John", 1, 5) == "erson");
		}
	}
}