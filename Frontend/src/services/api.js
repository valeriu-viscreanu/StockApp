const API_BASE = 'http://localhost:5002/api/v1';

export const authHeaders = (token) => ({
  'Content-Type': 'application/json',
  'Authorization': `Bearer ${token}`
});

export const handleApiError = (response, logout) => {
  if (response.status === 401) {
    logout();
    return true;
  }
  return false;
};

export const fetchStockPrices = async (symbols, token, logout) => {
  const params = symbols.map(s => `symbols=${s}`).join('&');
  const response = await fetch(`${API_BASE}/TradeApi/quotes?${params}`, {
    headers: authHeaders(token)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return await response.json();
  return null;
};

export const fetchStockQuote = async (symbol, token, logout) => {
  const response = await fetch(`${API_BASE}/TradeApi/quote/${symbol}`, {
    headers: authHeaders(token)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return await response.json();
  return null;
};

export const fetchStockData = async (symbol, timeframe, token, logout) => {
  const response = await fetch(`${API_BASE}/TradeApi/data/${symbol}?timeframe=${timeframe}`, {
    headers: authHeaders(token)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return await response.json();
  return null;
};

export const fetchBalance = async (token, logout) => {
  const response = await fetch(`${API_BASE}/CashApi/balance`, {
    headers: authHeaders(token)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return await response.json();
  return null;
};

export const fetchOrders = async (token, logout) => {
  const response = await fetch(`${API_BASE}/TradeApi/orders`, {
    headers: authHeaders(token)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return await response.json();
  return null;
};

export const createOrder = async (type, orderData, token, logout) => {
  const endpoint = type === 'buy' ? 'buy-order' : 'sell-order';
  const response = await fetch(`${API_BASE}/TradeApi/${endpoint}`, {
    method: 'POST',
    headers: authHeaders(token),
    body: JSON.stringify(orderData)
  });
  if (handleApiError(response, logout)) return { error: 'Unauthorized' };
  const data = await response.json();
  if (response.ok) return { data };
  return { error: data.message || `${type} order failed` };
};

export const handleCashActionApi = async (type, amount, token, logout) => {
  const endpoint = type === 'add' ? 'add-funds' : 'withdraw';
  const response = await fetch(`${API_BASE}/CashApi/${endpoint}`, {
    method: 'POST',
    headers: authHeaders(token),
    body: JSON.stringify(amount)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return await response.json();
  const errorData = await response.json();
  throw new Error(errorData.message || `${type} failed`);
};

export const loginApi = async (email, password) => {
  const response = await fetch(`${API_BASE.replace('/v1', '')}/Auth/login`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ email, password })
  });
  if (response.ok) return await response.json();
  return null;
};
