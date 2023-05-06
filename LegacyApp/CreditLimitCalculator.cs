

namespace LegacyApp
{
    public class CreditLimitCalculator : ICreditLimitCalculator
    {
        public void CalculateCreditLimit(User user)
        {
            if (user.Client.Name == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (user.Client.Name == "ImportantClient")
            {
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditServiceClient())
                {
                    var creditLimit = userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditServiceClient())
                {
                    var creditLimit = userCreditService.GetCreditLimit(user.Firstname, user.Surname, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }
        }
    }
}
