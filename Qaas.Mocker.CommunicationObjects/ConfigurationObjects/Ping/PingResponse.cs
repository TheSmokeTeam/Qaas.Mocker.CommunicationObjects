using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace Qaas.Mocker.CommunicationObjects.ConfigurationObjects.Ping;

[ExcludeFromCodeCoverage]
public record PingResponse
{ 
    public string Id { get; set; }
    public string ServerName { get; set; }
    public string ServerInstanceId { get; set; }
    
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public InputOutputState ServerInputOutputState { get; set; }
}