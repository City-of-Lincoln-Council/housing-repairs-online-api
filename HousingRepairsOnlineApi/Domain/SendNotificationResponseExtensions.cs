using System.Collections.Generic;
using Notify.Models.Responses;

namespace HousingRepairsOnlineApi.Domain
{
    public static class SendNotificationResponseExtensions
    {
        public static SendSmsConfirmationResponse ToSendSmsResponse(this SmsNotificationResponse notificationResponse, string number,
            string templateId, Dictionary<string, dynamic> personalisation)
        {
            return new SendSmsConfirmationResponse
            {
                BookingReference = (string)personalisation["booking_ref"],
                TemplateId = templateId,
                PhoneNumber = number,
                GovNotifyId = notificationResponse.id,
                AppointmentTime = (string)personalisation["appointment_time"],
            };
        }
        public static SendEmailConfirmationResponse ToSendEmailResponse(this EmailNotificationResponse notificationResponse, string email,
            string templateId, Dictionary<string, dynamic> personalisation)
        {
            return new SendEmailConfirmationResponse
            {
                BookingReference = (string)personalisation["booking_ref"],
                TemplateId = templateId,
                Email = email,
                GovNotifyId = notificationResponse.id,
                AppointmentTime = (string)personalisation["appointment_time"],
            };
        }
    }
}
