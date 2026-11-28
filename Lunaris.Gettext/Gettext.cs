using System.Globalization;
using System.Runtime.InteropServices;

namespace Lunaris.Gettext;

/// <summary>
/// Locale categories for controlling how <see cref="Gettext" /> formats messages
/// </summary>
/// <seealso cref="Gettext.SetLocale(LocaleCategory, string)" />
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
	/// <remarks>Not supported in Windows. Use <see cref="All" /> instead.</remarks>
	Messages,
	/// <summary>All categories.</summary>
	All
}

/// <summary>
/// The main <c>Gettext</c> class.
/// </summary>
/// <seealso cref="GetString" />
/// <seealso cref="GetPluralString" />
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
		=> string.Format(Marshal.PtrToStringUTF8(_gettext(msgid))!, args);

	/// <summary>
	/// Gets <paramref name="msgid" /> from <paramref name="domain" /> using <see cref="LocaleCategory.Messages" />.
	/// </summary>
	/// <param name="domain">The domain to get <paramref name="msgid" /> from.</param>
	/// <param name="msgid">The message ID.</param>
	/// <param name="args">String replacements to pass to <c>string.Format</c></param>
	public static string GetStringFromDomain(string domain, string msgid, params object[] args)
		=> string.Format(Marshal.PtrToStringUTF8(_dgettext(domain, msgid))!, args);

	/// <summary>
	/// Gets <paramref name="msgid" /> from <paramref name="domain" /> using <paramref name="category" />.
	/// </summary>
	/// <param name="domain">The domain to get <paramref name="msgid" /> from.</param>
	/// <param name="category">The <see cref="LocaleCategory"/> to use.</param>
	/// <param name="msgid">The message ID.</param>
	/// <param name="args">String replacements to pass to <c>string.Format</c></param>
	public static string GetStringFromDomainWithCategory(string domain, LocaleCategory category, string msgid, params object[] args)
		=> string.Format(Marshal.PtrToStringUTF8(_dcgettext(domain, msgid, category))!, args);

	/// <summary>
	/// Gets <paramref name="msgid" /> and <paramref name="msgidPlural" /> from the selected domain using <see cref="LocaleCategory.Messages" />.
	/// </summary>
	/// <param name="msgid">The singular message ID.</param>
	/// <param name="msgidPlural">The plural message ID.</param>
	/// <param name="n">The count.</param>
	/// <param name="args">String replacements to pass to <c>string.Format</c>. <c>{0}</c> is always reserved for <paramref name="n" />.</param>
	public static string GetPluralString(string msgid, string msgidPlural, int n, params object[] args)
		=> string.Format(Marshal.PtrToStringUTF8(_ngettext(msgid, msgidPlural, n))!, [n, .. args]);

	/// <summary>
	/// Gets <paramref name="msgid" /> and <paramref name="msgidPlural" /> from <paramref name="domainname"/> using <see cref="LocaleCategory.Messages" />.
	/// </summary>
	/// <param name="domainname">The domain to get <paramref name="msgid" /> and <paramref name="msgidPlural" /> from</param>
	/// <param name="msgid">The singular message ID.</param>
	/// <param name="msgidPlural">The plural message ID.</param>
	/// <param name="n">The count.</param>
	/// <param name="args">String replacements to pass to <c>string.Format</c>. <c>{0}</c> is always reserved for <paramref name="n" />.</param>
	public static string GetPluralStringFromDomain(string domainname, string msgid, string msgidPlural, int n, params object[] args)
		=> string.Format(Marshal.PtrToStringUTF8(_dngettext(domainname, msgid, msgidPlural, n))!, [n, .. args]);

	/// <summary>
	/// Gets <paramref name="msgid" /> and <paramref name="msgidPlural" /> from <paramref name="domainname"/> using <paramref name="category"/>.
	/// </summary>
	/// <param name="domainname">The domain to get <paramref name="msgid" /> and <paramref name="msgidPlural" /> from</param>
	/// <param name="category">The <see cref="LocaleCategory"/> to use.</param>
	/// <param name="msgid">The singular message ID.</param>
	/// <param name="msgidPlural">The plural message ID.</param>
	/// <param name="n">The count.</param>
	/// <param name="args">String replacements to pass to <c>string.Format</c>. <c>{0}</c> is always reserved for <paramref name="n" />.</param>
	public static string GetPluralStringFromDomainWithCategory(string domainname, LocaleCategory category, string msgid, string msgidPlural, int n, params object[] args)
		=> string.Format(Marshal.PtrToStringUTF8(_dcngettext(domainname, msgid, msgidPlural, n, category))!, [n, .. args]);

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
	public static string SetLocale(LocaleCategory category, string locale)
	{
		// windows is truly a snowflake operating system, because while
		// everyone and everything uses posix/unix conventions, they
		// have to roll their own everything, so we have this compat now.
		if (OperatingSystem.IsWindows())
		{
			// do not compat locale if provided locale passes as IETF BCP 47
			// standard
			if (locale.Contains('-') || locale.StartsWith("."))
				return Marshal.PtrToStringUTF8(_setlocale(category, locale))!;

			// set locale to a generic locale if "C" or "POSIX" is passed
			if (locale == "C" || locale == "POSIX")
				return Marshal.PtrToStringUTF8(_setlocale(category, "C"))!;

			// underscores must be replaced with hyphens and extra locale
			// identifiers should be stripped because of the IETF BCP 47
			// standard
			string formattedLocale = locale.Replace('_', '-').Split('.')[0].Split('@')[0];

			try
			{
				return Marshal.PtrToStringUTF8(_setlocale(category, formattedLocale))!
					   ?? throw new CultureNotFoundException($"\"{locale}\" is not a valid locale");
			}
			catch (CultureNotFoundException)
			{
				// reduce locale to bare language identifier
				return Marshal.PtrToStringUTF8(_setlocale(category, formattedLocale.Split('-')[0]))!
					   ?? throw new CultureNotFoundException($"\"{locale}\" is not a valid locale");
			}
		}

		return Marshal.PtrToStringUTF8(_setlocale(category, locale))
			   ?? throw new CultureNotFoundException($"\"{locale}\" is not a valid locale");
	}
}
