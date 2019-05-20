namespace Test.Interface
{
    public interface IRedisContext
    {
        void SetValue(string key, string value);
        string GetValue(string key);
    }
}