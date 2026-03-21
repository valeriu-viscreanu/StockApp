import React, { useMemo } from 'react';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Filler,
  Legend,
} from 'chart.js';
import { Line } from 'react-chartjs-2';

ChartJS.register(
  CategoryScale,
  LinearScale,
  PointElement,
  LineElement,
  Title,
  Tooltip,
  Filler,
  Legend
);

function PriceChart({ symbol, price, timeframe }) {
  const chartData = useMemo(() => {
    if (!price || price <= 0) return null;

    const labels = [];
    const prices = [];
    const now = new Date();
    
    let pointsCount = timeframe === 'day' ? 24 : timeframe === 'month' ? 30 : 52;
    let volatility = price * (timeframe === 'day' ? 0.005 : timeframe === 'month' ? 0.015 : 0.05);

    let simulatedPrice = price;
    for (let i = pointsCount - 1; i >= 0; i--) {
        const date = new Date(now);
        if (timeframe === 'day') {
            date.setHours(now.getHours() - i);
            labels.unshift(date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }));
        } else if (timeframe === 'month') {
            date.setDate(now.getDate() - i);
            labels.unshift(date.toLocaleDateString([], { month: 'short', day: 'numeric' }));
        } else {
            date.setDate(now.getDate() - (i * 7));
            labels.unshift(date.toLocaleDateString([], { month: 'short', year: 'numeric' }));
        }
        
        prices.unshift(simulatedPrice);
        simulatedPrice = Math.max(0.01, simulatedPrice - (Math.random() - 0.5) * volatility);
    }

    return {
      labels,
      datasets: [
        {
          label: `${symbol} Price`,
          data: prices,
          borderColor: '#28a745',
          backgroundColor: 'rgba(40, 167, 69, 0.1)',
          borderWidth: 2,
          fill: true,
          pointRadius: timeframe === 'year' ? 1 : 2,
          tension: 0.1,
        },
      ],
    };
  }, [symbol, price, timeframe]);

  const options = {
    responsive: true,
    maintainAspectRatio: false,
    plugins: {
      legend: { display: false },
      tooltip: {
        callbacks: {
          label: (context) => `$${context.parsed.y.toFixed(2)}`,
        },
      },
    },
    scales: {
      x: {
        grid: { display: false },
        ticks: { maxTicksLimit: 7 },
      },
      y: {
        ticks: {
          callback: (value) => `$${value}`,
        },
      },
    },
  };

  if (!chartData) {
    return (
      <div className="chart-loading" style={{ height: '350px', display: 'flex', alignItems: 'center', justifyContent: 'center', color: 'var(--text-muted)' }}>
        Loading chart data...
      </div>
    );
  }

  return (
    <div style={{ width: '100%', height: '350px' }}>
      <Line data={chartData} options={options} />
    </div>
  );
}

export default PriceChart;
