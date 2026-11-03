namespace Lunaris.Gettext.Tests;

public class Locales
{
	[Fact]
	public void ValidLocale()
	{
		var result = Gettext.SetLocale(LocaleCategory.Messages, "C");

		Assert.Equal("C", result);
	}
}
