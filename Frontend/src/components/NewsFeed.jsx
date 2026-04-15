import React, { useState, useEffect } from 'react';
import { fetchNews } from '../services/api';

function NewsFeed() {
    const [newsItems, setNewsItems] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        const getNews = async () => {
            try {
                const data = await fetchNews();
                if (data) {
                    setNewsItems(data);
                }
            } catch (error) {
                console.error('Error fetching news:', error);
            } finally {
                setLoading(false);
            }
        };
        getNews();
    }, []);

    if (loading) {
        return (
            <div className="news-feed-container">
                <h2 className="news-title">Market News Feed</h2>
                <p>Loading news...</p>
            </div>
        );
    }

    return (
        <div className="news-feed-container">
            <h2 className="news-title">Market News Feed</h2>
            <div className="news-scroll-area">
                {newsItems.map((item) => (
                    <div key={item.id || item.ID} className="news-item">
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
