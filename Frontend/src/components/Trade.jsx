import React from 'react';

function Trade({ popularStocks, selectedStock, setSelectedStock, quantity, setQuantity }) {
  return (
    <div className="advanced-dashboard">
      <div className="sidebar-panel">
        <div className="sidebar-header">
          <h3>Market Watch</h3>
        </div>
        <div className="stock-list">
          {popularStocks.map(stock => (
            <a
              key={stock.symbol}
              href="#"
              className={`stock-item ${selectedStock.symbol === stock.symbol ? 'active' : ''}`}
              onClick={(e) => { e.preventDefault(); setSelectedStock(stock); }}
            >
              <div className="stock-icon">{stock.symbol.substring(0, 1)}</div>
              <div className="stock-details">
                <span className="stock-symbol">{stock.symbol}</span>
                <span className="stock-company">{stock.name}</span>
              </div>
              <div className="stock-mini-chart">
                <span className="stock-mini-price">${stock.price.toFixed(2)}</span>
              </div>
            </a>
          ))}
        </div>
      </div>

      <div className="main-view-panel">
        <div className="trade-container">
          <div className="stock-info">
            <h1 className="stock-name">{selectedStock.name} ({selectedStock.symbol})</h1>
            <div className="stock-price">
              <span className="currency">$</span>
              <span className="price-value">{selectedStock.price.toFixed(2)}</span>
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
              <button type="button" className="btn-sell">&#8595; Sell</button>
              <button type="button" className="btn-buy">&#8593; Buy</button>
            </div>
          </div>

          <div className="graph-panel">
            <div className="timeframe-buttons">
              <button className="btn-timeframe">Day</button>
              <button className="btn-timeframe active">Month</button>
              <button className="btn-timeframe">Year</button>
            </div>
            <div className="chart-placeholder">
              [ Chart Visualization Placeholder ]
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Trade;
