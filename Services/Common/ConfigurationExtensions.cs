using System;
using Microsoft.Extensions.Configuration;

namespace Services.Common
{
    public static class ConfigurationExtensions
    {
        public static string GetEnvironmentVariable(this IConfiguration configuration, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("Key cannot be empty");

            var value = Environment.GetEnvironmentVariable(key) ?? configuration[key];

            return string.IsNullOrWhiteSpace(value) ? throw new ArgumentNullException($"{key} environment variable is not configured") : value;
        }
    }
}
