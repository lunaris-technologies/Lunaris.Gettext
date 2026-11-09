# Lunaris.Gettext

A C# library that directly interfaces with the native gettext library.

The syntax is meant to be near-exact with how you would utilize gettext via C,
including shorthands, with the exception of certain function design changes for
standard C# conventions.

String concatenation works the same as `string.Format` (`{0}`, `{1}`, etc.).

## Dependencies

gettext is the only **runtime dependency**. If you are on Linux, it comes
pre-installed. On macOS, it can be installed via Homebrew, and there are
packages available for gettext on winget. Git Bash being installed is enough, as
it also pulls in gettext as a runtime dependency.

## Example

```cs
global using static Lunaris.Gettext;

BindTextDomain("ExampleApp", "/usr/share/locale");
TextDomain("ExampleApp");
BindTextDomainCodeset("ExampleApp", "UTF-8");

Console.WriteLine(GetString("Hello, world!"));
                // -> Saluton, mondo!

// Plural forms
Console.WriteLine(
  GetPluralString(
    "I have {0} apple.",  // Singular form
    "I have {0} apples.", // Plural form
    2                     // Count
  )
); // -> Mi havas 2 pomoj
```

## Usage with xgettext

To use Lunaris.Gettext with xgettext, use these flags:

```sh
xgettext \
	--output ExampleApp.pot \
	--from-code UTF-8 \
	--keyword \
	--keyword=GetString \
	--keyword=GetPluralString:1,2 \
	--keyword=__ \
	--keyword=N__:1,2 \
	Program.cs
```
