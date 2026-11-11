using System.Globalization;
using System.Runtime.InteropServices;

namespace Lunaris.Gettext;

/// <summary>
/// Locale categories for controlling how <see cref="Gettext" /> formats messages
/// </summary>
/// <see cref="Gettext.SetLocale(LocaleCategory, string)" />
public enum LocaleCategory
{
	/// <summary>Category for character classification and case-conversion rules</summary>
	CType,
	/// <summary>Category for general numeric rules that don't apply to <see cref="Monetary" />.</summary>
	Numeric,
	/// <summary>Category for dates and times.</summary>
	Time,
	/// <summary>Category for string sorting and character-collation order.</summary>
	Collate,
	/// <summary>Category for currency formatting conventions.</summary>
	Monetary,
	/// <summary>Category for the user interface.</summary>
	Messages,
	/// <summary>All categories.</summary>
	All
}

/// <summary>
/// The main <c>Gettext</c> class.
/// </summary>
/// <example>
/// global using static Lunaris.Gettext;
///
/// BindTextDomain("ExampleApp", "/usr/share/locale");
/// TextDomain("ExampleApp");
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
	/// Gets <paramref name="msgid" /> from the selected domain using <see cref="LocaleCategory.Messages" />.
	/// </summary>
	/// <param name="msgid">The message ID.</param>
	/// <param name="args">String replacements to pass to <c>string.Format</c></param>
	public static string GetString(string msgid, params object[] args)
	{
		nint inputPtr = Marshal.StringToCoTaskMemUTF8(msgid);

		try
		{
			return string.Format(Marshal.PtrToStringUTF8(_gettext(inputPtr))!, args);
		}
		finally
		{
			Marshal.FreeCoTaskMem(inputPtr);
		}
	}

	/// <summary>
	/// Gets <paramref name="msgid" /> from <paramref name="domain" /> using <see cref="LocaleCategory.Messages" />.
	/// </summary>
	/// <param name="domain">The domain to get <paramref name="msgid" /> from.</param>
	/// <param name="msgid">The message ID.</param>
	/// <param name="args">String replacements to pass to <c>string.Format</c></param>
	public static string GetStringFromDomain(string domain, string msgid, params object[] args)
	{
		nint domainnamePtr = Marshal.StringToCoTaskMemUTF8(domain);
		nint inputPtr = Marshal.StringToCoTaskMemUTF8(msgid);

		try
		{
			return string.Format(Marshal.PtrToStringUTF8(_dgettext(domainnamePtr, inputPtr))!, args);
		}
		finally
		{
			Marshal.FreeCoTaskMem(domainnamePtr);
			Marshal.FreeCoTaskMem(inputPtr);
		}
	}

	/// <summary>
	/// Gets <paramref name="msgid" /> from <paramref name="domain" /> using <paramref name="category" />.
	/// </summary>
	/// <param name="domain">The domain to get <paramref name="msgid" /> from.</param>
	/// <param name="category">The <see cref="LocaleCategory"/> to use.</param>
	/// <param name="msgid">The message ID.</param>
	/// <param name="args">String replacements to pass to <c>string.Format</c></param>
	public static string GetStringFromDomainWithCategory(string domain, LocaleCategory category, string msgid, params object[] args)
	{
		nint domainnamePtr = Marshal.StringToCoTaskMemUTF8(domain);
		nint inputPtr = Marshal.StringToCoTaskMemUTF8(msgid);

		try
		{
			return string.Format(Marshal.PtrToStringUTF8(_dcgettext(domainnamePtr, inputPtr, category))!, args);
		}
		finally
		{
			Marshal.FreeCoTaskMem(domainnamePtr);
			Marshal.FreeCoTaskMem(inputPtr);
		}
	}

	/// <summary>
	/// Gets <paramref name="msgid" /> and <paramref name="msgidPlural" /> from the selected domain using <see cref="LocaleCategory.Messages" />.
	/// </summary>
	/// <param name="msgid">The singular message ID.</param>
	/// <param name="msgidPlural">The plural message ID.</param>
	/// <param name="n">The count.</param>
	/// <param name="args">String replacements to pass to <c>string.Format</c>. <c>{0}</c> is always reserved for <paramref name="n" />.</param>
	public static string GetPluralString(string msgid, string msgidPlural, nuint n, params object[] args)
	{
		nint msgidPtr = Marshal.StringToCoTaskMemUTF8(msgid);
		nint msgidPluralPtr = Marshal.StringToCoTaskMemUTF8(msgidPlural);

		try
		{
			List<object> format = [n, .. args];

			return string.Format(Marshal.PtrToStringUTF8(_ngettext(msgidPtr, msgidPluralPtr, n))!, format.ToArray());
		}
		finally
		{
			Marshal.FreeCoTaskMem(msgidPtr);
			Marshal.FreeCoTaskMem(msgidPluralPtr);
		}
	}

	/// <summary>
	/// Gets <paramref name="msgid" /> and <paramref name="msgidPlural" /> from <paramref name="domainname"/> using <see cref="LocaleCategory.Messages" />.
	/// </summary>
	/// <param name="domainname">The domain to get <paramref name="msgid" /> and <paramref name="msgidPlural" /> from</param>
	/// <param name="msgid">The singular message ID.</param>
	/// <param name="msgidPlural">The plural message ID.</param>
	/// <param name="n">The count.</param>
	/// <param name="args">String replacements to pass to <c>string.Format</c>. <c>{0}</c> is always reserved for <paramref name="n" />.</param>
	public static string GetPluralStringFromDomain(string domainname, string msgid, string msgidPlural, nuint n, params object[] args)
	{
		nint domainnamePtr = Marshal.StringToCoTaskMemUTF8(domainname);
		nint msgidPtr = Marshal.StringToCoTaskMemUTF8(msgid);
		nint msgidPluralPtr = Marshal.StringToCoTaskMemUTF8(msgidPlural);

		try
		{
			List<object> format = [n, .. args];

			return string.Format(Marshal.PtrToStringUTF8(_dngettext(domainnamePtr, msgidPtr, msgidPluralPtr, n))!, format.ToArray());
		}
		finally
		{
			Marshal.FreeCoTaskMem(domainnamePtr);
			Marshal.FreeCoTaskMem(msgidPtr);
			Marshal.FreeCoTaskMem(msgidPluralPtr);
		}
	}

	/// <summary>
	/// Gets <paramref name="msgid" /> and <paramref name="msgidPlural" /> from <paramref name="domainname"/> using <paramref name="category"/>.
	/// </summary>
	/// <param name="domainname">The domain to get <paramref name="msgid" /> and <paramref name="msgidPlural" /> from</param>
	/// <param name="category">The <see cref="LocaleCategory"/> to use.</param>
	/// <param name="msgid">The singular message ID.</param>
	/// <param name="msgidPlural">The plural message ID.</param>
	/// <param name="n">The count.</param>
	/// <param name="args">String replacements to pass to <c>string.Format</c>. <c>{0}</c> is always reserved for <paramref name="n" />.</param>
	public static string GetPluralStringFromDomainWithCategory(string domainname, LocaleCategory category, string msgid, string msgidPlural, nuint n, params object[] args)
	{
		nint domainnamePtr = Marshal.StringToCoTaskMemUTF8(domainname);
		nint msgidPtr = Marshal.StringToCoTaskMemUTF8(msgid);
		nint msgidPluralPtr = Marshal.StringToCoTaskMemUTF8(msgidPlural);

		try
		{
			List<object> format = [n, .. args];

			return string.Format(Marshal.PtrToStringUTF8(_dcngettext(domainnamePtr, msgidPtr, msgidPluralPtr, n, category))!, format.ToArray());
		}
		finally
		{
			Marshal.FreeCoTaskMem(domainnamePtr);
			Marshal.FreeCoTaskMem(msgidPtr);
			Marshal.FreeCoTaskMem(msgidPluralPtr);
		}
	}

	/// <summary>
	/// Sets <paramref name="locale" /> for the specified <paramref name="category" />.
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
		nint localeResult;

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
				localeResult = _setlocale(category, formattedLocale);
				if (localeResult == 0) throw new CultureNotFoundException($"\"{locale}\" is not a valid locale");

				return Marshal.PtrToStringUTF8(localeResult);
			}
			catch (CultureNotFoundException)
			{
				try
				{
					// reduce locale to bare language identifier
					formattedLocale = formattedLocale.Split('-')[0];
					localeResult = _setlocale(category, formattedLocale);
					if (localeResult == 0) throw new CultureNotFoundException($"\"{locale}\" is not a valid locale");

					return Marshal.PtrToStringUTF8(localeResult);
				}
				catch (CultureNotFoundException)
				{
					throw new CultureNotFoundException($"\"{locale}\" is not a valid locale");
				}
			}
		}

		localeResult = _setlocale(category, locale);
		if (localeResult == 0) throw new CultureNotFoundException($"\"{locale}\" is not a valid locale");

		return Marshal.PtrToStringUTF8(localeResult);
	}
}
