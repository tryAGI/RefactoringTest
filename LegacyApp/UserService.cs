using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly ClientService _clientService;
        private readonly UserCreditServiceWrapper _userCreditServiceWrapper;
        private readonly ICreditLimitCalculator _creditLimitCalculator;

        public UserService(ClientService clientService, UserCreditServiceWrapper userCreditServiceWrapper, ICreditLimitCalculator creditLimitCalculator)
        {
            _clientService = clientService;
            _userCreditServiceWrapper = userCreditServiceWrapper;
            _creditLimitCalculator = creditLimitCalculator;
        }

        public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            if (string.IsNullOrEmpty(firname) || string.IsNullOrEmpty(surname))
            {
                return false;
            }

            if (email.Contains("@") && !email.Contains("."))
            {
                return false;
            }

            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;

            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day))
            {
                age--;
            }

            if (age < 21)
            {
                return false;
            }

            var client = _clientService.GetClientById(clientId);

            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firname,
                Surname = surname
            };

            var creditLimit = _userCreditServiceWrapper.GetCreditLimit(user);

            if (client.Name == "VeryImportantClient")
            {
                creditLimit = creditLimit * 2;
            }

            user.CreditLimit = creditLimit;

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);

            return true;
        }
    }
}