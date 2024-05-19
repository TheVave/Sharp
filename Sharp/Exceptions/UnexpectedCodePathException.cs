using Sharp.StrangeDataTypes;

namespace Sharp.Exceptions
{
	public class UnexpectedCodePathException : Exception, IAny
	{
		public UnexpectedCodePathException(string message) : base(message)
		{
		}

		public UnexpectedCodePathException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public UnexpectedCodePathException()
		{
		}
	}
}
