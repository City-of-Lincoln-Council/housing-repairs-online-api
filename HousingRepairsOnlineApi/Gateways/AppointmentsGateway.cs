using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using HACT.Dtos;
using HousingRepairsOnline.Authentication.Helpers;

namespace HousingRepairsOnlineApi.Gateways
{
    public class AppointmentsGateway : IAppointmentsGateway
    {
        private readonly HttpClient httpClient;
        private readonly string authenticationIdentifier;

        public AppointmentsGateway(HttpClient httpClient, string authenticationIdentifier)
        {
            this.httpClient = httpClient;
            this.authenticationIdentifier = authenticationIdentifier;
        }

        public async Task<IEnumerable<Appointment>> GetAvailableAppointments(string sorCode, string locationId)
        {
            var request = new HttpRequestMessage(HttpMethod.Get,
                $"/Appointments?sorCode={sorCode}&locationId={locationId}");

            request.SetupJwtAuthentication(httpClient, authenticationIdentifier);

            var response = await httpClient.SendAsync(request);

            var result = Enumerable.Empty<Appointment>();
            if (response.StatusCode == HttpStatusCode.OK)
            {
                result = await response.Content.ReadFromJsonAsync<List<Appointment>>();
            }

            return result;
        }
    }
}
