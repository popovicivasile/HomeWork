namespace HomeWork
{
    public static class ConfigurationBuilderExtensions
    {
        public static IConfigurationBuilder Configure<T>(this IConfigurationBuilder configurationBuilder,
            string basePath, bool addUserSecrets = true) where T : class
        {
            configurationBuilder
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.Development.json", optional: false, reloadOnChange: true)
                .AddJsonFile("secrets/appsettings.secrets.json", optional: true)
                .AddEnvironmentVariables();

            if (addUserSecrets)
            {
                configurationBuilder.AddUserSecrets<T>();
            }

            return configurationBuilder;
        }
    }
}
