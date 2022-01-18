﻿using HousingRepairsOnlineApi.Gateways;
using HousingRepairsOnlineApi.UseCases;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.UseCasesTests
{
    public class RetrieveImageLinkUseCaseTests
    {
        private readonly RetrieveImageLinkUseCase sytemUndertest;
        private readonly Mock<IBlobStorageGateway> mockAzureStorageGateway;

        public RetrieveImageLinkUseCaseTests()
        {
            mockAzureStorageGateway = new Mock<IBlobStorageGateway>();
            sytemUndertest = new RetrieveImageLinkUseCase(mockAzureStorageGateway.Object, 100);
        }

        [Fact]
        public async void GivenAPhotoUrl_WhenExecute_ThenGatewayIsCalled()
        {
            const string FileName = "//https://housingrepairsonline.blob.core.windows.net/housing-repairs-online/0f6780dd-ce73-44f9-b64c-b56c061560ea.png";
            mockAzureStorageGateway.Setup(x =>
                x.GetServiceSasUriForBlob(FileName, 100, null)).ReturnsAsync("uri");

            var _ = await sytemUndertest.Execute(FileName);

            mockAzureStorageGateway.Verify(x => x.GetServiceSasUriForBlob ("0f6780dd-ce73-44f9-b64c-b56c061560ea.png", 100, null), Times.Once);
        }
    }
}
