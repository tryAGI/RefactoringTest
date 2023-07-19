namespace LegacyApp
{
    public interface IUserCreditServiceWrapper
    {
        int GetCreditLimit(string firstname, string surname, DateTime dateOfBirth);
    }
}