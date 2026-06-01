using Amigurumemi.Repositories.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.ApplicationServices.Implementations
{
	public abstract class BaseService
	{
		protected readonly IUnitOfWork _unitOfWork;
		protected readonly ILogger _logger;

		protected BaseService(
			ILogger logger,
			IUnitOfWork unitOfWork)
		{
			_logger = logger;
			_unitOfWork = unitOfWork;
		}
	}
}
