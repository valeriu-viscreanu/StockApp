import React, { useState, useEffect } from 'react';
import PriceChart from './PriceChart';
import FavoriteToggle from './FavoriteToggle';
import * as api from '../services/api';

function Trade({ popularStocks, selectedStock, setSelectedStock, quantity, setQuantity, onBuy, onSell, tradeMessage, stockChartData, fetchStockChartData, userHoldings = [] }) {
  const [favorites, setFavorites] = useState(() => {
    const saved = localStorage.getItem('favorites');
    return saved ? JSON.parse(saved) : [];
  });
  const [timeframe, setTimeframe] = useState('month');
  const [searchQuery, setSearchQuery] = useState('');
  const [searchResults, setSearchResults] = useState([]);
  const [isSearching, setIsSearching] = useState(false);

  useEffect(() => {
    const delayDebounceFn = setTimeout(async () => {
      if (searchQuery.trim().length >= 2) {
        setIsSearching(true);
        try {
          const results = await api.searchStocks(searchQuery);
          setSearchResults(results?.result || []);
        } catch (e) {
          setSearchResults([]);
        } finally {
          setIsSearching(false);
        }
      } else {
        setSearchResults([]);
      }
    }, 500);

    return () => clearTimeout(delayDebounceFn);
  }, [searchQuery]);

  const handleTimeframeChange = (tf) => {
    setTimeframe(tf);
    fetchStockChartData(selectedStock.symbol, tf);
  };

  const toggleFavorite = (symbol) => {
    setFavorites(prev => {
      const newFavs = prev.includes(symbol)
        ? prev.filter(s => s !== symbol)
        : [...prev, symbol];
      localStorage.setItem('favorites', JSON.stringify(newFavs));
      return newFavs;
    });
  };

  const sortedStocks = [...popularStocks].sort((a, b) => {
    const symbolA = a.symbol.toUpperCase();
    const symbolB = b.symbol.toUpperCase();

    const isOwnedA = userHoldings.some(h => h.stockSymbol.toUpperCase() === symbolA);
    const isOwnedB = userHoldings.some(h => h.stockSymbol.toUpperCase() === symbolB);

    // Prioritize owned stocks
    if (isOwnedA && !isOwnedB) return -1;
    if (!isOwnedA && isOwnedB) return 1;

    const isFavoriteA = favorites.includes(symbolA);
    const isFavoriteB = favorites.includes(symbolB);

    // Prioritize favorite stocks next
    if (isFavoriteA && !isFavoriteB) return -1;
    if (!isFavoriteA && isFavoriteB) return 1;

    // Otherwise maintain order
    return 0;
  });

  return (
    <div className="advanced-dashboard">
      <div className="sidebar-panel">
        <div className="sidebar-header">
          <h3>Market Watch</h3>
        </div>

        <div className="search-container">
          <div className="search-input-wrapper">
            <span className="search-icon">🔍</span>
            <input
              type="text"
              className="search-field"
              placeholder="Search stocks..."
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
            />
          </div>

          {(searchResults.length > 0 || (searchQuery.length >= 2 && !isSearching)) && (
            <div className="search-results-dropdown">
              {searchResults.map(result => (
                <div
                  key={result.symbol}
                  className="search-result-item"
                  onClick={() => {
                    setSelectedStock({ symbol: result.symbol, name: result.description, price: 0 });
                    setSearchQuery('');
                    setSearchResults([]);
                  }}
                >
                  <div className="search-result-info">
                    <span className="search-result-symbol">{result.symbol}</span>
                    <span className="search-result-name">{result.description}</span>
                  </div>
                  <span className="search-result-type">{result.type}</span>
                </div>
              ))}
              {searchResults.length === 0 && !isSearching && searchQuery.length >= 2 && (
                <div className="no-results">No results for "{searchQuery}"</div>
              )}
            </div>
          )}
        </div>

        <div className="stock-list">
          {sortedStocks.map(stock => {
            const holding = userHoldings.find(h => h.stockSymbol.toUpperCase() === stock.symbol.toUpperCase());
            return (
              <a
                key={stock.symbol}
                href="#"
                className={`stock-item ${selectedStock.symbol === stock.symbol ? 'active' : ''}`}
                onClick={(e) => { e.preventDefault(); setSelectedStock(stock); }}
              >
                <div className="stock-icon">{stock.symbol.substring(0, 1)}</div>
                <div className="stock-details">
                  <span className="stock-symbol" style={{ display: 'flex', alignItems: 'center' }}>
                    {stock.symbol}
                    <FavoriteToggle
                      symbol={stock.symbol}
                      isFavorite={favorites.includes(stock.symbol)}
                      onToggle={toggleFavorite}
                    />
                  </span>
                  <span className="stock-company">{stock.name}</span>
                </div>
                <div className="stock-mini-chart">
                  <span className="stock-mini-price">
                    {stock.price > 0 ? `$${stock.price.toFixed(2)}` : 'Loading...'}
                  </span>
                </div>
              </a>
            );
          })}
        </div>
      </div>

      <div className="main-view-panel">
        <div className="trade-container">
          <div className="stock-info">
            <h1 className="stock-name" style={{ display: 'flex', alignItems: 'center' }}>
              {selectedStock.name} ({selectedStock.symbol})
              <FavoriteToggle
                symbol={selectedStock.symbol}
                isFavorite={favorites.includes(selectedStock.symbol)}
                onToggle={toggleFavorite}
              />
            </h1>
            <div className="stock-price">
              <span className="currency">$</span>
              <span className="price-value">
                {selectedStock.price > 0 ? selectedStock.price.toFixed(2) : '...'}
              </span>
              {userHoldings.find(h => h.stockSymbol.toUpperCase() === selectedStock.symbol.toUpperCase()) && (
                <div style={{
                  display: 'inline-flex',
                  alignItems: 'center',
                  marginLeft: '15px',
                  fontSize: '0.9rem',
                  background: 'rgba(0, 209, 158, 0.1)',
                  padding: '4px 10px',
                  borderRadius: '6px',
                  border: '1px solid rgba(0, 209, 158, 0.2)',
                  color: 'var(--primary)',
                  fontWeight: '600'
                }}>
                  <span style={{ marginRight: '5px', opacity: 0.8 }}>Owned:</span>
                  {userHoldings.find(h => h.stockSymbol.toUpperCase() === selectedStock.symbol.toUpperCase()).quantity} shares
                </div>
              )}
            </div>
            <div className="stock-price-label">Live Market Price</div>
          </div>

          <div className="order-panel">
            <h3 className="order-title">New Order</h3>
            <div className="order-form">
              <label>Quantity:</label>
              <input
                type="number"
                className="quantity-input"
                min="1"
                value={quantity}
                onChange={(e) => setQuantity(parseInt(e.target.value) || 1)}
              />
            </div>

            <div className="order-buttons">
              <button type="button" className="btn-sell" onClick={onSell}>&#8595; Sell</button>
              <button type="button" className="btn-buy" onClick={onBuy}>&#8593; Buy</button>
            </div>

            {tradeMessage && (
              <div className="trade-message" style={{
                marginTop: '12px',
                padding: '8px 12px',
                borderRadius: '8px',
                fontSize: '0.85rem',
                fontWeight: 600,
                background: tradeMessage.startsWith('Bought') || tradeMessage.startsWith('Sold')
                  ? 'rgba(40, 167, 69, 0.15)'
                  : 'rgba(220, 53, 69, 0.15)',
                color: tradeMessage.startsWith('Bought') || tradeMessage.startsWith('Sold')
                  ? 'var(--primary)'
                  : '#dc3545'
              }}>
                {tradeMessage}
              </div>
            )}
          </div>

          <div className="graph-panel">
            <div className="timeframe-buttons">
              <button
                className={`btn-timeframe ${timeframe === 'day' ? 'active' : ''}`}
                onClick={() => handleTimeframeChange('day')}
              >
                Day
              </button>
              <button
                className={`btn-timeframe ${timeframe === 'month' ? 'active' : ''}`}
                onClick={() => handleTimeframeChange('month')}
              >
                Month
              </button>
              <button
                className={`btn-timeframe ${timeframe === 'year' ? 'active' : ''}`}
                onClick={() => handleTimeframeChange('year')}
              >
                Year
              </button>
            </div>
            <div style={{ padding: '0 20px', width: '100%', boxSizing: 'border-box' }}>
              <PriceChart
                symbol={selectedStock.symbol}
                price={selectedStock.price}
                timeframe={timeframe}
                stockChartData={stockChartData}
              />
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Trade;
