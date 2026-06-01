using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Yarns
{
	
	public class YarnValidator : AbstractValidator<YarnRequest>
	{
		public YarnValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(x => x.Brand)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(x => x.Color)
				.NotEmpty()
				.MaximumLength(50);

			RuleFor(x => x.Weight)
				.GreaterThan(0);

			RuleFor(x => x.Price)
				.GreaterThanOrEqualTo(0);

			RuleFor(x => x.ProjectId)
				.GreaterThan(0);

			RuleFor(x => x.Weight)
				.GreaterThan(0)
				.LessThanOrEqualTo(1000);
		}
	}
	
}
