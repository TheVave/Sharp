
namespace SharpPhysics
{
    public sealed class _2dMovmentRepresenter
    {
		/// <summary>
		/// Represents the starting position of the "line"
		/// </summary>
        public _2dPosition StartPosition;

		/// <summary>
		/// Represents the ending position of the "line"
		/// </summary>
		public _2dPosition EndPosition;

        /// <summary>
        /// WARNING: this code creates a MovmentRepersenter with the StartPosition of 0,0,0.
        /// </summary>
        /// <param name="endPosition"></param>
        public _2dMovmentRepresenter(_2dPosition endPosition)
        {
            StartPosition = new(0, 0, 0);
            EndPosition = endPosition;
        }

		/// <summary>
		/// Creates a MovementRepresenter that will start at startPosition and end at endPosition.
		/// </summary>
		/// <param name="startPosition"></param>
		/// <param name="endPosition"></param>
        public _2dMovmentRepresenter(_2dPosition startPosition, _2dPosition endPosition)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
        }
    }
}
