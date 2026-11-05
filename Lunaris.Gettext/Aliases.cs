namespace Lunaris.Gettext;

public static partial class Gettext
{
	/// <inheritdoc cref="GetString(string, object[])" />
	public static string __(string msgid, params object[] args) => GetString(msgid, args);

	/// <inheritdoc cref="GetPluralString(string, string, nuint, object[])" />
	public static string __N(string msgid1, string msgid2, nuint n, params object[] args) => GetPluralString(msgid1, msgid2, n, args);
}
