using System.Text;
using ShopActivityMonitoring.Application.Interfaces;

namespace ShopActivityMonitoring.Application.Services;

public class CsvReportService : ICsvReportService
{
    private readonly IShopEventRepository _eventRepository;

    public CsvReportService(IShopEventRepository eventRepository)
    {
        _eventRepository = eventRepository;
    }

    public async Task<byte[]> GenerateEventsCsvAsync()
    {
        var events = await _eventRepository.GetAllAsync();

        var sb = new StringBuilder();
        sb.AppendLine("Id;UserName;ProductName;Category;EventType;Quantity;TotalAmount;CreatedAt;Source");

        foreach (var e in events.OrderBy(x => x.CreatedAt))
        {
            sb.AppendLine(string.Join(";",
                e.Id,
                e.User?.UserName ?? "",
                e.Product?.Name ?? "",
                e.Product?.Category ?? "",
                e.EventType.ToString(),
                e.Quantity,
                e.TotalAmount.ToString("F2"),
                e.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                e.Source));
        }

        return Encoding.UTF8.GetPreamble().Concat(Encoding.UTF8.GetBytes(sb.ToString())).ToArray();
    }
}