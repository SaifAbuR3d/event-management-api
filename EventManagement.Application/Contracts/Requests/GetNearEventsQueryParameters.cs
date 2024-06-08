namespace EventManagement.Application.Contracts.Requests;
public class GetNearEventsQueryParameters
{
    /// <summary>
    /// latitude of the location, 0 by default, should be between -90 and 90
    /// </summary>
    public double Latitude { get; set; } = 0;
    /// <summary>
    /// longitude of the location, 0 by default, should be between -180 and 180
    /// </summary>
    public double Longitude { get; set; } = 0;
    /// <summary>
    /// Maximum distance in KM, 50 by default, should be between 1 and 10000
    /// </summary>
    public int MaximumDistanceInKM { get; set; } = 50;
    /// <summary>
    /// Number of events to return, 20 by default, should be between 1 and 100
    /// </summary>
    public int NumberOfEvents { get; set; } = 20;
}
