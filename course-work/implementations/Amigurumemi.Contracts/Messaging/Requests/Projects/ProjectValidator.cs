using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Projects
{
	public class ProjectValidator : AbstractValidator<ProjectRequest>
	{
		public ProjectValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(x => x.PatternId)
				.NotEmpty();

			RuleFor(x => x.Progress)
				.InclusiveBetween(0, 100);

			RuleFor(x => x.StartDate)
				.NotEmpty();

			RuleFor(x => x.EstimatedCost)
				.GreaterThanOrEqualTo(0);
		}
	}
}
