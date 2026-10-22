# Lunaris.Gettext

A C# library that directly interfaces with the native gettext library.

The syntax is meant to be near-exact with how you would utilize gettext via C,
including shorthands.

String concatenation works the same as `string.Format` (`{0}`, `{1}`, etc.).

## Example

```cs
global using static Lunaris.Gettext;

SetLocale(LC.ALL, "eo"); // You do not need to do this manually, but this is here for the sake of demonstration.
BindTextDomain("ExampleApp", "/usr/share/locale");
SetTextDomain("ExampleApp");
BindTextDomainCodeset("ExampleApp", "UTF-8");

Console.WriteLine(__("Hello, world!"));
                // -> Saluton, mondo!

// Plural forms
Console.WriteLine(N__(
    "I have {0} apple.",  // Singular form
    "I have {0} apples.", // Plural form
    2                     // Count
));
// -> Mi havas 2 pomoj
```
