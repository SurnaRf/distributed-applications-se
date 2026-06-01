using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Responses.Shared
{
	public class BaseGetResponse<E>
	{
		public List<E> Items { get; set; }
		public PagerResponse Pager { get; set; }
		public string OrderBy { get; set; }
		public bool SortAsc { get; set; }
	}
}
