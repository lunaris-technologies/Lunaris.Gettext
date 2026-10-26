namespace Lunaris.Gettext.Tests;

public class PluralForms
{
	[Fact]
	public void Singular()
	{
		var result = Gettext.GetPluralString("{0} apple", "{0} apples", 1);

		Assert.Equal("1 apple", result);
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
			"My name is {1} and I have {0} apple",
			"My name is {1} and I have {0} apples",
			1, "John"
		);

		Assert.Equal("My name is John and I have 1 apple", result);
	}

	[Fact]
	public void PluralConcatenated()
	{
		var result = Gettext.GetPluralString(
			"My name is {1} and I have {0} apple",
			"My name is {1} and I have {0} apples",
			2, "John"
		);

		Assert.Equal("My name is John and I have 2 apples", result);
	}
}
