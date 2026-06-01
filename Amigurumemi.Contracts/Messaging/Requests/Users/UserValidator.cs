using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Users
{
	public class UserValidator : AbstractValidator<UserRequest>
	{
		public UserValidator()
		{
			RuleFor(x => x.Username)
				.NotEmpty()
				.MinimumLength(3)
				.MaximumLength(50);

			RuleFor(x => x.Email)
				.NotEmpty()
				.EmailAddress();

			RuleFor(x => x.Password)
				.NotEmpty()
				.MinimumLength(6);

			RuleFor(x => x.FirstName)
				.NotEmpty()
				.MaximumLength(50);

			RuleFor(x => x.LastName)
				.NotEmpty()
				.MaximumLength(50);
		}
	}
}
