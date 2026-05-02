const API_BASE = 'http://localhost:5002/api/v1';
//const API_BASE = '/api/v1';

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

export const fetchPopularStocks = async (token, logout) => {
  const response = await fetch(`${API_BASE}/TradeApi/popular-stocks`, {
    headers: authHeaders(token)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return await response.json();
  return null;
};

export const searchStocks = async (query) => {
  const response = await fetch(`${API_BASE}/TradeApi/search?q=${query}`);
  if (response.ok) return await response.json();
  return null;
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

export const fetchUserOperations = async (token, logout) => {
  const response = await fetch(`${API_BASE}/UserOperationsApi`, {
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

export const registerApi = async (name, email, password, details = {}) => {
  const response = await fetch(`${API_BASE.replace('/v1', '')}/Auth/register`, {
    method: 'POST',
    headers: { 'Content-Type': 'application/json' },
    body: JSON.stringify({ name, email, password, ...details })
  });
  const data = await response.json();
  if (response.ok) return { data };
  return { error: data.message || 'Registration failed' };
};

export const fetchUserDetails = async (token, logout) => {
  const response = await fetch(`${API_BASE}/UserDetailsApi`, {
    headers: authHeaders(token)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return await response.json();
  return null;
};

export const updateUserDetails = async (details, token, logout) => {
  const response = await fetch(`${API_BASE}/UserDetailsApi`, {
    method: 'POST',
    headers: authHeaders(token),
    body: JSON.stringify(details)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return true;
  return null;
};

export const fetchRoles = async () => {
  const response = await fetch(`${API_BASE}/UserRoleApi`);
  if (response.ok) return await response.json();
  return [];
};

export const fetchNews = async () => {
  const response = await fetch(`${API_BASE}/News`);
  if (response.ok) return await response.json();
  return null;
};

// Financial Goals
export const fetchFinancialGoals = async (token, logout) => {
  const response = await fetch(`${API_BASE}/FinancialGoal`, {
    headers: authHeaders(token)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return await response.json();
  return null;
};

export const fetchGoalTypes = async (token, logout) => {
  const response = await fetch(`${API_BASE}/FinancialGoal/types`, {
    headers: authHeaders(token)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return await response.json();
  return null;
};

export const createFinancialGoal = async (goalData, token, logout) => {
  const response = await fetch(`${API_BASE}/FinancialGoal`, {
    method: 'POST',
    headers: authHeaders(token),
    body: JSON.stringify(goalData)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return await response.json();
  return null;
};

export const updateFinancialGoal = async (id, goalData, token, logout) => {
  const response = await fetch(`${API_BASE}/FinancialGoal/${id}`, {
    method: 'PUT',
    headers: authHeaders(token),
    body: JSON.stringify(goalData)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return true;
  return null;
};

export const deleteFinancialGoal = async (id, token, logout) => {
  const response = await fetch(`${API_BASE}/FinancialGoal/${id}`, {
    method: 'DELETE',
    headers: authHeaders(token)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return true;
  return null;
};

export const addGoalContribution = async (id, amount, token, logout) => {
  const response = await fetch(`${API_BASE}/FinancialGoal/${id}/contribution`, {
    method: 'POST',
    headers: authHeaders(token),
    body: JSON.stringify(amount)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return true;
  return null;
};
