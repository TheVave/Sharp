namespace SharpPhysics._2d.Physics
{
	/// <summary>
	/// This class is the main calculator of the ([nothing]/_2d)PhysicsSimulator class
	/// </summary>
	public sealed class SUVATEquations
	{

		/// <summary>
		/// VS is velocity start
		/// normally represented as U
		/// </summary>
		public double VS { get; set; }

		/// <summary>
		/// VE is velocity end normally represented by v
		/// </summary>
		public double VE { get; set; }

		/// <summary>
		/// Time
		/// </summary>
		public double T { get; set; }

		/// <summary>
		/// Acceleration
		/// </summary>
		public double A { get; set; }

		/// <summary>
		/// Displacement
		/// </summary>
		public double S { get; set; }

		/// <summary>
		/// input variation
		/// </summary>
		/// <param name="ToCheck"></param>
		/// <returns></returns>
		/// <exception cref="NullReferenceException"></exception>
		bool CheckIfNull(object ToCheck, string messageIfNull)
		{
			if (ToCheck is null)
			{
				throw new NullReferenceException(messageIfNull);
			}
			else
			{
				return true;
			}
		}


		/// <summary>
		/// Solving for time, knowing VS A S VE
		/// </summary>
		/// <returns></returns>
		public double NTWVSASVE()
		{
			return (VS + A) * VE;
		}



		/// <summary>
		/// Solving for S with V S T A
		/// </summary>
		/// <returns></returns>
		public double NSWVSTA()
		{
			return VS * T + 1 / 2 * A * T;
		}

		/// <summary>
		/// Solving for S with VS VE T
		/// </summary>
		/// <returns></returns>
		public double NSWVSVET()
		{
			return 1 / 2 * (VS + VE) * T;
		}

		/// <summary>
		/// Solving for VE with VS A S
		/// </summary>
		/// <returns></returns>
		public double NVEWVSAS()
		{
			return Math.Pow(VS, 2) + 2 * A * S;
		}

		/// <summary>
		/// Solving for S with VE T A
		/// </summary>
		/// <returns></returns>
		public double NSWVETA()
		{
			return VE * T - 1 / 2 * Math.Pow(A * T, 2);
		}
	}
}