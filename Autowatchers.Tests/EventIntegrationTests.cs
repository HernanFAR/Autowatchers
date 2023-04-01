using System.Text.Json;
using Microsoft.Extensions.Logging;
using Moq;

namespace Autowatchers.Tests;

public class EventIntegrationTests
{
    [Fact]
    public void EventIntegration_ShouldCallLogger_WhenPropertyGetSetStringIsModified_DetailIsDeep()
    {
        // Arrange
        var logger = Mock.Of<ILogger<Net7.DeepWatchers.WithBlockNamespace>>();
        var loggerMock = Mock.Get(logger);

        using var @object = new Net7.DeepWatchers.WithBlockNamespace(logger);
        var beforeString = JsonSerializer.Serialize(@object.DummyClass);

        //Act
        @object.Test1ThatModifies();

        // Assert
        Assert.Equal(Net7.DeepWatchers.WithBlockNamespace.NewName, @object.DummyClass.GetSetString);
        Assert.Equal(Net7.DeepWatchers.WithBlockNamespace.OldNestedValue, @object.DummyClass.NestedClass);

        var afterString = JsonSerializer.Serialize(@object.DummyClass);

        loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == $"Se ha escrito la propiedad GetSetString: Nuevo valor: {afterString} y antiguo valor: {beforeString}."),
                It.IsAny<Exception?>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), Times.Once);

    }

    [Fact]
    public void EventIntegration_ShouldCallLogger_WhenPropertyNestedClassIsModified_DetailIsDeep()
    {
        // Arrange
        var logger = Mock.Of<ILogger<Net7.DeepWatchers.WithBlockNamespace>>();
        var loggerMock = Mock.Get(logger);

        using var @object = new Net7.DeepWatchers.WithBlockNamespace(logger);
        var beforeString = JsonSerializer.Serialize(@object.DummyClass);

        //Act
        @object.Test2ThatModifies();

        // Assert
        Assert.Equal(Net7.DeepWatchers.WithBlockNamespace.OldName, @object.DummyClass.GetSetString);
        Assert.Equal(Net7.DeepWatchers.WithBlockNamespace.NewNestedValue, @object.DummyClass.NestedClass);

        var afterString = JsonSerializer.Serialize(@object.DummyClass);

        loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == $"Se ha escrito la propiedad NestedClass: Nuevo valor: {afterString} y antiguo valor: {beforeString}."),
                It.IsAny<Exception?>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), Times.Once);

    }

    [Fact]
    public void EventIntegration_ShouldCallLogger_WhenAPropertyIsModified_DetailIsNormal()
    {
        // Arrange
        var logger = Mock.Of<ILogger<Net7.NormalWatchers.WithBlockNamespace>>();
        var loggerMock = Mock.Get(logger);

        using var @object = new Net7.NormalWatchers.WithBlockNamespace(logger);
        var beforeString = @object.DummyClass.GetSetString;

        //Act
        @object.TestThatModifies();

        // Assert
        Assert.Equal(Net7.DeepWatchers.WithBlockNamespace.NewName, @object.DummyClass.GetSetString);
        Assert.Equal(Net7.DeepWatchers.WithBlockNamespace.OldNestedValue, @object.DummyClass.NestedClass);

        var afterString = @object.DummyClass.GetSetString;

        loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Information),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString() == $"Se ha escrito la propiedad GetSetString: Nuevo valor: {afterString} y antiguo valor: {beforeString}."),
                It.IsAny<Exception?>(),
                It.Is<Func<It.IsAnyType, Exception?, string>>((v, t) => true)), Times.Once);


    }
}
