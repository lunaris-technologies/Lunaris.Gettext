global using static Lunaris.Gettext.Gettext;
using Lunaris.Gettext;

internal class Program
{
	internal static void Main()
	{
		SetLocale(LC.ALL, "en");

		Console.WriteLine(__("Hello, world!"));
		Console.WriteLine(N__("I have {0} apple.", "I have {0} apples.", 2));
	}
}
