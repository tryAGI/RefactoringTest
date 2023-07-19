using LegacyApp.Models;
using LegacyApp.Services;

namespace LegacyApp.Wrappers
{
    public class UserCreditServiceWrapper
    {
        private readonly IUserCreditService _userCreditService;

        public UserCreditServiceWrapper(IUserCreditService userCreditService)
        {
            _userCreditService = userCreditService;
        }

        public int GetCreditLimit(User user)
        {
            return _userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
        }
    }
}