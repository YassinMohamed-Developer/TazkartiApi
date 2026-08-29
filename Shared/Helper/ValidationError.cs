using System;
using System.Collections.Generic;
using System.Text;

namespace Shared.Helper
{
	public static class ValidationError
	{
		public static class AuthError
		{
			public const string HaveSameNationalId =
				"This National Id already exists.";

			public const string InvalidNationalId =
				"This is Invalid National Id ";

			public const string InvalidEmail =
						"This is Invalid Email ";

			public const string InvalidCredentials =
				"Invalid credentials for ";

			public const string EmailAlreadyExists =
				"This Email already exists";

			public const string UserNameAlreadyExists =
				"Username already exists.";

			public const string RegistrationSucceeded =
				"Registration completed successfully.";

			public const string RegistrationFailed =
				"Registration failed.";

			public const string PasswordResetFailed =
				"The operation could not be completed.";

			public const string PasswordChanged =
				"Your password has been changed.";

			public const string CheckYourEmail =
				"Please check your email.";

			public const string FailedToCreateUser =
				"Failed to create the user.";

			public const string InvalidToken =
				"Invalid token.";

			public const string GoogleAuthenticationFailed =
				"Google authentication failed.";

			public const string LoginSucceeded =
				"Login successful.";
			public static readonly string ClubAlreadyExists = "This club already exists.";
		}
	}
}
