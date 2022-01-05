﻿using System;
using System.Net.Http;
using Azure.Cosmos;
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

        private static readonly string EndpointUrl = GetEnvironmentVariable("COSMOS_ENDPOINT_URL");
        private static readonly string AuthorizationKey = GetEnvironmentVariable("COSMOS_AUTHORIZATION_KEY");
        private static readonly string DatabaseId = GetEnvironmentVariable("COSMOS_DATABASE_ID");
        private static readonly string ContainerId = GetEnvironmentVariable("COSMOS_CONTAINER_ID");

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSoREngine("SoRConfig.json");

            services.AddTransient<IRetrieveAddressesUseCase, RetrieveAddressesUseCase>();
            services.AddTransient<IRetrieveAvailableAppointmentsUseCase, RetrieveAvailableAppointmentsUseCase>();

            var addressesApiUrl = GetEnvironmentVariable("ADDRESSES_API_URL");
            var schedulingApiUrl = GetEnvironmentVariable("SCHEDULING_API_URL");
            var authenticationIdentifier = GetEnvironmentVariable("AUTHENTICATION_IDENTIFIER");
            services.AddHttpClient();

            services.AddTransient<IAddressGateway, AddressGateway>(s =>
            {
                var httpClient = s.GetService<HttpClient>();
                httpClient.BaseAddress = new Uri(addressesApiUrl);
                return new AddressGateway(httpClient, authenticationIdentifier);
            });

            services.AddTransient<IAppointmentsGateway, AppointmentsGateway>(s =>
            {
                var httpClient = s.GetService<HttpClient>();
                httpClient.BaseAddress = new Uri(schedulingApiUrl);
                return new AppointmentsGateway(httpClient, authenticationIdentifier);
            });

            services.AddHousingRepairsOnlineAuthentication(HousingRepairsOnlineApiIssuerId);

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HousingRepairsOnlineApi", Version = "v1" });
                c.AddJwtSecurityScheme();
            });

            services.AddTransient<ICosmosGateway, CosmosGateway>(s => new CosmosGateway(
                EndpointUrl, AuthorizationKey, DatabaseId, ContainerId
                ));
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
