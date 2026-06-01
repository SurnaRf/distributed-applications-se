using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Shared
{
	public class PagerRequest
	{
		public int Page { get; set; } = 1;

		public int PageSize { get; set; } = 5;
	}
}
