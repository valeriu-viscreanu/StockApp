using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StockApp.Application.DTO;
using StockApp.Application.Services;
using System.Collections.Generic;

namespace StockApp.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class NewsController : ControllerBase
    {
        private readonly INewsService _newsService;

        public NewsController(INewsService newsService)
        {
            _newsService = newsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<NewsResponse>>> GetNews([FromQuery] int count = 20)
        {
            var news = await _newsService.GetLatestNewsAsync(count);
            if (!news.Any())
            {
                return Ok(GetDefaultNews());
            }
            return Ok(news);
        }

        private List<NewsResponse> GetDefaultNews()
        {
            var newsItems = new List<NewsResponse>
            {
                new NewsResponse {
                    ID = 1,
                    Headline = "Tech Giants Surge as Q1 Earnings Beat Expectations",
                    Text = "Major technology companies saw their stock prices climb today following strong quarterly earnings reports that exceeded analyst predictions. Investors are optimistic about continued growth in the AI sector.",
                    Image = "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 2,
                    Headline = "Federal Reserve Hints at Potential Rate Cuts Later This Year",
                    Text = "In a recent statement, the Federal Reserve chair suggested that inflation is cooling down, which could pave the way for interest rate reductions by the end of the year. Markets reacted positively to the news.",
                    Image = "https://images.unsplash.com/photo-1526304640581-d334cdbbf45e?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 3,
                    Headline = "Energy Sector Faces Volatility Amid Geopolitical Tensions",
                    Text = "Ongoing conflicts in major oil-producing regions have led to significant price swings in the energy market. Analysts are closely monitoring the situation for potential long-term impacts on global supply chains.",
                    Image = "https://images.unsplash.com/photo-1466611653911-95282fc3656d?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 4,
                    Headline = "Sustainable Stocks Continue to Attract ESG Investors",
                    Text = "The demand for environmentally conscious investments remains high, with green energy and carbon-neutral companies outperforming their traditional counterparts in recent months.",
                    Image = "https://images.unsplash.com/photo-1473341304170-971dccb5ac1e?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 5,
                    Headline = "Global Trade Outlook Improves as Supply Chain Disruptions Ease",
                    Text = "Manufacturers and retailers are reporting fewer delays and lower shipping costs as the global supply chain continues to stabilize after years of uncertainty. This trend is expected to boost consumer spending.",
                    Image = "https://images.unsplash.com/photo-1586528116311-ad861a5c6439?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 6,
                    Headline = "Automakers Rev Up Electric Vehicle Production Strategies",
                    Text = "Several leading car manufacturers have announced ambitious plans to transition their entire fleets to electric vehicles by the next decade, signaling a major shift in the automotive industry.",
                    Image = "https://images.unsplash.com/photo-1593941707882-a5bba14938c7?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 7,
                    Headline = "Retail Sales Resilience Boosts Confidence in Consumer Spending",
                    Text = "Despite inflationary pressures, retail sales have remained remarkably resilient, suggesting that consumer confidence is holding steady as people continue to spend on essential and non-essential goods.",
                    Image = "https://images.unsplash.com/photo-1534452203294-46c8ad093716?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 8,
                    Headline = "Fintech Startups Disrupt Traditional Banking Models",
                    Text = "A new wave of financial technology startups is gaining traction by offering innovative and user-friendly alternatives to traditional banking services, forcing established banks to adapt quickly.",
                    Image = "https://images.unsplash.com/photo-1551288049-bebda4e38f71?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 9,
                    Headline = "Cloud Computing Demand Remains Strong for Enterprise Digital Transformation",
                    Text = "Businesses are increasingly migrating their operations to the cloud to improve efficiency and scalability, leading to sustained demand for cloud service providers and related infrastructure.",
                    Image = "https://images.unsplash.com/photo-1451187580459-43490279c0fa?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 10,
                    Headline = "Pharmaceutical Innovations Lead to Breakthroughs in Healthcare",
                    Text = "Significant advancements in biotech and pharmaceutical research have resulted in promising new treatments for previously incurable diseases, creating new opportunities for investors in the life sciences sector.",
                    Image = "https://images.unsplash.com/photo-1532187875605-181bd84a70d4?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 11,
                    Headline = "AI Development Spurs Rivalry Between Silicon Valley Giants",
                    Text = "The race to lead the artificial intelligence market has intensified as top technology firms compete to develop the most advanced language models and AI-integrated products.",
                    Image = "https://images.unsplash.com/photo-1677442136019-21780ecad995?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 12,
                    Headline = "Crypto Market Shows Signs of Recovery After Period of Consolidation",
                    Text = "After months of downward trends, several major cryptocurrencies are beginning to show signs of stabilizing and gaining value as investor sentiment starts to shift in a more positive direction.",
                    Image = "https://images.unsplash.com/photo-1518546305927-5a555bb7020d?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 13,
                    Headline = "Labor Market Strength Persists Despite Economic Headwinds",
                    Text = "Recent data shows that employment numbers remain strong across various sectors, even as the global economy faces challenges from inflation and geopolitical instability.",
                    Image = "https://images.unsplash.com/photo-1486406146926-c627a92ad1ab?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 14,
                    Headline = "Green Hydrogen Gains Traction as Key to Decarbonization",
                    Text = "Governments and private industries are increasingly turning towards green hydrogen as a sustainable energy alternative to help achieve global climate goals and reduce reliance on fossil fuels.",
                    Image = "https://images.unsplash.com/photo-1624391913340-9710cc39f37c?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 15,
                    Headline = "Consumer Electronics Market Braces for Next Generation of Smart Devices",
                    Text = "Innovative startups and established manufacturers are preparing to launch a new wave of smart gadgets, from wearable health monitors to advanced home automation systems.",
                    Image = "https://images.unsplash.com/photo-1498050108023-c5249f4df085?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 16,
                    Headline = "Agricultural Technology Innovations Address Global Food Security Challenges",
                    Text = "Advancements in precision farming and automated agricultural systems are helping farmers improve crop yields and resource efficiency to meet the growing global demand for food.",
                    Image = "https://images.unsplash.com/photo-1560493676-04071c5f467b?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 17,
                    Headline = "Digital Payment Adoption Continues to Rise Globally",
                    Text = "The shift away from traditional cash transactions is accelerating as more consumers and businesses adopt digital payment solutions for their daily needs, driving growth in the fintech sector.",
                    Image = "https://images.unsplash.com/photo-1563013544-824ae1b704d3?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 18,
                    Headline = "Residential Real Estate Market Adjusts to Higher Mortgage Rates",
                    Text = "Higher borrowing costs have led to a slowdown in home sales and a stabilization of property prices in several major markets as buyers and sellers adapt to the current economic environment.",
                    Image = "https://images.unsplash.com/photo-1560518883-ce09059eeffa?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 19,
                    Headline = "Cybersecurity Becomes Priority for Businesses Amid Growing Threats",
                    Text = "With increasing digitalization comes a rise in cyberattacks, leading companies to invest heavily in advanced security solutions to protect their data and maintain customer trust.",
                    Image = "https://images.unsplash.com/photo-1550751827-4bd374c3f58b?auto=format&fit=crop&q=80&w=200&h=150"
                },
                new NewsResponse {
                    ID = 20,
                    Headline = "Space Exploration Sector Attracts Increased Private Investment",
                    Text = "A new era of space commercialization is underway as private firms continue to develop cost-effective launch capabilities and explore opportunities for lunar and orbital infrastructure.",
                    Image = "https://images.unsplash.com/photo-1446776811953-b23d57bd21aa?auto=format&fit=crop&q=80&w=200&h=150"
                }
            };

            return newsItems;
        }
    }
}
