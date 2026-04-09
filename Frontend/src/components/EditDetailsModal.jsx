import React, { useState, useEffect } from 'react';
import Modal from './Modal';
import * as api from '../services/api';

function EditDetailsModal({ isOpen, onClose, token, logout }) {
  const [form, setForm] = useState({
    street: '',
    streetNumber: '',
    building: '',
    unit: '',
    city: '',
    zipCode: '',
    country: '',
    additionalInfo: '',
    phoneNumber: ''
  });
  const [isLoading, setIsLoading] = useState(false);
  const [error, setError] = useState('');

  useEffect(() => {
    if (isOpen) {
      const loadDetails = async () => {
        setIsLoading(true);
        setError('');
        try {
          const data = await api.fetchUserDetails(token, logout);
          if (data) {
            setForm({
              street: data.street || '',
              streetNumber: data.streetNumber || '',
              building: data.building || '',
              unit: data.unit || '',
              city: data.city || '',
              zipCode: data.zipCode || '',
              country: data.country || '',
              additionalInfo: data.additionalInfo || '',
              phoneNumber: data.phoneNumber || ''
            });
          }
        } catch (err) {
          setError('Failed to load profile details.');
        } finally {
          setIsLoading(false);
        }
      };
      loadDetails();
    }
  }, [isOpen, token, logout]);

  if (!isOpen) return null;

  const handleChange = (e) => {
    const { name, value } = e.target;
    setForm(prev => ({ ...prev, [name]: value }));
  };

  const handleSave = async (e) => {
    e.preventDefault();
    setError('');
    const result = await api.updateUserDetails(form, token, logout);
    if (result === true) {
      onClose();
    } else {
      setError('Failed to save. Please try again.');
    }
  };

  return (
    <Modal onClose={onClose}>
      <div className="edit-details-modal">
        <h2>Edit Profile</h2>
        {isLoading ? (
          <p>Loading...</p>
        ) : (
          <form onSubmit={handleSave}>
            <div className="form-row">
              <div className="form-group">
                <label>Street</label>
                <input
                  type="text"
                  name="street"
                  value={form.street}
                  onChange={handleChange}
                  className="login-input"
                />
              </div>
              <div className="form-group">
                <label>Number</label>
                <input
                  type="text"
                  name="streetNumber"
                  value={form.streetNumber}
                  onChange={handleChange}
                  className="login-input"
                />
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label>Building</label>
                <input
                  type="text"
                  name="building"
                  value={form.building}
                  onChange={handleChange}
                  className="login-input"
                />
              </div>
              <div className="form-group">
                <label>Unit/Apt</label>
                <input
                  type="text"
                  name="unit"
                  value={form.unit}
                  onChange={handleChange}
                  className="login-input"
                />
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label>City</label>
                <input
                  type="text"
                  name="city"
                  value={form.city}
                  onChange={handleChange}
                  className="login-input"
                />
              </div>
              <div className="form-group">
                <label>Zip Code</label>
                <input
                  type="text"
                  name="zipCode"
                  value={form.zipCode}
                  onChange={handleChange}
                  className="login-input"
                />
              </div>
            </div>

            <div className="form-row">
              <div className="form-group">
                <label>Country</label>
                <input
                  type="text"
                  name="country"
                  value={form.country}
                  onChange={handleChange}
                  className="login-input"
                />
              </div>
              <div className="form-group">
                <label>Phone Number</label>
                <input
                  type="text"
                  name="phoneNumber"
                  value={form.phoneNumber}
                  onChange={handleChange}
                  className="login-input"
                />
              </div>
            </div>

            <div className="form-group">
              <label>Additional Info</label>
              <input
                type="text"
                name="additionalInfo"
                value={form.additionalInfo}
                onChange={handleChange}
                className="login-input"
              />
            </div>

            {error && <p className="login-error">{error}</p>}
            
            <div className="form-row">
              <button type="button" onClick={onClose} className="login-button secondary-button">
                Cancel
              </button>
              <button type="submit" className="login-button">
                Save
              </button>
            </div>
          </form>
        )}
      </div>
    </Modal>
  );
}

export default EditDetailsModal;
