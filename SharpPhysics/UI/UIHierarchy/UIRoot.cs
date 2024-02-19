using SharpPhysics.UI.UIElements;

namespace SharpPhysics.UI.UIHierarchy
{
	public static class UIRoot
	{
		public static IUIElement[] NonWindowedObjects = [];
		public static UIWindow[] Windows = [new("Debug Window", [new Label("Add objects here through the SharpPhysics.UI namespace!"), new DecimalSlider(-1, 1)])];
	}
}
