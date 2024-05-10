using Sharp.StrangeDataTypes;

namespace Sharp.UI.UIHierarchy
{
	public class UIWindow(string title, IUIElement[] elements) : IAny
	{
		public string Title { get; set; } = title;
		public IUIElement[] Elements = elements;
	}
}
