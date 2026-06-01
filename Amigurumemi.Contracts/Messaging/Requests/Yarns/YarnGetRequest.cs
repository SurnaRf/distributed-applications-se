using Amigurumemi.Contracts.Messaging.Requests.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Yarns
{

	public class YarnGetRequest : BaseGetRequest
	{
		public YarnFilterRequest? Filter { get; set; }
	}
}
