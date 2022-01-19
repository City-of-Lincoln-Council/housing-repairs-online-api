using System.Threading.Tasks;
using HousingRepairsOnlineApi.Domain;
using HousingRepairsOnlineApi.Helpers;
using HousingRepairsOnlineApi.UseCases;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.HelpersTests
{
    public class InternalEmailSenderTests
    {
        private InternalEmailSender systemUnderTest;
        private Mock<IInternalEmailSender> internalEmailSender;
        private Mock<IRetrieveImageLinkUseCase> retrieveImageLinkUseCase;
        private Mock<ISendInternalEmailUseCase> sendInternalEmailUseCase;

        public InternalEmailSenderTests()
        {
            retrieveImageLinkUseCase = new Mock<IRetrieveImageLinkUseCase>();
            internalEmailSender = new Mock<IInternalEmailSender>();
            sendInternalEmailUseCase = new Mock<ISendInternalEmailUseCase>();
            systemUnderTest = new InternalEmailSender(retrieveImageLinkUseCase.Object, sendInternalEmailUseCase.Object);

        }

        [Fact]
        public async Task GivenARetrieveImageLink_WhenExecute_ThenSendInternalEmailUseCaseIsCalled()
        {

            var repair = new Repair
            {
                Id = "1AB2C3D4",
                ContactDetails = new RepairContactDetails { Value = "07465087654" },
                Address = new RepairAddress { Display = "address", LocationId = "uprn" },
                Description = new RepairDescription { Text = "repair description", Base64Image = "image", PhotoUrl = "x/Url.png" },
                Location = new RepairLocation { Value = "location" },
                Problem = new RepairProblem { Value = "problem" },
                Issue = new RepairIssue { Value = "issue" },
                SOR = "sor"
            };

            retrieveImageLinkUseCase.Setup(x => x.Execute(repair.Description.PhotoUrl)).Returns("Url.png");

            await systemUnderTest.Execute(repair);

            sendInternalEmailUseCase.Verify(x => x.Execute(
                repair.Id,
                repair.Address.LocationId,
                repair.Address.Display,
                repair.SOR,
                repair.Description.Text,
                repair.ContactDetails.Value,
                "Url.png"),
                Times.Once);
        }

        [Fact]
        public async Task GivenNoRetrieveImageLink_WhenExecute_ThenSendInternalEmailUseCaseIsCalled()
        {

            var repair = new Repair
            {
                Id = "1AB2C3D4",
                ContactDetails = new RepairContactDetails { Value = "07465087654" },
                Address = new RepairAddress { Display = "address", LocationId = "uprn" },
                Description = new RepairDescription { Text = "repair description", Base64Image = "image", PhotoUrl = "x/Url.png" },
                Location = new RepairLocation { Value = "location" },
                Problem = new RepairProblem { Value = "problem" },
                Issue = new RepairIssue { Value = "issue" },
                SOR = "sor"
            };

            retrieveImageLinkUseCase.Setup(x => x.Execute(repair.Description.PhotoUrl)).Returns("");

            systemUnderTest.Execute(repair);

            sendInternalEmailUseCase.Verify(x => x.Execute(
                    repair.Id,
                    repair.Address.LocationId,
                    repair.Address.Display,
                    repair.SOR,
                    repair.Description.Text,
                    repair.ContactDetails.Value,
                    "Url.png"),
                Times.Never);
        }
    }
}
