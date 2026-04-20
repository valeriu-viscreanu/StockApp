import React from 'react';

function Holdings({ userHoldings = [], popularStocks = [] }) {
    const isDummy = userHoldings.length === 0;
    const holdingsToDisplay = isDummy ? [
        { stockSymbol: 'AAPL', stockName: 'Apple Inc.', quantity: 10 },
        { stockSymbol: 'MSFT', stockName: 'Microsoft Corp.', quantity: 5 },
        { stockSymbol: 'TSLA', stockName: 'Tesla Inc.', quantity: 2 }
    ] : userHoldings;

    // Merge holdings with current prices from popularStocks if available
    const displayHoldings = holdingsToDisplay.map(h => {
        const stockInfo = popularStocks.find(s => s.symbol.toUpperCase() === h.stockSymbol.toUpperCase());
        const currentPrice = stockInfo?.price || (isDummy ? (h.stockSymbol === 'AAPL' ? 150 : h.stockSymbol === 'MSFT' ? 300 : 200) : 0);
        return {
            ...h,
            currentPrice,
            totalValue: currentPrice * h.quantity
        };
    });

    const totalPortfolioValue = displayHoldings.reduce((sum, h) => sum + h.totalValue, 0);

    return (
        <div className="main-view-panel">
            <div className="news-feed-container" style={{ marginTop: 0 }}>
                <h1 className="news-title">
                    My Holdings {isDummy && <span style={{ fontSize: '0.9rem', color: 'var(--text-muted)', fontWeight: 'normal' }}>(Demo Data)</span>}
                </h1>

                <div className="orders-column">
                    <div className="stats-grid" style={{ marginBottom: '30px', gridTemplateColumns: 'repeat(auto-fit, minmax(200px, 1fr))' }}>
                        <div className="stat-card">
                            <div className="stat-label">Total Holdings Value</div>
                            <div className="stat-value" style={{ color: 'var(--primary)' }}>
                                ${totalPortfolioValue.toLocaleString(undefined, { minimumFractionDigits: 2 })}
                            </div>
                        </div>
                        <div className="stat-card">
                            <div className="stat-label">Unique Stocks Items</div>
                            <div className="stat-value">{displayHoldings.length}</div>
                        </div>
                    </div>

                    <div className="holdings-list">
                        {displayHoldings.map(holding => (
                            <div key={holding.stockSymbol} className="order-card" style={{ display: 'flex', justifyContent: 'space-between', alignItems: 'center' }}>
                                <div className="order-details-wrapper">
                                    <div className="order-stock-name">{holding.stockName} ({holding.stockSymbol})</div>
                                    <div className="order-details">
                                        Quantity: <span className="order-quantity">{holding.quantity}</span> shares
                                        {holding.currentPrice > 0 && (
                                            <> | Current Price: <span className="order-price">${holding.currentPrice.toFixed(2)}</span></>
                                        )}
                                    </div>
                                </div>
                                <div style={{ textAlign: 'right' }}>
                                    <div style={{ fontSize: '1.2rem', fontWeight: '800', color: 'var(--text)' }}>
                                        ${holding.totalValue.toLocaleString(undefined, { minimumFractionDigits: 2 })}
                                    </div>
                                    <div style={{ fontSize: '0.75rem', color: 'var(--text-muted)', textTransform: 'uppercase', letterSpacing: '0.5px' }}>
                                        Market Value
                                    </div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>

                <div style={{ marginTop: '40px', paddingTop: '20px', borderTop: '1px solid var(--border)', fontSize: '0.75rem', color: 'var(--text-muted)', fontStyle: 'italic', textAlign: 'center' }}>
                    * Holdings data is synced with your account profile. Prices are updated periodically from live market data.
                    <br />
                    <span style={{ fontSize: '0.65rem', opacity: 0.7 }}>
                        {isDummy
                            ? "Fine print: You currently have no owned stocks. Displaying dummy data for showcase purposes as per system instructions."
                            : "Fine print: This view uses live portfolio data from the StockApp API core services."}
                    </span>
                </div>
            </div>
        </div>
    );
}

export default Holdings;
