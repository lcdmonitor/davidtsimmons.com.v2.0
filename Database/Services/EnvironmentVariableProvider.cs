namespace Database.Services
{
    public interface IEnvironmentVariableProvider
    {
        public string? GetEnvironmentVariable(string variable);
    }

    public class EnvironmentVariableProvider : IEnvironmentVariableProvider
    {
        public string? GetEnvironmentVariable(string variable)
        {
            return Environment.GetEnvironmentVariable(variable);
        }
    }
}