namespace TeamsMonitor.Core.Models;

/// <summary>
/// Response from the Teams WebSocket API
/// </summary>
public class ServiceResponse
{
    /// <summary>
    /// Request ID of the request this is a response to
    /// </summary>
    public int RequestId { get; set; }
    /// <summary>
    /// Response from the service
    /// </summary>
    public string? Response { get; set; }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    public override string ToString()
    {
        return $"ServiceResponse {{ RequestId = {RequestId}, Response = {Response} }}";
    }
}
