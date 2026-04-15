using System;

namespace StockApp.Application.DTO
{
    public class NewsResponse
    {
        public int ID { get; set; }
        public string Headline { get; set; }
        public string Text { get; set; }
        public string Image { get; set; }
    }
}
