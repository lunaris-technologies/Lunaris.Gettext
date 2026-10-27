namespace Lunaris.Gettext.Tests;

public class SingularForms
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
		var result = Gettext.GetString("The quick brown {1} jumped over the lazy {0}", "dog", "fox");

		Assert.Equal("The quick brown fox jumped over the lazy dog", result);
	}
}
