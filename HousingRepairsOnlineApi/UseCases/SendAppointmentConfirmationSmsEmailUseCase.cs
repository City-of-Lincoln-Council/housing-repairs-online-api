using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Gateways;

namespace HousingRepairsOnlineApi.UseCases
{
    public class SendAppointmentConfirmationEmailUseCase : ISendAppointmentConfirmationEmailUseCase
    {
        private readonly INotifyGateway notifyGateway;
        private readonly string templateId;

        public SendAppointmentConfirmationEmailUseCase(INotifyGateway notifyGateway, string templateId)
        {
            this.notifyGateway = notifyGateway;
            this.templateId = templateId;
        }
        public async Task<SendEmailConfirmationResponse> Execute(string email, string bookingRef, string appointmentTime)
        {
            Guard.Against.NullOrWhiteSpace(email, nameof(email), "The email provided is invalid");
            Guard.Against.NullOrWhiteSpace(bookingRef, nameof(bookingRef), "The booking reference provided is invalid");
            Guard.Against.NullOrWhiteSpace(appointmentTime, nameof(appointmentTime), "The appointment time provided is invalid");

            var personalisation = new Dictionary<string, dynamic>
            {
                {"booking_ref", bookingRef},
                {"appointment_time", appointmentTime}
            };

            var response = await notifyGateway.SendEmail(email, templateId, personalisation);
            return response;
        }
    }

}
