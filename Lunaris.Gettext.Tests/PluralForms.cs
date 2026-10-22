namespace Lunaris.Gettext.Tests;

public class PluralForms
{
	[Fact]
	public void Singular()
	{
		Gettext.SetLocale(LC.ALL, "en");

		var result = Gettext.GetPluralString("{0} apple", "{0} apples", 1);

		Assert.Equal("1 apple", result);
	}

	[Fact]
	public void Plural()
	{
		Gettext.SetLocale(LC.ALL, "en");

		var result = Gettext.GetPluralString("{0} apple", "{0} apples", 2);

		Assert.Equal("2 apples", result);
	}
}
