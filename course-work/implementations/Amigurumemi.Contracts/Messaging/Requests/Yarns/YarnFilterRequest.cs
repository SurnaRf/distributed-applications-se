using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Yarns
{
	public class YarnFilterRequest
	{
		public string? Name { get; set; }

		public string? Brand { get; set; }

		public string? Color { get; set; }

		public int? ProjectId { get; set; }

		public int? UserId { get; set; }
	}
}
