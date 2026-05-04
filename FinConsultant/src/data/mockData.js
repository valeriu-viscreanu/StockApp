export const clients = [
  { id: 1, name: 'Eleanor Hartmann', avatar: 'EH', risk: 'Conservative', aum: 1_240_000, returns: 8.4, status: 'active', joined: '2020-03-12', advisor: 'You', sector: 'Real Estate & Bonds', lastContact: '2 days ago' },
  { id: 2, name: 'Marcus Delacroix', avatar: 'MD', risk: 'Aggressive', aum: 3_870_000, returns: 22.1, status: 'active', joined: '2019-06-05', advisor: 'You', sector: 'Tech & Equities', lastContact: '1 day ago' },
  { id: 3, name: 'Sophia Velandia', avatar: 'SV', risk: 'Moderate', aum: 890_000, returns: 11.3, status: 'review', joined: '2021-11-18', advisor: 'You', sector: 'Mixed Portfolio', lastContact: '5 days ago' },
  { id: 4, name: 'James Thornton', avatar: 'JT', risk: 'Conservative', aum: 2_100_000, returns: 6.2, status: 'active', joined: '2018-02-27', advisor: 'You', sector: 'Fixed Income', lastContact: 'Today' },
  { id: 5, name: 'Priya Mehta', avatar: 'PM', risk: 'Moderate', aum: 660_000, returns: 13.7, status: 'onboarding', joined: '2024-01-09', advisor: 'You', sector: 'ESG Funds', lastContact: 'Today' },
  { id: 6, name: 'Lucas Ferreira', avatar: 'LF', risk: 'Aggressive', aum: 5_200_000, returns: 31.5, status: 'active', joined: '2017-08-14', advisor: 'You', sector: 'Private Equity', lastContact: '3 days ago' },
];

export const portfolioHistory = [
  { month: 'Nov', value: 11.2 },
  { month: 'Dec', value: 12.1 },
  { month: 'Jan', value: 11.8 },
  { month: 'Feb', value: 13.4 },
  { month: 'Mar', value: 14.2 },
  { month: 'Apr', value: 13.9 },
  { month: 'May', value: 15.6 },
];

export const aumHistory = [
  { month: 'Nov', aum: 11.4 },
  { month: 'Dec', aum: 12.2 },
  { month: 'Jan', aum: 13.0 },
  { month: 'Feb', aum: 13.8 },
  { month: 'Mar', aum: 14.6 },
  { month: 'Apr', aum: 13.9 },
  { month: 'May', aum: 15.1 },
];

export const allocationData = [
  { name: 'Equities', value: 42, color: '#6366f1' },
  { name: 'Fixed Income', value: 28, color: '#22d3ee' },
  { name: 'Real Estate', value: 15, color: '#f59e0b' },
  { name: 'Private Equity', value: 10, color: '#10b981' },
  { name: 'Cash & Other', value: 5, color: '#94a3b8' },
];

export const recentActivity = [
  { id: 1, client: 'Marcus Delacroix', action: 'Buy Order', detail: '500 shares NVDA @ $132.40', time: '2 hours ago', type: 'buy' },
  { id: 2, client: 'Eleanor Hartmann', action: 'Rebalance', detail: 'Portfolio shifted 5% to bonds', time: '5 hours ago', type: 'rebalance' },
  { id: 3, client: 'Priya Mehta', action: 'Onboarding', detail: 'KYC documents submitted', time: 'Yesterday', type: 'onboard' },
  { id: 4, client: 'Lucas Ferreira', action: 'Sell Order', detail: '1,200 shares TSLA @ $248.10', time: 'Yesterday', type: 'sell' },
  { id: 5, client: 'James Thornton', action: 'Meeting', detail: 'Q2 review scheduled for May 10', time: '2 days ago', type: 'meeting' },
];

export const meetings = [
  { id: 1, client: 'James Thornton', type: 'Q2 Review', date: 'May 10', time: '10:00 AM', mode: 'Video Call' },
  { id: 2, client: 'Sophia Velandia', type: 'Portfolio Review', date: 'May 12', time: '2:00 PM', mode: 'In-Person' },
  { id: 3, client: 'Priya Mehta', type: 'Onboarding Call', date: 'May 13', time: '11:30 AM', mode: 'Video Call' },
  { id: 4, client: 'Eleanor Hartmann', type: 'Annual Review', date: 'May 18', time: '3:00 PM', mode: 'Phone' },
];

export const topHoldings = [
  { symbol: 'AAPL', name: 'Apple Inc.', weight: 8.2, value: 1_122_000, change: +1.4 },
  { symbol: 'MSFT', name: 'Microsoft Corp.', weight: 7.1, value: 971_000, change: +0.9 },
  { symbol: 'NVDA', name: 'NVIDIA Corp.', weight: 5.8, value: 793_000, change: +3.2 },
  { symbol: 'BND', name: 'Vanguard Bond ETF', weight: 9.4, value: 1_285_000, change: -0.1 },
  { symbol: 'VNQ', name: 'Vanguard Real Estate', weight: 4.6, value: 629_000, change: +0.6 },
];
