namespace ShopActivityMonitoring.Application.Interfaces;

public interface ICsvReportService
{
    Task<byte[]> GenerateEventsCsvAsync();
}