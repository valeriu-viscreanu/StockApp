import React, { useState, useEffect } from 'react';
import * as api from '../services/api';
import { useAuth } from '../context/AuthContext';

function Activities() {
  const [activities, setActivities] = useState([]);
  const { token, handleLogout } = useAuth();
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const loadActivities = async () => {
      if (token) {
        setLoading(true);
        const data = await api.fetchUserOperations(token, handleLogout);
        if (data) {
          setActivities(data);
        }
        setLoading(false);
      }
    };
    loadActivities();
  }, [token, handleLogout]);

  if (loading) return <div style={{ padding: '2rem', textAlign: 'center' }}>Loading...</div>;

  return (
    <div style={{ padding: '2rem', maxWidth: '800px', margin: '0 auto' }}>
      <h1>Activities</h1>
      <div style={{ display: 'flex', flexDirection: 'column', gap: '1rem', marginTop: '1rem' }}>
        {activities.map(activity => (
          <div key={activity.userOperationID} style={{
            background: 'var(--bg-card)',
            padding: '1.5rem',
            borderRadius: '12px',
            display: 'flex',
            justifyContent: 'space-between',
            alignItems: 'center',
            boxShadow: 'var(--card-shadow)',
            border: '1px solid var(--border)'
          }}>
            <div style={{ display: 'flex', flexDirection: 'column' }}>
              <div style={{ fontSize: '1.1rem', fontWeight: '500' }}>
                {activity.description || activity.operationType}
              </div>
              <div style={{ fontSize: '0.85rem', opacity: 0.7, marginTop: '0.25rem' }}>
                {new Date(activity.timeStamp).toLocaleString()}
              </div>
            </div>
            <div style={{ 
              fontSize: '1.2rem', 
              fontWeight: 'bold', 
              color: activity.amount > 0 ? '#4caf50' : (activity.amount < 0 ? '#f44336' : 'inherit')
            }}>
              {activity.amount > 0 ? '+' : ''}${activity.amount?.toFixed(2)}
            </div>
          </div>
        ))}
        {activities.length === 0 && (
          <div style={{ textAlign: 'center', opacity: 0.7, padding: '2rem' }}>
            No recent activities found.
          </div>
        )}
      </div>
    </div>
  );
}

export default Activities;
