namespace LegacyApp
{
    public interface IUserValidator
    {
        bool IsValid(User user, out string validationErrorMessage);
    }
}
