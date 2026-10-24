using System.Runtime.InteropServices;
using System.Text;

namespace Lunaris.Gettext;

/// <summary>
/// The main <c>Gettext</c> class.
/// </summary>
/// <example>
/// global using static Lunaris.Gettext;
/// SetLocale(LC.ALL, "eo"); // You do not need to do this manually, but this is here for the sake of demonstration.
/// BindTextDomain("ExampleApp", "/usr/share/locale");
/// SetTextDomain("ExampleApp");
/// BindTextDomainCodeset("ExampleApp", "UTF-8");
///
/// Console.WriteLine(__("Hello, world!"));
///                 // -> Saluton, mondo!
///
/// // Plural forms
/// Console.WriteLine(N__(
///     "I have {0} apple.",  // Singular form
///     "I have {0} apples.", // Plural form
///     2                     // Count
/// ));
/// // -> Mi havas 2 pomoj
/// </example>
public static partial class Gettext
{
	[LibraryImport("libc.so.6", EntryPoint = "gettext", StringMarshalling = StringMarshalling.Utf8)]
	private static partial nint _GetString(nint msgid);

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
			nint resultPtr = _GetString(inputPtr);
			if (resultPtr == 0) return string.Format(msgid, args);
			return string.Format(Marshal.PtrToStringUTF8(resultPtr) ?? msgid, args);
		}
		finally
		{
			Marshal.FreeCoTaskMem(inputPtr);
		}
	}
	/// <inheritdoc cref="GetString(string, object[])" />
	public static string __(string msgid, params object[] args) => GetString(msgid, args);

	[LibraryImport("libc.so.6", EntryPoint = "ngettext", StringMarshalling = StringMarshalling.Utf8)]
	private static partial nint _GetPluralString(nint msgid1, nint msgid2, nuint n);

	/// <summary>
	/// Gets <paramref name="msgid1" /> and <paramref name="msgid2" /> from the selected textdomain and locale and their plural forms when applicable.
	/// </summary>
	/// <param name="msgid1">The singular message ID.</param>
	/// <param name="msgid2">The plural message ID.</param>
	/// <param name="n">The count.</param>
	/// <param name="args">String replacements to pass to <c>string.Format</c></param>
	public static string GetPluralString(string msgid1, string msgid2, nuint n, params object[] args)
	{
		nint msgid1Ptr = Marshal.StringToCoTaskMemUTF8(msgid1);
		nint msgid2Ptr = Marshal.StringToCoTaskMemUTF8(msgid2);

		try
		{
			nint resultPtr = _GetPluralString(msgid1Ptr, msgid2Ptr, n);

			string format = resultPtr == 0
				? (n == 1 ? msgid1 : msgid2)
				: Marshal.PtrToStringUTF8(resultPtr)
					?? (n == 1 ? msgid1 : msgid2);

			return string.Format(format, n, args);
		}
		finally
		{
			Marshal.FreeCoTaskMem(msgid1Ptr);
			Marshal.FreeCoTaskMem(msgid2Ptr);
		}
	}
	/// <inheritdoc cref="GetPluralString(string, string, nuint, object[])" />
	public static string N__(string msgid1, string msgid2, nuint n, params object[] args) => GetPluralString(msgid1, msgid2, n, args);

	/// <summary>
	/// Sets the active message domain.
	/// </summary>
	/// <param name="domainname">The message domain.</param>
	[LibraryImport("libc.so.6", EntryPoint = "textdomain", StringMarshalling = StringMarshalling.Utf8)]
	public static partial nint SetTextDomain(string domainname);

	/// <summary>
	/// Sets the active message domain.
	/// </summary>
	/// <param name="domainname">The message domain.</param>
	/// <param name="dirname">The directory of stored locales. This should be <c>/usr/share/locale</c>.</param>
	[LibraryImport("libc.so.6", EntryPoint = "bindtextdomain", StringMarshalling = StringMarshalling.Utf8)]
	public static partial nint BindTextDomain(string domainname, string dirname);

	/// <summary>
	/// Sets the active message domain's codeset.
	/// </summary>
	/// <param name="domainname">The message domain.</param>
	/// <param name="codeset">The codeset. In most cases, this should be <c>UTF-8</c>.</param>
	[LibraryImport("libc.so.6", EntryPoint = "bind_textdomain_codeset", StringMarshalling = StringMarshalling.Utf8)]
	public static partial nint BindTextDomainCodeset(string domainname, string codeset);
}
