namespace Infrastructure.Configuration.Factories
{
    public interface IConfigurationQueryFactory
    {
        IConfigurationQuery CreateConfigurationQuery(string filePath);
    }
}
