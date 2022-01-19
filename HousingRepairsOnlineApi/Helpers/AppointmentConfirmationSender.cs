using System;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.UseCases;

namespace HousingRepairsOnlineApi.Helpers
{
    public class AppointmentConfirmationSender : IAppointmentConfirmationSender
    {
        private readonly ISendAppointmentConfirmationEmailUseCase sendAppointmentConfirmationEmailUseCase;
        private readonly ISendAppointmentConfirmationSmsUseCase sendAppointmentConfirmationSmsUseCase;

        public AppointmentConfirmationSender(ISendAppointmentConfirmationEmailUseCase sendAppointmentConfirmationEmailUseCase, ISendAppointmentConfirmationSmsUseCase sendAppointmentConfirmationSmsUseCase)
        {
            this.sendAppointmentConfirmationEmailUseCase = sendAppointmentConfirmationEmailUseCase;
            this.sendAppointmentConfirmationSmsUseCase = sendAppointmentConfirmationSmsUseCase;
        }

        public void Execute(Repair repair)
        {
            switch (repair?.ContactDetails?.Type)
            {
                case AppointmentConfirmationSendingTypes.Email:
                    sendAppointmentConfirmationEmailUseCase.Execute(repair.ContactDetails.Value, repair.Id, repair.Time.Display);
                    break;
                case AppointmentConfirmationSendingTypes.Sms:
                    sendAppointmentConfirmationSmsUseCase.Execute(repair.ContactDetails.Value, repair.Id,
                        repair.Time.Display);
                    break;
                default:
                    throw new Exception($"Invalid contact type: {repair?.ContactDetails?.Type}");
            }
        }
    }
}
