﻿namespace Sharp.StrangeDataTypes
{
	public class ChangedObjectReturn : IAny
	{
		public int idx;
		public HowChanged HowChanged = HowChanged.NotChanged;
	}
}
