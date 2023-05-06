using System;

namespace LegacyApp
{
    public class UserValidator : IUserValidator
    {
        public bool IsValid(User user, out string validationErrorMessage)
        {
            validationErrorMessage = string.Empty;

            if (string.IsNullOrEmpty(user.Firstname) || string.IsNullOrEmpty(user.Surname))
            {
                validationErrorMessage = "Firstname and surname are required.";
                return false;
            }

            if (user.EmailAddress.Contains("@") && !user.EmailAddress.Contains("."))
            {
                validationErrorMessage = "Invalid email address format.";
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - user.DateOfBirth.Year;

            if (now.Month < user.DateOfBirth.Month || (now.Month == user.DateOfBirth.Month && now.Day < user.DateOfBirth.Day))
            {
                age--;
            }

            if (age < 21)
            {
                validationErrorMessage = "User must be at least 21 years old.";
                return false;
            }

            return true;
        }
    }
}
