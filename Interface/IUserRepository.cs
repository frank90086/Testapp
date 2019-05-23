namespace Test.Interface
{
    public interface IUserRepository
    {
        bool CheckClientCredential(string id, string password);
    }
}