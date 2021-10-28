using System;
using System.Collections.Generic;
using System.IO;
using Ardalis.GuardClauses;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace HousingRepairsOnlineApi.Helpers
{
    public static class ServiceCollectionExtensions
    {
        public static void AddSoREngine(this IServiceCollection services, string sorConfigPath)
        {
            Guard.Against.NullOrWhiteSpace(sorConfigPath, nameof(sorConfigPath));

            var SorConfigPath = sorConfigPath;
            string json;
            try
            {
                json = File.ReadAllText(SorConfigPath);
            }
            catch (FileNotFoundException e)
            {
                throw new InvalidOperationException($"Required configuration file '{SorConfigPath}' not found.", e);
            }

            IDictionary<string, IDictionary<string, IDictionary<string, string>>> soRMapping;
            try
            {
                soRMapping = JsonConvert.DeserializeObject<IDictionary<string, IDictionary<string, IDictionary<string, string>>>>(json);
            }
            catch (JsonReaderException e)
            {
                throw new InvalidOperationException($"Contents of configuration file {SorConfigPath} is malformed.", e);
            }

            services.AddTransient<ISoREngine, SoREngine>(_ => new SoREngine(soRMapping));
        }
    }
}
