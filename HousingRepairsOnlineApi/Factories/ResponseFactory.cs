using System.Collections.Generic;
using HousingRepairsOnlineApi.UseCases;
using Notify.Models.Responses;

namespace HousingRepairsOnlineApi.Factories
{
    public static class ResponseFactory
    {
        public static SendSmsResponse ToResponse(this SmsNotificationResponse notificationResponse, string number,
            string templateId, Dictionary<string, dynamic> personalisation)
        {
            return new SendSmsResponse
            {
                BookingReference = (string)personalisation["booking_ref"],
                TemplateId = templateId,
                PhoneNumber = number,
                GovNotifyId = notificationResponse.id,
                AppointmentTime = (string)personalisation["appointment_time"],
            };
        }
    }
}
