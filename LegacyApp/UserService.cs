using System;

namespace LegacyApp
{
    public class UserService
    {
        private readonly IClientRepository _clientRepository;
        private readonly IUserValidator _userValidator;
        private readonly ICreditLimitCalculator _creditLimitCalculator;

        public UserService(IClientRepository clientRepository, IUserValidator userValidator, ICreditLimitCalculator creditLimitCalculator)
        {
            _clientRepository = clientRepository;
            _userValidator = userValidator;
            _creditLimitCalculator = creditLimitCalculator;
        }
        public bool AddUser(string firname, string surname, string email, DateTime dateOfBirth, int clientId)
        {
            var user = new User
            {
                Client = _clientRepository.GetById(clientId),
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firname,
                Surname = surname
            };

            if (!_userValidator.IsValid(user, out var validationErrorMessage))
            {
                return false;
            }

            _creditLimitCalculator.CalculateCreditLimit(user);

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            UserDataAccess.AddUser(user);

            return true;
        }
    }
}