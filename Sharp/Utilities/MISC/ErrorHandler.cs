using Sharp.Exceptions;
using System.Runtime.InteropServices;

namespace Sharp.Utilities.MISC;
public static class ErrorHandler
{
	public static bool IsWindows = true;
	public static bool InitCalled = false;
	public static string[] errors = File.ReadAllLines($"{Environment.CurrentDirectory}\\errors.txt");
	public static void ThrowError(string message, bool crash)
	{
		if (Environment.OSVersion.Platform == PlatformID.Win32NT)
		{
			[DllImport("user32.dll")]
			static extern int MessageBox(nint h, string m, string c, int type);
			_ = MessageBox(nint.Zero, message, "Error", /* 0x01 is MB_ICONERROR (error symbol) and 0x00 is MB_OK (ok message box) */ 0x10 | 0x00);
			if (crash) throw new MessageBoxException(message + " (Shown in message box)");
		}
		else
		{
			if (ErrorTweaks.SayNonWindowsOS)
				throw new MessageBoxException(message + " This OS is not Windows, so the program cannot display a message box. Hide this by disabling ErrorTweaks.SayNonWindowsOS.");
			else
				throw new MessageBoxException(message);
		}
	}
	public static void ThrowError(int messageIdx, bool crash) => ThrowError(messageIdx, [], crash);
	public static void ThrowError(int messageIdx, string[] parameters, bool crash)
	{
		parameters = parameters.Prepend(Environment.CurrentDirectory).ToArray();
		string curMessage = errors[messageIdx - 1];
		int i = 0;
		foreach (string param in parameters)
			curMessage = curMessage.Replace($"#{++i}" /* <== readability! */, param);
		ThrowError(curMessage, crash);
	}
	public static bool YesNoQuestion(string question, string title, bool crashOnNo)
	{
		return true;
	}
}