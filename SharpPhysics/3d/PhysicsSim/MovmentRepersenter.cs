using SharpPhysics;

namespace SharpPhysics
{
    public sealed class MovmentRepersenter
    {
        public Translation.Position StartPosition;
        public Translation.Position EndPosition;

        /// <summary>
        /// WARNING: this code creates a MovmentRepersenter with the StartPosition of 0,0,0.
        /// </summary>
        /// <param name="endPosition"></param>
        public MovmentRepersenter(Translation.Position endPosition)
        {
            StartPosition = new(0, 0, 0);
            EndPosition = endPosition;
        }

        public MovmentRepersenter(Translation.Position startPosition, Translation.Position endPosition)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
        }
    }
}
