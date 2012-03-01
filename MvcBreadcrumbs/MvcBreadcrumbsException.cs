using System;
using System.Runtime.Serialization;

namespace MvcBreadcrumbs
{
	public class MvcBreadcrumbsException : ApplicationException
	{
		public MvcBreadcrumbsException() 
		{
		}

		public MvcBreadcrumbsException(string message) : base(message)
		{
		}

		public MvcBreadcrumbsException(string message, Exception innerException) : base(message, innerException)
		{
		}

		protected MvcBreadcrumbsException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
