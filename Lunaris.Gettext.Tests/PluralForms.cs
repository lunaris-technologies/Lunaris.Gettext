namespace Lunaris.Gettext.Tests;

public class PluralForms
{
	[Fact]
	public void Singular()
	{
		var result = Gettext.GetPluralString("One apple", "{0} apples", 1);

		Assert.Equal("One apple", result);
	}

	[Fact]
	public void Plural()
	{
		var result = Gettext.GetPluralString("{0} apple", "{0} apples", 2);

		Assert.Equal("2 apples", result);
	}

	[Fact]
	public void SingularConcatenated()
	{
		var result = Gettext.GetPluralString(
			"My name is {1} and I have one apple",
			"My name is {1} and I have {0} apples",
			1, "John"
		);

		Assert.Equal("My name is John and I have one apple", result);
	}

	[Fact]
	public void PluralConcatenated()
	{
		var result = Gettext.GetPluralString(
			"My name is {1} and I have one apple",
			"My name is {1} and I have {0} apples",
			2, "John"
		);

		Assert.Equal("My name is John and I have 2 apples", result);
	}

	[Fact]
	public void SingularTranslated()
	{
		Gettext.SetLocale(LocaleCategory.Messages, "eo");
		Gettext.BindTextDomain("Lunaris.Gettext.Tests", $"{AppContext.BaseDirectory}locale");
		Gettext.SetTextDomain("Lunaris.Gettext.Tests");
		Gettext.BindTextDomainCodeset("Lunaris.Gettext.Tests", "UTF-8");

		var result = Gettext.GetPluralString("I have one apple.", "I have {0} apples.", 1);

		Assert.Equal("Mi havas unu pomon.", result);
	}

	[Fact]
	public void SingularTranslationConcatenated()
	{
		Gettext.SetLocale(LocaleCategory.Messages, "eo");
		Gettext.BindTextDomain("Lunaris.Gettext.Tests", $"{AppContext.BaseDirectory}locale");
		Gettext.SetTextDomain("Lunaris.Gettext.Tests");
		Gettext.BindTextDomainCodeset("Lunaris.Gettext.Tests", "UTF-8");

		var result = Gettext.GetPluralString(
			"My name is {1} and I have one apple.",
			"My name is {1} and I have {0} apples.",
			1, "Johano"
		);

		Assert.Equal("Mia nomo estas Johano kaj mi havas unu pomon.", result);
	}

	[Fact]
	public void PluralTranslated()
	{
		Gettext.SetLocale(LocaleCategory.Messages, "eo");
		Gettext.BindTextDomain("Lunaris.Gettext.Tests", $"{AppContext.BaseDirectory}locale");
		Gettext.SetTextDomain("Lunaris.Gettext.Tests");
		Gettext.BindTextDomainCodeset("Lunaris.Gettext.Tests", "UTF-8");

		var result = Gettext.GetPluralString("I have one apple.", "I have {0} apples.", 2);

		Assert.Equal("Mi havas 2 pomojn.", result);
	}

	[Fact]
	public void PluralTranslationConcatenated()
	{
		Gettext.SetLocale(LocaleCategory.Messages, "eo");
		Gettext.BindTextDomain("Lunaris.Gettext.Tests", $"{AppContext.BaseDirectory}locale");
		Gettext.SetTextDomain("Lunaris.Gettext.Tests");
		Gettext.BindTextDomainCodeset("Lunaris.Gettext.Tests", "UTF-8");

		var result = Gettext.GetPluralString(
			"My name is {1} and I have one apple.",
			"My name is {1} and I have {0} apples.",
			2, "Johano"
		);

		Assert.Equal("Mia nomo estas Johano kaj mi havas 2 pomojn.", result);
	}
}
