using System;

namespace HousingRepairsOnlineApi.Domain.Boundaries;

public class CreateOrderResult
{
    public int Id { get; }
    public int StatusCode { get; }
    public string StatusCodeDescription { get; }
    public bool ExternallyManagedAppointment { get; set; }
    public Uri ExternalAppointmentManagementUrl { get; set; }

    public CreateOrderResult(int id, int statusCode, string statusCodeDescription)
    {
        Id = id;
        StatusCode = statusCode;
        StatusCodeDescription = statusCodeDescription;
        ExternallyManagedAppointment = false;
    }
}
