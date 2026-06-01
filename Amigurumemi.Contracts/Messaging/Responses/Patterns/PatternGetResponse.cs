using Amigurumemi.Contracts.Messaging.Requests.Patterns;
using Amigurumemi.Contracts.Messaging.Responses.Shared;
using Amigurumemi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Responses.Patterns
{
	public class PatternGetResponse : BaseGetResponse<PatternResponse>
	{
		public PatternFilterRequest Filter { get; set; }
	}
}
