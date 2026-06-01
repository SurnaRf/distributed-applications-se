using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Shared
{
	public class BaseGetRequest
	{
		public PagerRequest Pager { get; set; } = new();

		public string? OrderBy { get; set; }

		public bool SortAsc { get; set; } = true;
	}
}
