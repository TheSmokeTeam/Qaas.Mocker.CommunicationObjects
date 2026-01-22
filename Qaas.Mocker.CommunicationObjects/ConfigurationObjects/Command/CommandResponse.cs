using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Qaas.Mocker.CommunicationObjects.ConfigurationObjects.Command;

[ExcludeFromCodeCoverage]
public record CommandResponse
{
    public string Id { get; init; }

    public string ServerInstanceId { get; init; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public CommandType Command { get; init; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Status Status { get; init; }
    
    public string? ExceptionMessage { get; init; }
}