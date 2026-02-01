using MaxMind.GeoIP2;
using MaxMind.GeoIP2.Exceptions;
using Microsoft.Extensions.Logging;

namespace WT.B2C.API.BuildingBlocks.Runtime;

public interface IGeoIpService
{
    string? GetCountryIsoCode(string ipAddress);
}

public class GeoIpService : IGeoIpService, IDisposable
{
    private readonly DatabaseReader? _reader;
    private readonly ILogger<GeoIpService> _logger;

    public GeoIpService(ILogger<GeoIpService> logger)
    {
        _logger = logger;

        var dbPath = Path.Combine(AppContext.BaseDirectory, "AppData", "GeoIP", "GeoLite2-Country.mmdb");
        
        if (File.Exists(dbPath))
        {
            try
            {
                _reader = new DatabaseReader(dbPath);
                _logger.LogInformation("GeoIP database loaded successfully from: {DbPath}", dbPath);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load GeoIP database from: {DbPath}", dbPath);
                _reader = null;
            }
        }
        else
        {
            _logger.LogWarning("GeoIP database not found at: {DbPath}. Country detection will fall back to null. Please download the database - see README.md in the GeoIP directory.", dbPath);
            _reader = null;
        }
    }

    public string? GetCountryIsoCode(string ipAddress)
    {
        if (_reader == null || string.IsNullOrWhiteSpace(ipAddress))
            return null;

        try
        {
            var response = _reader.Country(ipAddress);
            return response?.Country?.IsoCode;
        }
        catch (AddressNotFoundException)
        {
            _logger.LogDebug("IP address {IpAddress} not found in GeoIP database", ipAddress);
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Error looking up IP address {IpAddress} in GeoIP database", ipAddress);
            return null;
        }
    }

    public void Dispose()
    {
        _reader?.Dispose();
    }
}
