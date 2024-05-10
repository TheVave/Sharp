using Sharp._2d.ObjectRepresentation.Translation;
using Sharp._2d.Raycasting;
using Sharp.StrangeDataTypes;

namespace Sharp.Utilities.MathUtils
{
	public class Angle(double angleValue) : IAny
	{
		/// <summary>
		/// Rotation in radians
		/// </summary>
		public double AngleValue = angleValue;

		/// <summary>
		/// gets a Translation._2dPosition object that repersents the given paramenters, simlar to cartesian coords
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public Position GetPosition(int length)
		{
			_2dLine line = new _2dLineMaker(0, 0, length, AngleValue).Get_2DRay();
			return new(line.XEnd, line.YEnd);
		}
	}
}
