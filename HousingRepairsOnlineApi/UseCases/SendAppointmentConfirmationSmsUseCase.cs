using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Gateways;

namespace HousingRepairsOnlineApi.UseCases
{
    public class SendAppointmentConfirmationSmsUseCase : ISendAppointmentConfirmationSmsUseCase
    {
        private readonly INotifyGateway notifyGateway;
        private readonly string templateId;

        public SendAppointmentConfirmationSmsUseCase(INotifyGateway notifyGateway, string templateId)
        {
            this.notifyGateway = notifyGateway;
            this.templateId = templateId;
        }

        public async Task<SendSmsConfirmationResponse> Execute(string number, string bookingRef, string appointmentTime)
        {
            Guard.Against.NullOrWhiteSpace(number, nameof(number), "The phone number provided is invalid");
            Guard.Against.NullOrWhiteSpace(bookingRef, nameof(bookingRef), "The booking reference provided is invalid");
            Guard.Against.NullOrWhiteSpace(appointmentTime, nameof(appointmentTime), "The appointment time provided is invalid");
            ValidatePhoneNumber(number);

            var personalisation = new Dictionary<string, dynamic>
            {
                {"booking_ref", bookingRef},
                {"appointment_time", appointmentTime}
            };

            var response = await notifyGateway.SendSms(number, templateId, personalisation);
            return response;
        }

        private static bool ValidatePhoneNumber(string number)
        {
            var result = new Regex(@"^(((\+44\s?\d{4}|\(?0\d{4}\)?)\s?\d{3}\s?\d{3})|((\+44\s?\d{3}|\(?0\d{3}\)?)\s?\d{3}\s?\d{4})|((\+44\s?\d{2}|\(?0\d{2}\)?)\s?\d{4}\s?\d{4}))(\s?\#(\d{4}|\d{3}))?$");
            if (!result.IsMatch(number))
            {
                throw new ArgumentException("The phone number provided is invalid", nameof(number));
            }
            return true;
        }
    }
}
