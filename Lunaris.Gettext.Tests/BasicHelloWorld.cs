namespace Lunaris.Gettext.Tests;

public class BasicHelloWorld
{
	[Fact]
	public void Test()
	{
		var result = Gettext.GetString("Hello, world!");

		Assert.Equal("Hello, world!", result);
	}
}
