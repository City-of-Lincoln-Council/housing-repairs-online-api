using System;
using System.Collections.Generic;
using Amazon.DynamoDBv2;
using FluentAssertions;
using HousingRepairsOnlineApi.Helpers;
using Moq;
using Xunit;

namespace HousingRepairsOnlineApi.Tests.HelpersTests;

public class DynamoDbIncrementingIdGeneratorTests
{
    private readonly IAmazonDynamoDB dynamoDbMock = new Mock<IAmazonDynamoDB>().Object;
    private const string TableName = "test";
    private const string PartitionKey = "PartitionKey";

    [Fact]
    public void GivenNullDynamoDbArgument_WhenConstructing_ThenArgumentNullExceptionIsThrown()
    {
        // Arrange

        // Act
        Action act = () =>
            _ = new DynamoDbIncrementingIdGenerator(null, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>());

        // Assert
        act.Should().Throw<ArgumentNullException>();
    }

    [Theory]
    [MemberData(nameof(InvalidStringArgumentTestData))]
#pragma warning disable xUnit1026
    public void GivenAnInvalidTableName_WhenConstructing_ThenExceptionIsThrown<T>(T exception, string tableName) where T : Exception
#pragma warning restore xUnit1026
    {
        // Arrange

        // Act
        Action act = () =>
            _ = new DynamoDbIncrementingIdGenerator(dynamoDbMock, tableName, It.IsAny<string>(), It.IsAny<int>());

        // Assert
        act.Should().ThrowExactly<T>();
    }

    [Theory]
    [MemberData(nameof(InvalidStringArgumentTestData))]
#pragma warning disable xUnit1026
    public void GivenAnInvalidPartitionKey_WhenConstructing_ThenExceptionIsThrown<T>(T exception, string partitionKey) where T : Exception
#pragma warning restore xUnit1026
    {
        // Arrange

        // Act
        Action act = () =>
            _ = new DynamoDbIncrementingIdGenerator(dynamoDbMock, TableName, partitionKey, It.IsAny<int>());

        // Assert
        act.Should().ThrowExactly<T>();
    }

    [Theory]
    [MemberData(nameof(InvalidNumberArgumentTestData))]
#pragma warning disable xUnit1026
    public void GivenAnInvalidInitialId_WhenConstructing_ThenExceptionIsThrown<T>(T exception, int initialId) where T : Exception
#pragma warning restore xUnit1026
    {
        // Arrange

        // Act
        Action act = () =>
            _ = new DynamoDbIncrementingIdGenerator(dynamoDbMock, TableName, PartitionKey, initialId);

        // Assert
        act.Should().ThrowExactly<T>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(100)]
    [InlineData(20_000_000)]
    [InlineData(int.MaxValue)]
#pragma warning disable xUnit1026
    public void GivenAValidInitialId_WhenConstructing_ThenNoExceptionIsThrown(int initialId)
#pragma warning restore xUnit1026
    {
        // Arrange

        // Act
        Action act = () =>
            _ = new DynamoDbIncrementingIdGenerator(dynamoDbMock, TableName, PartitionKey, initialId);

        // Assert
        act.Should().NotThrow();
    }

    public static IEnumerable<object[]> InvalidStringArgumentTestData()
    {
        yield return new object[] { new ArgumentNullException(), null };
        yield return new object[] { new ArgumentException(), "" };
        yield return new object[] { new ArgumentException(), " " };
    }

    public static IEnumerable<object[]> InvalidNumberArgumentTestData()
    {
        yield return new object[] { new ArgumentOutOfRangeException(), -1 };
        yield return new object[] { new ArgumentOutOfRangeException(), int.MinValue };
    }
}
