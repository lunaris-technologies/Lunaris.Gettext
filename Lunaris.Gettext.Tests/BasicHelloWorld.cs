namespace Lunaris.Gettext.Tests;

public class BasicHelloWorld
{
	[Fact]
	public void Test()
	{
		Gettext.SetLocale(LC.ALL, "en");

		var result = Gettext.GetString("Hello, world!");

		Assert.Equal("Hello, world!", result.ToString());
	}
}
