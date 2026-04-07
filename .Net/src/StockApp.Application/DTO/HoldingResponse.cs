namespace StockApp.Application.DTO
{
    public class HoldingResponse
    {
        public string StockSymbol { get; set; } = string.Empty;
        public string StockName { get; set; } = string.Empty;
        public uint Quantity { get; set; }
    }
}
