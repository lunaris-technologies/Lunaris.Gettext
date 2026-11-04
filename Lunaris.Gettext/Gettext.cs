using System.Globalization;
using System.Runtime.InteropServices;

namespace Lunaris.Gettext;

/// <summary>
/// Locale categories for controlling how <see cref="Gettext" /> formats messages
/// </summary>
public enum LocaleCategory
{
	/// <summary>Locale for character classification and case-conversion rules</summary>
	CType,
	/// <summary>Locale for general numeric rules that don't apply to <see cref="Monetary" />.</summary>
	Numeric,
	/// <summary>Locale for dates and times.</summary>
	Time,
	/// <summary>Locale for string sorting and character-collation order.</summary>
	Collate,
	/// <summary>Locale for currency formatting conventions.</summary>
	Monetary,
	/// <summary>Locale for the user interface.</summary>
	Messages,
	/// <summary>All locale categories.</summary>
	All
}

/// <summary>
/// The main <c>Gettext</c> class.
/// </summary>
/// <example>
/// global using static Lunaris.Gettext;
///
/// BindTextDomain("ExampleApp", "/usr/share/locale");
/// SetTextDomain("ExampleApp");
/// BindTextDomainCodeset("ExampleApp", "UTF-8");
///
/// Console.WriteLine(__("Hello, world!"));
/// Console.WriteLine(N__("I have {0} apple.", "I have {0} apples.", 2));
/// </example>
public static partial class Gettext
{
	static Gettext()
	{
		NativeResolver.Initialize();

		SetLocale(LocaleCategory.All, "");
	}

	/// <summary>
	/// Gets <paramref name="msgid" /> from the selected textdomain and locale.
	/// </summary>
	/// <param name="msgid">The message ID.</param>
	/// <param name="args">String replacements to pass to <c>string.Format</c></param>
	public static string GetString(string msgid, params object[] args)
	{
		nint inputPtr = Marshal.StringToCoTaskMemUTF8(msgid);

		try
		{
			nint resultPtr = _gettext(inputPtr);
			if (resultPtr == 0) return string.Format(msgid, args);
			return string.Format(Marshal.PtrToStringUTF8(resultPtr) ?? msgid, args);
		}
		finally
		{
			Marshal.FreeCoTaskMem(inputPtr);
		}
	}

	/// <summary>
	/// Gets <paramref name="msgid1" /> and <paramref name="msgid2" /> from the selected textdomain and locale and their plural forms when applicable.
	/// </summary>
	/// <param name="msgid1">The singular message ID.</param>
	/// <param name="msgid2">The plural message ID.</param>
	/// <param name="n">The count.</param>
	/// <param name="args">String replacements to pass to <c>string.Format</c>. <c>{0}</c> is always reserved for <paramref name="n" />.</param>
	public static string GetPluralString(string msgid1, string msgid2, nuint n, params object[] args)
	{
		nint msgid1Ptr = Marshal.StringToCoTaskMemUTF8(msgid1);
		nint msgid2Ptr = Marshal.StringToCoTaskMemUTF8(msgid2);

		try
		{
			nint resultPtr = _ngettext(msgid1Ptr, msgid2Ptr, n);

			List<object> format = [n, .. args];

			return string.Format(
				resultPtr == 0
					? (n == 1 ? msgid1 : msgid2)
					: Marshal.PtrToStringUTF8(resultPtr) ?? (n == 1 ? msgid1 : msgid2),
				format.ToArray()
			);
		}
		finally
		{
			Marshal.FreeCoTaskMem(msgid1Ptr);
			Marshal.FreeCoTaskMem(msgid2Ptr);
		}
	}

	/// <summary>
	/// Sets <paramref name="locale" /> to the specified <paramref name="category" />.
	/// </summary>
	/// <remarks>
	/// In most cases, you do not need to call this function.
	/// </remarks>
	/// <exception cref="CultureNotFoundException">
	/// <paramref name="locale" /> is not valid
	/// </exception>
	/// <param name="category">The locale category.</param>
	/// <param name="locale">The locale in the POSIX locale identifier format. Leaving this parameter blank derives the locale from the environment.</param>
	/// <seealso cref="LocaleCategory" />
	public static string? SetLocale(LocaleCategory category, string locale)
	{
		if (locale == "") return Marshal.PtrToStringUTF8(_setlocale(category, null));

		// windows is truly a snowflake operating system, because while
		// everyone and everything uses posix/unix conventions, they
		// have to roll their own everything, so we have this compat now.
		if (OperatingSystem.IsWindows())
		{
			// do not compat locale if provided locale passes as IETF BCP 47
			// standard
			if (locale.Contains('-'))
				return Marshal.PtrToStringUTF8(_setlocale(category, locale));

			// underscores must be replaced with hyphens and extra locale
			// identifiers should be stripped because of the IETF BCP 47
			// standard
			string formattedLocale = locale.Replace('_', '-').Split('.')[0].Split('@')[0];

			try
			{
				_ = new CultureInfo(formattedLocale);

				return Marshal.PtrToStringUTF8(_setlocale(category, formattedLocale));
			}
			catch (CultureNotFoundException)
			{
				try
				{
					// reduce locale to bare language identifier
					formattedLocale = formattedLocale.Split('-')[0];
					_ = new CultureInfo(formattedLocale);

					return Marshal.PtrToStringUTF8(_setlocale(category, formattedLocale.Split('-')[0]));
				}
				catch (CultureNotFoundException)
				{
					throw new CultureNotFoundException($"\"{locale}\" is not a valid locale");
				}

			}
		}

		nint localeResult = _setlocale(category, locale);
		if (localeResult == 0) throw new CultureNotFoundException($"\"{locale}\" is not a valid locale");

		return Marshal.PtrToStringUTF8(localeResult);
	}
}
