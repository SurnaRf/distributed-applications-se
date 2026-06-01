using Amigurumemi.Contracts.Messaging.Requests.Yarns;
using Amigurumemi.Contracts.Messaging.Responses.Shared;
using Amigurumemi.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Responses.Yarns
{
	public class YarnGetResponse : BaseGetResponse<YarnResponse>
	{
		public YarnFilterRequest Filter { get; set; }
	}
}
