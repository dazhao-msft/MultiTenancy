using Microsoft.Extensions.Configuration;

namespace AadConfiguration
{
    public class AadOptionsBuilder
    {
        private readonly IConfiguration _configuration;

        public AadOptionsBuilder()
        {
            var builder = new ConfigurationBuilder().AddJsonFile(@"c:\users\dazhao\Desktop\AadOptions.json");

            _configuration = builder.Build();
        }

        public void Bind(string key, AadOptions options)
        {
            _configuration.Bind(key, options);
        }
    }
}
