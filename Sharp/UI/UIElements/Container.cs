﻿using Sharp.StrangeDataTypes;
using System.Numerics;

namespace Sharp.UI.UIElements
{
	public class Container : IUIElement, IAny
	{
		public IUIElement[] Children;
		public bool Visible { get; set; } = true;
		public Action OnDraw = () => { };
		public Vector2 Position { get; set; } = Vector2.Zero;

		public bool Draw()
		{
			foreach (IUIElement child in Children)
			{
				child.Draw();
			}
			OnDraw.Invoke();
			return true;
		}
	}
}
