import React from 'react';
import { useSelector } from 'react-redux';

function Orders() {
  const { buyOrders, sellOrders } = useSelector((state) => state.orders);

  const totalBuyAmount = buyOrders.reduce((sum, o) => sum + o.tradeAmount, 0);
  const totalSellAmount = sellOrders.reduce((sum, o) => sum + o.tradeAmount, 0);

  return (
    <>
      <div className="breadcrumb">
        Stocks &#9654; Orders
      </div>
      <div className="orders-container">
        <div className="orders-column">
          <h3 className="orders-heading">Buy Orders</h3>
          {buyOrders.length === 0 ? (
            <p className="no-orders">No buy orders yet.</p>
          ) : (
            buyOrders.map(order => (
              <div key={order.id || order.buyOrderID} className="order-card">
                <span className={`status-badge ${order.status?.toLowerCase() === 'pending' ? 'status-pending' : 'status-processed'}`}>
                  {order.status || 'Processed'}
                </span>
                <div className="order-stock-name">{order.stockName} ({order.stockSymbol})</div>
                <div className="order-details">
                  <span className="order-quantity">{order.quantity} shares</span> at <span className="order-price">${order.price.toFixed(2)}</span>
                </div>
                <div className="order-trade-amount">
                  Trade Amount: <strong>${order.tradeAmount.toFixed(2)}</strong>
                </div>
                <hr />
                <div className="order-date">{order.date}</div>
              </div>
            ))
          )}
        </div>

        <div className="orders-column">
          <h3 className="orders-heading">Sell Orders</h3>
          {sellOrders.length === 0 ? (
            <p className="no-orders">No sell orders yet.</p>
          ) : (
            sellOrders.map(order => (
              <div key={order.id || order.sellOrderID} className="order-card">
                <span className={`status-badge ${order.status?.toLowerCase() === 'pending' ? 'status-pending' : 'status-processed'}`}>
                  {order.status || 'Processed'}
                </span>
                <div className="order-stock-name">{order.stockName} ({order.stockSymbol})</div>
                <div className="order-details">
                  <span className="order-quantity">{order.quantity} shares</span> at <span className="order-price">${order.price.toFixed(2)}</span>
                </div>
                <div className="order-trade-amount">
                  Trade Amount: <strong>${order.tradeAmount.toFixed(2)}</strong>
                </div>
                <hr />
                <div className="order-date">{order.date}</div>
              </div>
            ))
          )}
        </div>
      </div>
      <div className="orders-total-footer">
        <h3>Total Buy Amounts: ${totalBuyAmount.toLocaleString('en-US', { minimumFractionDigits: 2 })}</h3>
        <h3>Total Sell Amounts: ${totalSellAmount.toLocaleString('en-US', { minimumFractionDigits: 2 })}</h3>
      </div>
      <div style={{ height: '80px' }}></div>
    </>
  );
}

export default Orders;
