using System;
using System.Net.Http;
using HousingRepairsOnline.Authentication.DependencyInjection;
using HousingRepairsOnlineApi.Gateways;
using HousingRepairsOnlineApi.Helpers;
using HousingRepairsOnlineApi.UseCases;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace HousingRepairsOnlineApi
{
    public class Startup
    {
        private const string HousingRepairsOnlineApiIssuerId = "Housing Repairs Online Api";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSoREngine("SoRConfig.json");

            services.AddTransient<IRetrieveAddressesUseCase, RetrieveAddressesUseCase>();
            var addressesApiUrl = GetEnvironmentVariable("ADDRESSES_API_URL");
            var authenticationIdentifier = GetEnvironmentVariable("AUTHENTICATION_IDENTIFIER");
            services.AddHttpClient();
            services.AddTransient<IAddressGateway, AddressGateway>(s => new AddressGateway(
                s.GetService<HttpClient>(), addressesApiUrl, authenticationIdentifier));

            services.AddHousingRepairsOnlineAuthentication(HousingRepairsOnlineApiIssuerId);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HousingRepairsOnlineApi", Version = "v1" });
                c.AddJwtSecurityScheme();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HousingRepairsOnlineApi v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers().RequireAuthorization();
            });
        }

        private static string GetEnvironmentVariable(string name)
        {
            return Environment.GetEnvironmentVariable(name) ??
                   throw new InvalidOperationException($"Incorrect configuration: '{name}' environment variable must be set");
        }
    }
}
