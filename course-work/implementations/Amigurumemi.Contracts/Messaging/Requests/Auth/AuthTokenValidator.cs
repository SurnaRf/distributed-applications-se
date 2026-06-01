using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Amigurumemi.Contracts.Messaging.Requests.Auth
{
	public class AuthTokenValidator : AbstractValidator<AuthTokenRequest>
	{
		public AuthTokenValidator()
		{
			RuleFor(x => x.Username)
				.NotEmpty()
				.MinimumLength(3);

			RuleFor(x => x.Password)
				.NotEmpty()
				.MinimumLength(6);
		}
	}
}
