using AadConfiguration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;

namespace Microsoft.AspNetCore.Authentication
{
    public static class AzureAdServiceCollectionExtensions
    {
        public static AuthenticationBuilder AddAzureAdBearer(this AuthenticationBuilder builder)
            => builder.AddAzureAdBearer(_ => { });

        public static AuthenticationBuilder AddAzureAdBearer(this AuthenticationBuilder builder, Action<AadOptions> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureAzureOptions>();
            builder.AddJwtBearer();
            return builder;
        }

        private class ConfigureAzureOptions : IConfigureNamedOptions<JwtBearerOptions>
        {
            private readonly AadOptions _azureOptions;

            public ConfigureAzureOptions(IOptions<AadOptions> azureOptions)
            {
                _azureOptions = azureOptions.Value;
            }

            public void Configure(string name, JwtBearerOptions options)
            {
                options.Audience = _azureOptions.Audience;
                options.Authority = $"{_azureOptions.Instance}{_azureOptions.TenantId}";

                //
                // TODO:
                // In multi-tenant scenarions, tokens can be issued by different tenants than the one of gateway.
                // Apply the custom logic to validate the security token:
                // https://github.com/Azure-Samples/active-directory-dotnet-webapp-multitenant-openidconnect/blob/master/TodoListWebApp/App_Start/Startup.Auth.cs#L55
                //

                options.TokenValidationParameters.ValidateIssuer = false;
            }

            public void Configure(JwtBearerOptions options)
            {
                Configure(Options.DefaultName, options);
            }
        }
    }
}
