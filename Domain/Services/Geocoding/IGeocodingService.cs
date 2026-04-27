namespace Domain.Services.Geocoding;

public interface IGeocodingService
{
    Task<(string Latitude, string Longitude)?> GetCoordinatesAsync(string address, CancellationToken cancellationToken = default);
}
