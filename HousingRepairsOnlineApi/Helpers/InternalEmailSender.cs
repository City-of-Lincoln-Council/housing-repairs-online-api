using System;
using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.UseCases;

namespace HousingRepairsOnlineApi.Helpers
{
    public class InternalEmailSender : IInternalEmailSender
    {
        private IRetrieveImageLinkUseCase retrieveImageLinkUseCase;
        private ISendInternalEmailUseCase sendInternalEmailUseCase;

        public InternalEmailSender(IRetrieveImageLinkUseCase retrieveImageLinkUseCase, ISendInternalEmailUseCase sendInternalEmailUseCase)
        {
            this.retrieveImageLinkUseCase = retrieveImageLinkUseCase;
            this.sendInternalEmailUseCase = sendInternalEmailUseCase;
        }

        public async Task Execute(Repair repair)
        {
            var imageLink = "";

            await Task.Run(() =>
            {
                imageLink = retrieveImageLinkUseCase.Execute(repair.Description.PhotoUrl);

            });
            sendInternalEmailUseCase.Execute(repair.Id, repair.Address.LocationId, repair.Address.Display, repair.SOR, repair.Description.Text, repair.ContactDetails?.Value, imageLink);
        }
    }
}
