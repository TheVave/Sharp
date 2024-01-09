using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpPhysics.Exceptions
{
	public class MessageBoxException : Exception
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
