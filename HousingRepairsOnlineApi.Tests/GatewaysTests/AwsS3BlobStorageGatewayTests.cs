using System;
using System.Collections.Generic;
using Amazon.S3;
using Amazon.S3.Transfer;
using FluentAssertions;
using HousingRepairsOnlineApi.Gateways;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.GatewaysTests
{
    public class AwsS3BlobStorageGatewayTests
    {
        private readonly AwsS3BlobStorageGateway systemUnderTest;
        private readonly Mock<IAmazonS3> s3ClientMock = new();
        private readonly Mock<ITransferUtility> transferUtilityMock = new();
        private string bucketName = "bucketName";

        public AwsS3BlobStorageGatewayTests()
        {
            systemUnderTest = new AwsS3BlobStorageGateway(s3ClientMock.Object, transferUtilityMock.Object, bucketName, 7);
        }

        [Fact]
        public async void AFileIsUploaded()
        {
            // Arrange
            const string base64Img = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAIAAACQd1PeAAABhWlDQ1BJQ0MgcHJvZmlsZQAAKJF9kT1Iw1AUhU9TpaJVB4uIOGSoThZERRy1CkWoEGqFVh1MXvojNGlIUlwcBdeCgz+LVQcXZ10dXAVB8AfE0clJ0UVKvC8ptIjxwuN9nHfP4b37AKFWYprVNgZoum2mEnExk10RQ68IIIQe9KNLZpYxK0lJ+NbXPXVT3cV4ln/fn9Wt5iwGBETiGWaYNvE68dSmbXDeJ46woqwSnxOPmnRB4keuKx6/cS64LPDMiJlOzRFHiMVCCystzIqmRjxJHFU1nfKFjMcq5y3OWqnCGvfkLwzn9OUlrtMaQgILWIQEEQoq2EAJNmK066RYSNF53Mc/6Polcink2gAjxzzK0CC7fvA/+D1bKz8x7iWF40D7i+N8DAOhXaBedZzvY8epnwDBZ+BKb/rLNWD6k/RqU4seAb3bwMV1U1P2gMsdYODJkE3ZlYK0hHweeD+jb8oCfbdA56o3t8Y5Th+ANM0qeQMcHAIjBcpe83l3R+vc/u1pzO8H+I9yds6VEEcAAAAJcEhZcwAALiMAAC4jAXilP3YAAAAHdElNRQfmAQcOFjXsyx/IAAAAGXRFWHRDb21tZW50AENyZWF0ZWQgd2l0aCBHSU1QV4EOFwAAAAxJREFUCNdj0HiTBAACtgF3wqeo5gAAAABJRU5ErkJggg==";
            const string fileExtension = "png";

            // Act
            var actual = await systemUnderTest.UploadBlob(base64Img, fileExtension);

            // Assert
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentTestData))]
#pragma warning disable xUnit1026
        public void GivenInvalidBase64Image_WhenUploadingBlob_ThenExceptionIsThrown<T>(T exception, string base64Image) where T : Exception
#pragma warning restore xUnit1026
        {
            // Arrange

            // Act
            Action act = () => systemUnderTest.UploadBlob(base64Image, It.IsAny<string>());

            // Assert
            act.Should().ThrowExactly<T>();
        }

        [Theory]
        [MemberData(nameof(InvalidArgumentTestData))]
#pragma warning disable xUnit1026
        public void GivenInvalidFileExtension_WhenUploadingBlob_ThenExceptionIsThrown<T>(T exception, string fileExtension) where T : Exception
#pragma warning restore xUnit1026
        {
            // Arrange
            const string base64Image = "x";

            // Act
            Action act = () => systemUnderTest.UploadBlob(base64Image, fileExtension);

            // Assert
            act.Should().ThrowExactly<T>();
        }

        public static IEnumerable<object[]> InvalidArgumentTestData()
        {
            yield return new object[] { new ArgumentNullException(), null };
            yield return new object[] { new ArgumentException(), "" };
            yield return new object[] { new ArgumentException(), " " };
        }

    }
}
