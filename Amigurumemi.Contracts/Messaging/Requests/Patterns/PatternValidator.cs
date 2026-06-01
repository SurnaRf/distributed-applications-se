using Amigurumemi.Contracts.Messaging.Requests.Pattern;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Patterns
{
	public class PatternValidator : AbstractValidator<PatternRequest>
	{
		public PatternValidator()
		{
			RuleFor(x => x.Name)
				.NotEmpty()
				.MaximumLength(100);

			RuleFor(x => x.Description)
				.NotEmpty()
				.MaximumLength(1000);

			RuleFor(x => x.Category)
				.NotEmpty()
				.MaximumLength(50);

			RuleFor(x => x.Difficulty)
				.IsInEnum();
		}
	}
}
