using NUnit.Framework;

namespace Qaas.Mocker.CommunicationObjects.Tests;

[TestFixture]
public class CommunicationMethodsTests
{
    [Test, 
     TestCase("runner-to-mocker:testcontent", "tEstconteNt"),
     TestCase("runner-to-mocker:testcontent2:testserver", "testCOntent2", "teStServer"),
     TestCase("runner-to-mocker:testcontent2:testserver:testinstance", "testCOntent2", "teStServer", "testinSTANce")]
    public void TestCreateChannelRunnerToMocker_GenerateRoutingWithGivenParameters_ShouldGenerateAsExpected(
        string expectedResult, string contentType, string? serverName = null, string? serverInstanceName = null)
    {
        // Act
        var actualResult = CommunicationMethods.CreateChannelRunnerToMocker(contentType, serverName, serverInstanceName);

        // Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public void TestCreateChannelRunnerToMocker_GenerateRoutingWithInstanceOnly_ShouldGenerateAsExpected()
    {
        // Act
        var actualResult = CommunicationMethods.CreateChannelRunnerToMocker("conTent", serverInstanceName: "inStanCe");

        // Assert
        Assert.That(actualResult, Is.EqualTo("runner-to-mocker:content:instance"));
    }

    [Test]
    public void TestCreateChannelRunnerToMocker_GenerateRoutingWithEmptyServerName_ShouldKeepEmptyBranchSegment()
    {
        // Act
        var actualResult = CommunicationMethods.CreateChannelRunnerToMocker("conTent", string.Empty);

        // Assert
        Assert.That(actualResult, Is.EqualTo("runner-to-mocker:content:"));
    }
    
    [Test, 
     TestCase("mocker-to-runner:testcontent", "tEstconteNt"),
     TestCase("mocker-to-runner:testcontent2:testserver", "testCOntent2", "teStServer"),
     TestCase("mocker-to-runner:testcontent2:testserver:testinstance", "testCOntent2", "teStServer", "testinSTANce")]
    public void TestCreateChannelMockerToRunner_GenerateRoutingWithGivenParameters_ShouldGenerateAsExpected(
        string expectedResult, string contentType, string? serverName = null, string? serverInstanceName = null)
    {
        // Act
        var actualResult = CommunicationMethods.CreateChannelMockerToRunner(contentType, serverName, serverInstanceName);

        // Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test]
    public void TestCreateChannelMockerToRunner_GenerateRoutingWithInstanceOnly_ShouldGenerateAsExpected()
    {
        // Act
        var actualResult = CommunicationMethods.CreateChannelMockerToRunner("conTent", serverInstanceName: "inStanCe");

        // Assert
        Assert.That(actualResult, Is.EqualTo("mocker-to-runner:content:instance"));
    }

    [Test]
    public void TestCreateChannelMockerToRunner_GenerateRoutingWithEmptyServerName_ShouldKeepEmptyBranchSegment()
    {
        // Act
        var actualResult = CommunicationMethods.CreateChannelMockerToRunner("conTent", string.Empty);

        // Assert
        Assert.That(actualResult, Is.EqualTo("mocker-to-runner:content:"));
    }

    [Test, TestCase("testcontent:input", "tEstconteNt"), TestCase(":input", "")]
    public void TestCreateConsumerEndpointInput_GenerateRoutingWithGivenParameters_ShouldGenerateAsExpected(
        string expectedResult, string contentType)
    {
        // Act
        var actualResult = CommunicationMethods.CreateConsumerEndpointInput(contentType);

        // Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }

    [Test, TestCase("testcontent:output", "tEstconteNt"), TestCase(":output", "")]
    public void TestCreateConsumerEndpointOutput_GenerateRoutingWithGivenParameters_ShouldGenerateAsExpected(
        string expectedResult, string contentType)
    {
        // Act
        var actualResult = CommunicationMethods.CreateConsumerEndpointOutput(contentType);

        // Assert
        Assert.That(actualResult, Is.EqualTo(expectedResult));
    }
}
