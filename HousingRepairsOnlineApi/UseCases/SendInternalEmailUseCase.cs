using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Ardalis.GuardClauses;
using HousingRepairsOnlineApi.Gateways;

namespace HousingRepairsOnlineApi.UseCases
{
    public class SendInternalEmailUseCase : ISendInternalEmailUseCase
    {
        private readonly INotifyGateway notifyGateway;
        private readonly string templateId;
        private readonly string internalEmail;

        public SendInternalEmailUseCase(INotifyGateway notifyGateway, string templateId, string internalEmail)
        {
            this.notifyGateway = notifyGateway;
            this.templateId = templateId;
            this.internalEmail = internalEmail;
        }
        public void Execute(string repairRef, string uprn, string address, string sor, string repairDescription, string contactNumber, string image)
        {
            Guard.Against.NullOrWhiteSpace(repairRef, nameof(repairRef), "The repair reference provided is invalid");
            Guard.Against.NullOrWhiteSpace(uprn, nameof(uprn), "The uprn provided is invalid");
            Guard.Against.NullOrWhiteSpace(address, nameof(address), "The address provided is invalid");
            Guard.Against.NullOrWhiteSpace(sor, nameof(sor), "The sor provided is invalid");
            Guard.Against.NullOrWhiteSpace(repairDescription, nameof(repairDescription), "The repairDescription provided is invalid");
            Guard.Against.NullOrWhiteSpace(contactNumber, nameof(contactNumber), "The contact number provided is invalid");
            Guard.Against.NullOrWhiteSpace(image, nameof(image), "The image provided is invalid");

            ValidateEmail(internalEmail);

            var personalisation = new Dictionary<string, dynamic>
            {
                {"repair_ref", repairRef},
                {"uprn", uprn},
                {"address", address},
                {"sor", sor},
                {"repair_desc", repairDescription},
                {"contact_no", contactNumber},
                {"image_1", image},
            };

            notifyGateway.SendEmail(internalEmail, templateId, personalisation);
        }

        private static bool ValidateEmail(string email)
        {
            var result = new Regex(@"^\w+([\.-]?\w+)*([\+\.-]?\w+)?@\w+([\.-]?\w+)*(\.\w{2,3})+");
            if (!result.IsMatch(email))
            {
                throw new ArgumentException("The email provided is invalid", nameof(email));
            }
            return true;
        }
    }
}
