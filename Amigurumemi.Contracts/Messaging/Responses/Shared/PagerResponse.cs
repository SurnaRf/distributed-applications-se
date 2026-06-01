using Amigurumemi.Contracts.Messaging.Requests.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Responses.Shared
{
	public class PagerResponse : PagerRequest
	{
		public int Count { get; set; }
	}
}
