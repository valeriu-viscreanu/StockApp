using System;

namespace StockApp.Application.DTO
{
    public class NewsResponse
    {
        public int ID { get; set; }
        public string Headline { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public string Category { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
    }
}
