using System;
using System.IO;
using System.Text.Json;

namespace QuantityMeasurementApp.RepoLayer.Utilities
{
    public class ApplicationConfig
    {
        private static ApplicationConfig _instance;
        private readonly JsonDocument _config;

        private ApplicationConfig()
        {
            string path = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

            if (!File.Exists(path))
                path = "appsettings.json";

            if (!File.Exists(path))
                throw new FileNotFoundException("appsettings.json not found.");

            string json = File.ReadAllText(path);
            _config = JsonDocument.Parse(json);
        }

        public static ApplicationConfig GetInstance()
        {
            _instance ??= new ApplicationConfig();
            return _instance;
        }

        public string GetConnectionString()
        {
            string env = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
            if (!string.IsNullOrWhiteSpace(env)) return env;

            return _config.RootElement
                .GetProperty("ConnectionStrings")
                .GetProperty("DefaultConnection")
                .GetString();
        }

        public string GetTestConnectionString()
        {
            return _config.RootElement
                .GetProperty("ConnectionStrings")
                .GetProperty("TestConnection")
                .GetString();
        }

        // CHANGED: replaces GetRepositoryType()
        // Reads "StorageMode" from appsettings.json — returns "database" or "cache"
        public string GetStorageMode()
        {
            string env = Environment.GetEnvironmentVariable("STORAGE_MODE");
            if (!string.IsNullOrWhiteSpace(env)) return env.ToLower();

            return _config.RootElement
                .GetProperty("AppSettings")
                .GetProperty("StorageMode")
                .GetString()?.ToLower() ?? "database";
        }

        public int GetMaxPoolSize()
        {
            string env = Environment.GetEnvironmentVariable("MAX_POOL_SIZE");
            if (!string.IsNullOrWhiteSpace(env) && int.TryParse(env, out int envVal))
                return envVal;

            string val = _config.RootElement
                .GetProperty("AppSettings")
                .GetProperty("MaxPoolSize")
                .GetString();
            return int.TryParse(val, out int result) ? result : 10;
        }

        public string GetEnvironment()
        {
            return _config.RootElement
                .GetProperty("AppSettings")
                .GetProperty("Environment")
                .GetString() ?? "development";
        }
    }
}