//#define USE_LONG_RESULTS
// uncomment for long results and not ints


namespace SharpPhysics.StrangeDataTypes
{
	public class Result : IAny
	{
#if USE_LONG_RESULTS
		public long resultValue;

		public override bool Equals(object? obj)
		{
			return obj is Result result &&
				   resultValue == result.resultValue;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(resultValue);
		}
		public Result(long resultValue)
#else
		public int resultValue;

		public override bool Equals(object? obj)
		{
			return obj is Result result &&
				   resultValue == result.resultValue;
		}

		public override int GetHashCode()
		{
			return HashCode.Combine(resultValue);
		}

		public Result(int resultValue)
#endif
		{
			this.resultValue = resultValue;
		}
	}
}
