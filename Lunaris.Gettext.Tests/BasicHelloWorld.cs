namespace Lunaris.Gettext.Tests;

public class BasicHelloWorld
{
	[Fact]
	public void Plain()
	{
		var result = Gettext.GetString("The quick brown fox jumped over the lazy dog");

		Assert.Equal("The quick brown fox jumped over the lazy dog", result);
	}

	[Fact]
	public void Concatenated()
	{
		var result = Gettext.GetString("The quick brown {0} jumped over the lazy {1}", "fox", "dog");

		Assert.Equal("The quick brown fox jumped over the lazy dog", result);
	}
}
