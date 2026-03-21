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

function PriceChart({ symbol, price, timeframe, stockChartData }) {
  const chartData = useMemo(() => {
    // Use real stock data if available
    if (stockChartData && stockChartData.c && stockChartData.t && stockChartData.c.length > 0) {
      const labels = stockChartData.t.map((ts) => {
        const date = new Date(ts * 1000);
        if (timeframe === 'day') {
          return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' });
        } else if (timeframe === 'month') {
          return date.toLocaleDateString([], { month: 'short', day: 'numeric' });
        } else {
          return date.toLocaleDateString([], { month: 'short', year: 'numeric' });
        }
      });

      return {
        labels,
        datasets: [
          {
            label: `${symbol} Price`,
            data: stockChartData.c,
            borderColor: '#28a745',
            backgroundColor: 'rgba(40, 167, 69, 0.1)',
            borderWidth: 2,
            fill: true,
            pointRadius: timeframe === 'year' ? 1 : 2,
            tension: 0.1,
          },
        ],
      };
    }

    // Fallback: simulated data
    if (!price || price <= 0) return null;

    const labels = [];
    const prices = [];
    const now = new Date();
    
    let pointsCount = 20;
    let volatility = price * (timeframe === 'day' ? 0.005 : timeframe === 'month' ? 0.015 : 0.05);

    let simulatedPrice = price;
    for (let i = 0; i < pointsCount; i++) {
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
  }, [symbol, price, timeframe, candleData]);

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
