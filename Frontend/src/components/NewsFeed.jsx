import React from 'react';

const newsItems = [
    {
        id: 1,
        headline: "Tech Giants Surge as Q1 Earnings Beat Expectations",
        text: "Major technology companies saw their stock prices climb today following strong quarterly earnings reports that exceeded analyst predictions. Investors are optimistic about continued growth in the AI sector.",
        image: "https://images.unsplash.com/photo-1518770660439-4636190af475?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 2,
        headline: "Federal Reserve Hints at Potential Rate Cuts Later This Year",
        text: "In a recent statement, the Federal Reserve chair suggested that inflation is cooling down, which could pave the way for interest rate reductions by the end of the year. Markets reacted positively to the news.",
        image: "https://images.unsplash.com/photo-1526304640581-d334cdbbf45e?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 3,
        headline: "Energy Sector Faces Volatility Amid Geopolitical Tensions",
        text: "Ongoing conflicts in major oil-producing regions have led to significant price swings in the energy market. Analysts are closely monitoring the situation for potential long-term impacts on global supply chains.",
        image: "https://images.unsplash.com/photo-1466611653911-95282fc3656d?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 4,
        headline: "Sustainable Stocks Continue to Attract ESG Investors",
        text: "The demand for environmentally conscious investments remains high, with green energy and carbon-neutral companies outperforming their traditional counterparts in recent months.",
        image: "https://images.unsplash.com/photo-1473341304170-971dccb5ac1e?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 5,
        headline: "Global Trade Outlook Improves as Supply Chain Disruptions Ease",
        text: "Manufacturers and retailers are reporting fewer delays and lower shipping costs as the global supply chain continues to stabilize after years of uncertainty. This trend is expected to boost consumer spending.",
        image: "https://images.unsplash.com/photo-1586528116311-ad861a5c6439?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 6,
        headline: "Automakers Rev Up Electric Vehicle Production Strategies",
        text: "Several leading car manufacturers have announced ambitious plans to transition their entire fleets to electric vehicles by the next decade, signaling a major shift in the automotive industry.",
        image: "https://images.unsplash.com/photo-1593941707882-a5bba14938c7?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 7,
        headline: "Retail Sales Resilience Boosts Confidence in Consumer Spending",
        text: "Despite inflationary pressures, retail sales have remained remarkably resilient, suggesting that consumer confidence is holding steady as people continue to spend on essential and non-essential goods.",
        image: "https://images.unsplash.com/photo-1534452203294-46c8ad093716?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 8,
        headline: "Fintech Startups Disrupt Traditional Banking Models",
        text: "A new wave of financial technology startups is gaining traction by offering innovative and user-friendly alternatives to traditional banking services, forcing established banks to adapt quickly.",
        image: "https://images.unsplash.com/photo-1551288049-bebda4e38f71?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 9,
        headline: "Cloud Computing Demand Remains Strong for Enterprise Digital Transformation",
        text: "Businesses are increasingly migrating their operations to the cloud to improve efficiency and scalability, leading to sustained demand for cloud service providers and related infrastructure.",
        image: "https://images.unsplash.com/photo-1451187580459-43490279c0fa?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 10,
        headline: "Pharmaceutical Innovations Lead to Breakthroughs in Healthcare",
        text: "Significant advancements in biotech and pharmaceutical research have resulted in promising new treatments for previously incurable diseases, creating new opportunities for investors in the life sciences sector.",
        image: "https://images.unsplash.com/photo-1532187875605-181bd84a70d4?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 11,
        headline: "AI Development Spurs Rivalry Between Silicon Valley Giants",
        text: "The race to lead the artificial intelligence market has intensified as top technology firms compete to develop the most advanced language models and AI-integrated products.",
        image: "https://images.unsplash.com/photo-1677442136019-21780ecad995?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 12,
        headline: "Crypto Market Shows Signs of Recovery After Period of Consolidation",
        text: "After months of downward trends, several major cryptocurrencies are beginning to show signs of stabilizing and gaining value as investor sentiment starts to shift in a more positive direction.",
        image: "https://images.unsplash.com/photo-1518546305927-5a555bb7020d?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 13,
        headline: "Labor Market Strength Persists Despite Economic Headwinds",
        text: "Recent data shows that employment numbers remain strong across various sectors, even as the global economy faces challenges from inflation and geopolitical instability.",
        image: "https://images.unsplash.com/photo-1486406146926-c627a92ad1ab?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 14,
        headline: "Green Hydrogen Gains Traction as Key to Decarbonization",
        text: "Governments and private industries are increasingly turning towards green hydrogen as a sustainable energy alternative to help achieve global climate goals and reduce reliance on fossil fuels.",
        image: "https://images.unsplash.com/photo-1624391913340-9710cc39f37c?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 15,
        headline: "Consumer Electronics Market Braces for Next Generation of Smart Devices",
        text: "Innovative startups and established manufacturers are preparing to launch a new wave of smart gadgets, from wearable health monitors to advanced home automation systems.",
        image: "https://images.unsplash.com/photo-1498050108023-c5249f4df085?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 16,
        headline: "Agricultural Technology Innovations Address Global Food Security Challenges",
        text: "Advancements in precision farming and automated agricultural systems are helping farmers improve crop yields and resource efficiency to meet the growing global demand for food.",
        image: "https://images.unsplash.com/photo-1560493676-04071c5f467b?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 17,
        headline: "Digital Payment Adoption Continues to Rise Globally",
        text: "The shift away from traditional cash transactions is accelerating as more consumers and businesses adopt digital payment solutions for their daily needs, driving growth in the fintech sector.",
        image: "https://images.unsplash.com/photo-1563013544-824ae1b704d3?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 18,
        headline: "Residential Real Estate Market Adjusts to Higher Mortgage Rates",
        text: "Higher borrowing costs have led to a slowdown in home sales and a stabilization of property prices in several major markets as buyers and sellers adapt to the current economic environment.",
        image: "https://images.unsplash.com/photo-1560518883-ce09059eeffa?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 19,
        headline: "Cybersecurity Becomes Priority for Businesses Amid Growing Threats",
        text: "With increasing digitalization comes a rise in cyberattacks, leading companies to invest heavily in advanced security solutions to protect their data and maintain customer trust.",
        image: "https://images.unsplash.com/photo-1550751827-4bd374c3f58b?auto=format&fit=crop&q=80&w=200&h=150"
    },
    {
        id: 20,
        headline: "Space Exploration Sector Attracts Increased Private Investment",
        text: "A new era of space commercialization is underway as private firms continue to develop cost-effective launch capabilities and explore opportunities for lunar and orbital infrastructure.",
        image: "https://images.unsplash.com/photo-1446776811953-b23d57bd21aa?auto=format&fit=crop&q=80&w=200&h=150"
    }
];

function NewsFeed() {
    return (
        <div className="news-feed-container">
            <h2 className="news-title">Market News Feed</h2>
            <div className="news-scroll-area">
                {newsItems.map((item) => (
                    <div key={item.id} className="news-item">
                        <div className="news-image-container">
                            <img src={item.image} alt="News thumbnail" className="news-image" />
                        </div>
                        <div className="news-content">
                            <h3 className="news-headline">{item.headline}</h3>
                            <p className="news-text">{item.text}</p>
                        </div>
                    </div>
                ))}
            </div>
        </div>
    );
}

export default NewsFeed;
