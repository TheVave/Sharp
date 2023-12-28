namespace SharpPhysics
{
	public static class MainUIElementPackager
	{
		public static UIElement[] Addons = Array.Empty<UIElement>();
		public static string? PackageLocation;
		public static string? UILocation;
		public static void PackageStandard()
		{
			if (PackageLocation is null || UILocation is null)
				ErrorHandler.ThrowError("Error:Internal Error \n\n" +
									"Error info: Package error: SharpPhysics.dll, MAINUIELEMENTPACKAGER.cs,18, PACKAGELOCATION or UILOCATION var is null, the process can't compile the app.",
									false);
			Directory.CreateDirectory($@"{PackageLocation}\resources");
			Directory.CreateDirectory($@"{PackageLocation}\resources\ui");
			ErrorHandler.ThrowNotImplementedExcepetion();
			Environment.Exit(0xFFFF);
			//File.WriteAllText($@"{PackageLocation}\resources\ui\UIManifest.dat");
		}
		private static void PackageStandardGetManifest()
		{
			try
			{
				int flcount = 0;
				string internalName;
				foreach (string uiPage in Directory.GetFiles(UILocation))
				{
					internalName = File.ReadAllLines(uiPage)[0];
					flcount++;
				}
				if (flcount == 0) ErrorHandler.ThrowError("Error: External Error, missing UI files for packaging. \n" +
					" fix this by creating a UI file in the directory " + UILocation, false);

			}
			catch
			{
				ErrorHandler.ThrowError("Error: Internal Error \n\n" +
					"Error info: Package error: SharpPhysics.dll, MAINUIELEMENTPACKAGER.cs,37, UILOCATION is invalid or does not contain any files", false);
			}
		}
		public static void PackagePreprocessed()
		{
			ErrorHandler.ThrowNotImplementedExcepetion();
		}
	}
}
