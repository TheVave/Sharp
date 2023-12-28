namespace SharpPhysics
{
	public class Angle
	{
		/// <summary>
		/// Rotation in degrees
		/// </summary>
		public short AngleValue;

		/// <summary>
		/// gets a Translation._2dPosition object that repersents the given paramenters, simlar to cartesian coords
		/// </summary>
		/// <param name="length"></param>
		/// <returns></returns>
		public _2dPosition GetPosition(int length)
		{
			_2dLineMaker lnmkr = new();
			lnmkr.xStart = 0;
			lnmkr.yStart = 0;
			lnmkr.Length = length;
			lnmkr.Rot = AngleValue;
			_2dLine line = lnmkr.Get_2DRay();
			return new(line.XEnd,line.YEnd);
		} 
	}
}
