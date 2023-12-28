using SharpPhysics;

namespace SharpPhysics
{
    public sealed class _2dMovmentRepresenter
    {
        public _2dPosition StartPosition;
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

        public _2dMovmentRepresenter(_2dPosition startPosition, _2dPosition endPosition)
        {
            StartPosition = startPosition;
            EndPosition = endPosition;
        }
    }
}
