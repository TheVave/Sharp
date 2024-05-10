
using Sharp._2d.ObjectRepresentation.Translation;
using Sharp.StrangeDataTypes;

namespace Sharp._2d.Physics
{
	[Serializable]
	public sealed class _2dMovementRepresenter : IAny
	{
		/// <summary>
		/// Represents the starting position of the "line"
		/// </summary>
		public Position StartPosition;

		/// <summary>
		/// Represents the ending position of the "line"
		/// </summary>
		public Position EndPosition;

		/// <summary>
		/// WARNING: this code creates a MovementRepresenter with the StartPosition of 0,0,0.
		/// </summary>
		/// <param name="endPosition"></param>
		public _2dMovementRepresenter(Position endPosition)
		{
			StartPosition = new(0, 0, 0);
			EndPosition = endPosition;
		}

		/// <summary>
		/// Creates a MovementRepresenter that will start at startPosition and end at endPosition.
		/// </summary>
		/// <param name="startPosition"></param>
		/// <param name="endPosition"></param>
		public _2dMovementRepresenter(Position startPosition, Position endPosition)
		{
			StartPosition = startPosition;
			EndPosition = endPosition;
		}
	}
}
