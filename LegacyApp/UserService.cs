using System;

namespace LegacyApp
{
    public class UserService
    {
        private IClientRepository _clientRepository;
        private IUserCreditServiceWrapper _userCreditServiceWrapper;
        private IUserDataAccess _userDataAccess;

        public UserService(IClientRepository clientRepository, IUserCreditServiceWrapper userCreditServiceWrapper, IUserDataAccess userDataAccess)
        {
            _clientRepository = clientRepository;
            _userCreditServiceWrapper = userCreditServiceWrapper;
            _userDataAccess = userDataAccess;
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

            var client = _clientRepository.GetById(clientId);

            var user = CreateUser(firname, surname, email, dateOfBirth, client);

            if (client.Name == "VeryImportantClient")
            {
                // Skip credit check
                user.HasCreditLimit = false;
            }
            else if (client.Name == "ImportantClient")
            {
                // Do credit check and double credit limit
                user.HasCreditLimit = true;
                var creditLimit = _userCreditServiceWrapper.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
                creditLimit = creditLimit * 2;
                user.CreditLimit = creditLimit;
            }
            else
            {
                // Do credit check
                user.HasCreditLimit = true;
                var creditLimit = _userCreditServiceWrapper.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
                user.CreditLimit = creditLimit;
            }

            if (user.HasCreditLimit && user.CreditLimit < 500)
            {
                return false;
            }

            _userDataAccess.AddUser(user);

            return true;
        }

        private User CreateUser(string firname, string surname, string email, DateTime dateOfBirth, Client client)
        {
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                Firstname = firname,
                Surname = surname
            };

            return user;
        }
    }
}