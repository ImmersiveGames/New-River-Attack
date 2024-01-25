using System;
using UnityEngine;
using UnityEngine.InputSystem;
namespace RiverAttack
{
    public class GameManagerInput : MonoBehaviour
    {
        void Start()
        {
            InputSystem.onDeviceChange +=
                (device, change) =>
                {
                    switch (change)
                    {
                        case InputDeviceChange.Added:
                            //Debug.Log($"Device {device} was added");
                            break;
                        case InputDeviceChange.Removed:
                            //Debug.Log($"Device {device} was removed");
                            PauseGame(true);
                            break;
                        case InputDeviceChange.Disconnected:
                            //Debug.Log($"A existent Device {device} was disconnected");
                            PauseGame(true);
                            break;
                        case InputDeviceChange.Reconnected:
                            //Debug.Log($"A existent Device {device} was reconnected");
                            break;
                        case InputDeviceChange.Enabled:
                            //Debug.Log($"A existent Device {device} was enabled");
                            break;
                        case InputDeviceChange.Disabled:
                            //Debug.Log($"A existent Device {device} was disabled");
                            PauseGame(true);
                            break;
                        case InputDeviceChange.UsageChanged:
                        case InputDeviceChange.ConfigurationChanged:
                        case InputDeviceChange.SoftReset:
                        case InputDeviceChange.HardReset:
                            break;
                        default:
                            throw new ArgumentOutOfRangeException(nameof(change), change, null);
                    }
                };

        }

        void PauseGame(bool pause)
        {
            GameManager.instance.PauseGame(pause);
        }
    }
}
