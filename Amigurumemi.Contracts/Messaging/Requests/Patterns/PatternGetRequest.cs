using Amigurumemi.Contracts.Messaging.Requests.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Patterns
{
	public class PatternGetRequest : BaseGetRequest
	{
		public PatternFilterRequest? Filter { get; set; }
	}
}
