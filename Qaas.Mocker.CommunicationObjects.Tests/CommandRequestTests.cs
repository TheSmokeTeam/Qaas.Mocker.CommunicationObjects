using NUnit.Framework;
using Qaas.Mocker.CommunicationObjects.ConfigurationObjects.Command;

namespace Qaas.Mocker.CommunicationObjects.Tests;

[TestFixture]
public class CommandRequestTests
{
    [Test]
    public void AppendObjectToRelevantCommandConfig_WithChangeActionStubCommand_ShouldAssignChangeActionStub()
    {
        // Arrange
        var config = new ChangeActionStub { ActionName = "action", StubName = "stub" };
        var request = new CommandRequest { Command = CommandType.ChangeActionStub };

        // Act
        request.AppendObjectToRelevantCommandConfig(config);

        // Assert
        Assert.That(request.ChangeActionStub, Is.EqualTo(config));
        Assert.That(request.Consume, Is.Null);
        Assert.That(request.TriggerAction, Is.Null);
    }

    [Test]
    public void AppendObjectToRelevantCommandConfig_WithConsumeCommand_ShouldAssignConsume()
    {
        // Arrange
        var config = new Consume { TimeoutMs = 1000 };
        var request = new CommandRequest { Command = CommandType.Consume };

        // Act
        request.AppendObjectToRelevantCommandConfig(config);

        // Assert
        Assert.That(request.Consume, Is.EqualTo(config));
        Assert.That(request.ChangeActionStub, Is.Null);
        Assert.That(request.TriggerAction, Is.Null);
    }

    [Test]
    public void AppendObjectToRelevantCommandConfig_WithTriggerActionCommand_ShouldAssignTriggerAction()
    {
        // Arrange
        var config = new TriggerAction { ActionName = "action", TimeoutMs = 1000 };
        var request = new CommandRequest { Command = CommandType.TriggerAction };

        // Act
        request.AppendObjectToRelevantCommandConfig(config);

        // Assert
        Assert.That(request.TriggerAction, Is.EqualTo(config));
        Assert.That(request.ChangeActionStub, Is.Null);
        Assert.That(request.Consume, Is.Null);
    }

    [Test]
    public void AppendObjectToRelevantCommandConfig_WithUnknownCommand_ShouldNotAssignAnyConfig()
    {
        // Arrange
        var request = new CommandRequest { Command = (CommandType)999 };

        // Act
        request.AppendObjectToRelevantCommandConfig(new object());

        // Assert
        Assert.That(request.ChangeActionStub, Is.Null);
        Assert.That(request.Consume, Is.Null);
        Assert.That(request.TriggerAction, Is.Null);
    }

    [Test]
    public void AppendObjectToRelevantCommandConfig_WithMismatchedConfigType_ShouldThrowInvalidCastException()
    {
        // Arrange
        var request = new CommandRequest { Command = CommandType.Consume };

        // Act / Assert
        Assert.Throws<InvalidCastException>(() => request.AppendObjectToRelevantCommandConfig(new TriggerAction()));
    }
}
