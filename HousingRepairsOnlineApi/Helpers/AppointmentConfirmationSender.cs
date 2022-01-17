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

        public async Task Execute(RepairRequest repairRequest, string repairId)
        {
            switch (repairRequest?.ContactDetails?.Type)
            {
                case AppointmentConfirmationSendingTypes.Email:
                    sendAppointmentConfirmationEmailUseCase.Execute(repairRequest.ContactDetails.Value, repairId, repairRequest.Time.Display);
                    break;
                case AppointmentConfirmationSendingTypes.Sms:
                    sendAppointmentConfirmationSmsUseCase.Execute(repairRequest.ContactDetails.Value, repairId,
                        repairRequest.Time.Display);
                    break;
                default:
                    throw new Exception($"Invalid contact type: {repairRequest?.ContactDetails?.Type}");
            }
        }
    }
}
