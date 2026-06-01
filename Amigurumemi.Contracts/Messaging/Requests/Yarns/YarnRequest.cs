using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Yarns
{
	public class YarnRequest
	{
		public string Name { get; set; }

		public string Brand { get; set; }

		public string Color { get; set; }

		public double Weight { get; set; }

		public decimal Price { get; set; }

		public int? ProjectId { get; set; }

		public int UserId { get; set; }
	}
}
