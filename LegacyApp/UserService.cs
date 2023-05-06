using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            IUserValidator userValidator = new UserValidator();
            if (!userValidator.IsValidUser(firname, surname, email, dateOfBirth))
            {
                return false;
            }

            IAgeCalculator ageCalculator = new AgeCalculator();
            int age = ageCalculator.CalculateAge(dateOfBirth);

            if (age < 21)
            {
                return false;
            }

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firname,
                Surname = surname
            };

            ICreditLimitCalculator creditLimitCalculator = new CreditLimitCalculator();
            user.HasCreditLimit = creditLimitCalculator.HasCreditLimit(client);
            user.CreditLimit = creditLimitCalculator.CalculateCreditLimit(user, client);

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);

            return true;
        }
    }
}