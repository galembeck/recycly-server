using Microsoft.Extensions.Logging;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Domain.Services.Geocoding;

public class GeocodingService(IHttpClientFactory httpClientFactory, ILogger<GeocodingService> logger) : IGeocodingService
{
    private const string ClientName = "Nominatim";
    private const string SearchUrl = "/search?q={0}&format=json&limit=1";

    public async Task<(string Latitude, string Longitude)?> GetCoordinatesAsync(string address, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = httpClientFactory.CreateClient(ClientName);
            var encoded = Uri.EscapeDataString(address);
            var url = string.Format(SearchUrl, encoded);

            logger.LogInformation("Geocoding: querying Nominatim for \"{Address}\"", address);

            var results = await client.GetFromJsonAsync<NominatimResult[]>(url, cancellationToken);

            if (results is null || results.Length == 0)
            {
                logger.LogWarning("Geocoding: no results for \"{Address}\"", address);
                return null;
            }

            logger.LogInformation("Geocoding: resolved \"{Address}\" → {Lat}, {Lon}", address, results[0].Lat, results[0].Lon);
            return (results[0].Lat, results[0].Lon);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Geocoding: exception for \"{Address}\"", address);
            return null;
        }
    }

    private record NominatimResult(
        [property: JsonPropertyName("lat")] string Lat,
        [property: JsonPropertyName("lon")] string Lon);
}
