using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics._2d.ObjectRepresentation.Hierarchies;
using SharpPhysics.Utilities.MISC.Errors;

namespace SharpPhysics
{
	public static class SaveHelper
	{
		/// <summary>
		/// Indicates the number of saves that SharpPhysics will attempt to do before over-writing one.
		/// </summary>
		public static short MaxSaveCount { get; set; } = 10;
		public static void SaveFileStandardWrite(string[] data, string saveName)
		{
			new Thread(() =>
			{
				// last save
				if (!File.Exists($@"{Environment.CurrentDirectory}\Saves\ls.txt"))
				{
					if (ErrorHandler.YesNoQuestion($"Error, External error, Missing ls.txt (required file) in {Environment.CurrentDirectory}\\Saves\\{saveName}, do you want to attempt to fix this?", "?", false))
					{
						try
						{
							// ls.txt is a file that stores the save number.
							// It may have the contents of 8 to write to the 8th
							// game save file.
							File.WriteAllText($@"{Environment.CurrentDirectory}\Saves\ls.txt", "0");
						}
						catch
						{
							// if the directory does not exist or another reason
							ErrorHandler.ThrowError("Error, Internal error, unknown cause.", true);
						}
					}
				}
				// if the save dir does not exist create it
				if (!Directory.Exists($@"{Environment.CurrentDirectory}\Saves")) Directory.CreateDirectory($@"{Environment.CurrentDirectory}\Saves");
				// if the game save dir does not exist create it
				if (!Directory.Exists($@"{Environment.CurrentDirectory}\Saves\{saveName}")) Directory.CreateDirectory($@"{Environment.CurrentDirectory}\Saves\{saveName}");
				try
				{
					for (int i = 0; i < MaxSaveCount; i++)
					{
						if (!File.Exists($@"{Environment.CurrentDirectory}\Saves\{saveName}\{saveName}.{MaxSaveCount}.dat"))
						{
							File.WriteAllLines($@"{Environment.CurrentDirectory}\Saves\{saveName}\{saveName}.{MaxSaveCount}.dat", data);
						}
					}
				}
				catch
				{
					ErrorHandler.ThrowError("Error, Internal error, unknown cause", true);
				}
			});
		}
		/// <summary>
		/// Not currently finished.
		/// </summary>
		/// <param name="hierarchy"></param>
		public static void SaveObjects(_2dSceneHierarchy hierarchy)
		{
			return;
			string[] data = new string[0];
			foreach (SimulatedObject2d obj in hierarchy.Objects)
			{

			}
		}
	}
}
