const API_BASE = 'http://localhost:5002/api';

export const authHeaders = (token) => ({
  'Content-Type': 'application/json',
  'Authorization': `Bearer ${token}`
});

export const handleApiError = (response, logout) => {
  if (response.status === 401) {
    if (logout) logout();
    return true;
  }
  return false;
};

export const loginApi = async (email, password) => {
  try {
    const response = await fetch(`${API_BASE}/Auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ email, password })
    });
    if (response.ok) return await response.json();
    return null;
  } catch (err) {
    console.error('Login error:', err);
    return null;
  }
};

export const fetchClients = async (token, logout) => {
  // Placeholder for client fetching - using the same API structure
  const response = await fetch(`${API_BASE}/v1/AdvisorApi/clients`, {
    headers: authHeaders(token)
  });
  if (handleApiError(response, logout)) return null;
  if (response.ok) return await response.json();
  return null;
};
