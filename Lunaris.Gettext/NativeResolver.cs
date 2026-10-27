using System.Reflection;
using System.Runtime.InteropServices;

namespace Lunaris.Gettext;

internal static class NativeResolver
{
	internal static void Initialize()
	{
		NativeLibrary.SetDllImportResolver(typeof(Gettext).Assembly, ResolveNativeLibrary);
	}

	private static IntPtr ResolveNativeLibrary(string libraryName, Assembly assembly, DllImportSearchPath? searchPath)
	{
		string[] fallbacks = GetPlatformFallbacks();

		foreach (var candidate in fallbacks)
		{
			if (NativeLibrary.TryLoad(candidate, assembly, searchPath, out IntPtr handle))
			{
				return handle;
			}
		}

		throw new DllNotFoundException("gettext is not installed on the system");
	}

	private static string[] GetPlatformFallbacks()
	{
		return true switch
		{
			_ when OperatingSystem.IsWindows() => ["libintl-9.dll", "libintl-8.dll", "libintl.dll"],
			_ when OperatingSystem.IsLinux() => ["libc.so.6", "libintl.so", "libgnuintl.so"],
			_ when OperatingSystem.IsMacOS() => ["libintl.dylib"],
			_ when OperatingSystem.IsFreeBSD() => ["libintl.so"],
			_ => []
		};
	}
}
