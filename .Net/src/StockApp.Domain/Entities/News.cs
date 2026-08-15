using System.ComponentModel.DataAnnotations;

namespace StockApp.Domain.Entities
{
    public class News
    {
        public Guid NewsID { get; set; }

        [Required]
        public string Headline { get; set; } = string.Empty;

        [Required]
        public string Text { get; set; } = string.Empty;

        public string? Image { get; set; }

        public string? Source { get; set; }

        public DateTime PublishedDate { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        [Required]
        public string Category { get; set; } = "General";

        public string? SourceUrl { get; set; }
    }
}
