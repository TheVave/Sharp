namespace SharpPhysics
{
	public sealed class SUVATEquations
	{

		/// <summary>
		/// VS is velicity start
		/// normaly repersented as U
		/// </summary>
		public double VS { get; set; }

		/// <summary>
		/// VE is velocity end normaly repersented by v
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
		/// Displacment
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
			CheckIfNull(VS, "VS is null");
			CheckIfNull(A, "A is null");
			CheckIfNull(VE, "VE is null");
			return (VS + A) * VE;
		}



		/// <summary>
		/// Solving for S with V S T A
		/// </summary>
		/// <returns></returns>
		public double NSWVSTA()
		{
			//CheckIfNull(VS, "VS is null");
			//CheckIfNull(A, "A is null");
			//CheckIfNull(T, "VE is null");
			return VS * T + 1 / 2 * A * T;
		}

		/// <summary>
		/// Solving for S with VS VE T
		/// </summary>
		/// <returns></returns>
		public double NSWVSVET()
		{
			CheckIfNull(VS, "VS is null");
			CheckIfNull(T, "A is null");
			CheckIfNull(VE, "VE is null");
			return 1 / 2 * (VS + VE) * T;
		}

		/// <summary>
		/// Solving for VE with VS A S
		/// </summary>
		/// <returns></returns>
		public double NVEWVSAS()
		{
			CheckIfNull(VS, "VS is null");
			CheckIfNull(A, "A is null");
			CheckIfNull(S, "VE is null");
			return Math.Pow(VS, 2) + 2 * A * S;
		}

		/// <summary>
		/// Solving for S with VE T A
		/// </summary>
		/// <returns></returns>
		public double NSWVETA()
		{
			CheckIfNull(VS, "VE is null");
			CheckIfNull(T, "T is null");
			CheckIfNull(VE, "A is null");
			return VE * T - 1 / 2 * Math.Pow(A * T, 2);
		}
	}
}