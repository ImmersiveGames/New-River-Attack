using System;
using ImmersiveGames.Panels.NotificationManager;
using ImmersiveGames.Utils;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImmersiveGames.InputManager
{
    public class DeviceManager : Singleton<DeviceManager>
    {
        private NotificationManager _notificationManager;
        private string _message;
        
        public delegate void DeviceChangeHandler(InputDevice device, InputDeviceChange change);

        public event DeviceChangeHandler EventOnDeviceAdded;
        public event DeviceChangeHandler EventOnDeviceRemoved;

        private void OnEnable()
        {
            InputSystem.onDeviceChange += OnDeviceChange;
        }

        private void OnDisable()
        {
            InputSystem.onDeviceChange -= OnDeviceChange;
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            switch (change)
            {
                case InputDeviceChange.Added:
                    _message = $"Device {device} was added";
                    Debug.Log(_message);
                    EventOnDeviceAdded?.Invoke(device, change);
                    break;
                case InputDeviceChange.Removed:
                    _message = $"Device {device} was removed";
                    Debug.Log(_message);
                    EventOnDeviceRemoved?.Invoke(device, change);
                    break;
                // Adicione outros casos conforme necessário
                default:
                    _message = $"An unknown error occurred with the {device}";
                    Debug.Log(_message);
                    throw new ArgumentOutOfRangeException(nameof(change), change, null);
            }
            _notificationManager.AddNotification(_message);
        }
    }
}