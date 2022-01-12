using System;
using System.Net.Http;
using System.Threading.Tasks;
using Azure.Cosmos;
using Azure.Storage.Blobs;
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
using Notify.Client;

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

            var notifyApiKey = GetEnvironmentVariable("GOV_NOTIFY_KEY");

            services.AddTransient<INotifyGateway, NotifyGateway>(s =>
                {
                    var notifyClient = new NotificationClient(notifyApiKey);
                    return new NotifyGateway(notifyClient);
                }
            );
            var smsConfirmationTemplateId = GetEnvironmentVariable("CONFIRMATION_SMS_NOTIFY_TEMPLATE_ID");

            var emailConfirmationTemplateId = GetEnvironmentVariable("CONFIRMATION_EMAIL_NOTIFY_TEMPLATE_ID");

            services.AddTransient<ISendAppointmentConfirmationSmsUseCase, SendAppointmentConfirmationSmsUseCase>(s =>
            {
                var notifyGateway = s.GetService<INotifyGateway>();
                return new SendAppointmentConfirmationSmsUseCase(notifyGateway, smsConfirmationTemplateId);
            });

            services.AddTransient<ISendAppointmentConfirmationEmailUseCase, SendAppointmentConfirmationEmailUseCase>(s =>
            {
                var notifyGateway = s.GetService<INotifyGateway>();
                return new SendAppointmentConfirmationEmailUseCase(notifyGateway, emailConfirmationTemplateId);
            });

            services.AddHousingRepairsOnlineAuthentication(HousingRepairsOnlineApiIssuerId);
            services.AddTransient<ISaveRepairRequestUseCase, SaveRepairRequestUseCase>();

            var cosmosContainer = GetCosmosContainer();

            services.AddTransient<IIdGenerator, IdGenerator>();

            services.AddTransient<IRepairStorageGateway, CosmosGateway>(s =>
            {
                var idGenerator = s.GetService<IIdGenerator>();
                return new CosmosGateway(
                    cosmosContainer, idGenerator
                );
            });

            var blobContainerClient = GetBlobContainerClient();

            services.AddTransient<IBlobStorageGateway, AzureStorageGateway>(s =>
            {
                return new AzureStorageGateway(
                    blobContainerClient
                );
            });

            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HousingRepairsOnlineApi", Version = "v1" });
                c.AddJwtSecurityScheme();
            });

        }

        private static BlobContainerClient GetBlobContainerClient()
        {
            string storageConnectionString = Environment.GetEnvironmentVariable("AZURE_STORAGE_CONNECTION_STRING");
            string blobContainerName = Environment.GetEnvironmentVariable("STORAGE_CONTAINER_NAME");

            BlobServiceClient blobServiceClient = new BlobServiceClient(storageConnectionString);
            BlobContainerClient blobContainerClient = blobServiceClient.GetBlobContainerClient(blobContainerName);
            return blobContainerClient;
        }

        private static ContainerResponse GetCosmosContainer()
        {
            var endpointUrl = GetEnvironmentVariable("COSMOS_ENDPOINT_URL");
            var authorizationKey = GetEnvironmentVariable("COSMOS_AUTHORIZATION_KEY");
            var databaseId = GetEnvironmentVariable("COSMOS_DATABASE_ID");
            var containerId = GetEnvironmentVariable("COSMOS_CONTAINER_ID");

            CosmosClient cosmosClient = new CosmosClient(endpointUrl, authorizationKey);

            Task<DatabaseResponse> databaseResponseTask = cosmosClient.CreateDatabaseIfNotExistsAsync(databaseId);
            _ = databaseResponseTask.GetAwaiter().GetResult();
            ;

            Task<ContainerResponse> cosmosContainerResponse =
                cosmosClient.GetDatabase(databaseId).CreateContainerIfNotExistsAsync(containerId, "/RepairID");
            ContainerResponse cosmosContainer = cosmosContainerResponse.GetAwaiter().GetResult();
            ;
            return cosmosContainer;
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
