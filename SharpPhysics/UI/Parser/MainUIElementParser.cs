using SharpPhysics._2d.ObjectRepresentation;
using SharpPhysics.UI.Representation;
namespace SharpPhysics.UI.Parser
{
	public static class MainUIElementParser
	{
		public static UIElement[] elements = Array.Empty<UIElement>();
		public static void ParseXaml(string Pagepath)
		{
			string[] str = File.ReadAllLines(Pagepath);
			// Tokenizing

		}
	}
}
