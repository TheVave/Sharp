using SharpPhysics.StrangeDataTypes;

namespace SharpPhysics.Exceptions
{
	public class MessageBoxException : Exception, IAny
	{
		public MessageBoxException(string message) : base(message)
		{
		}

		public MessageBoxException(string message, Exception innerException) : base(message, innerException)
		{
		}

		public MessageBoxException()
		{
		}
	}
}
