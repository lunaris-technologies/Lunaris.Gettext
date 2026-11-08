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

	[Fact]
	public void Translated()
	{
		Gettext.SetLocale(LocaleCategory.Messages, "eo");
		Gettext.BindTextDomain("Lunaris.Gettext.Tests", $"{AppContext.BaseDirectory}locale");
		Gettext.TextDomain("Lunaris.Gettext.Tests");
		Gettext.BindTextDomainCodeset("Lunaris.Gettext.Tests", "UTF-8");

		var result = Gettext.GetString("I have an apple.");

		Assert.Equal("Mi havas pomon.", result);
	}

	[Fact]
	public void TranslationConcatenated()
	{
		Gettext.SetLocale(LocaleCategory.Messages, "eo");
		Gettext.BindTextDomain("Lunaris.Gettext.Tests", $"{AppContext.BaseDirectory}locale");
		Gettext.TextDomain("Lunaris.Gettext.Tests");
		Gettext.BindTextDomainCodeset("Lunaris.Gettext.Tests", "UTF-8");

		var result = Gettext.GetString("My name is {0}.", "Johano");

		Assert.Equal("Mia nomo estas Johano.", result);
	}
}
