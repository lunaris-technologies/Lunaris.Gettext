using System.Runtime.InteropServices;

namespace Lunaris.Gettext;

public static partial class Gettext
{
	#region Internal Classes
	[LibraryImport("gettext", EntryPoint = "gettext", StringMarshalling = StringMarshalling.Utf8)]
	private static partial nint _gettext(nint msgid);

	[LibraryImport("gettext", EntryPoint = "dgettext", StringMarshalling = StringMarshalling.Utf8)]
	private static partial nint _dgettext(nint domain, nint msgid);

	[LibraryImport("gettext", EntryPoint = "dcgettext", StringMarshalling = StringMarshalling.Utf8)]
	private static partial nint _dcgettext(nint domain, nint msgid, LocaleCategory category);

	[LibraryImport("gettext", EntryPoint = "ngettext", StringMarshalling = StringMarshalling.Utf8)]
	private static partial nint _ngettext(nint msgid1, nint msgid2, nuint n);

	[LibraryImport("gettext", EntryPoint = "dngettext", StringMarshalling = StringMarshalling.Utf8)]
	private static partial nint _dngettext(nint domainname, nint msgid1, nint msgid2, nuint n);

	[LibraryImport("gettext", EntryPoint = "dcngettext", StringMarshalling = StringMarshalling.Utf8)]
	private static partial nint _dcngettext(nint domainname, nint msgid1, nint msgid2, nuint n, LocaleCategory category);

	[LibraryImport("libc", EntryPoint = "setlocale", StringMarshalling = StringMarshalling.Utf8)]
	private static partial nint _setlocale(LocaleCategory category, string? locale);
	#endregion

	#region Public Classes
	/// <summary>
	/// Sets the active message domain.
	/// </summary>
	/// <param name="domainname">The message domain.</param>
	[LibraryImport("gettext", EntryPoint = "textdomain", StringMarshalling = StringMarshalling.Utf8)]
	public static partial nint SetTextDomain(string domainname);

	/// <summary>
	/// Sets the active message domain.
	/// </summary>
	/// <param name="domainname">The message domain.</param>
	/// <param name="dirname">The directory of stored locales. This should be <c>/usr/share/locale</c>, or the target platform's equivalent.</param>
	[LibraryImport("gettext", EntryPoint = "bindtextdomain", StringMarshalling = StringMarshalling.Utf8)]
	public static partial nint BindTextDomain(string domainname, string dirname);

	/// <summary>
	/// Sets the active message domain's codeset.
	/// </summary>
	/// <param name="domainname">The message domain.</param>
	/// <param name="codeset">The codeset. In most cases, this should be <c>UTF-8</c>.</param>
	[LibraryImport("gettext", EntryPoint = "bind_textdomain_codeset", StringMarshalling = StringMarshalling.Utf8)]
	public static partial nint BindTextDomainCodeset(string domainname, string codeset);
	#endregion
}
