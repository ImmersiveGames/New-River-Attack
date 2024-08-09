using System;
using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.SteamServicesManagers.Interface;
using Steamworks;
using UnityEngine;

namespace ImmersiveGames.SteamServicesManagers
{
    public class SteamStatsService : MonoBehaviour, IStatsService
    {
        public static SteamStatsService Instance { get; private set; }
        private static bool ConnectedToSteam => SteamConnectionManager.ConnectedToSteam;

        private Dictionary<string, object> _offlineStats = new Dictionary<string, object>();
        private readonly Dictionary<string, float> _thresholdStats = new Dictionary<string, float>();

        #region Unity Methods

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Start()
        {
            LoadOfflineStats();
            SyncStates(); // Tenta sincronizar conquistas ao iniciar
        }

        private void OnApplicationQuit()
        {
            SyncStates(); // Sincroniza conquistas ao sair
        }

        #endregion

        #region IStatsService Interface

        public T GetStat<T>(string statName) where T : struct
        {
            return ConnectedToSteam ? GetStatFromSteam<T>(statName) : GetStatFromOffline<T>(statName);
        }

        public void SetStat<T>(string statName, T statValue) where T : struct
        {
            try
            {
                _offlineStats[statName] = statValue;

                if (!ConnectedToSteam) return;

                if (typeof(T) == typeof(int))
                {
                    var newValue = (int)(object)statValue; // Conversão segura para int
                    if (_thresholdStats.ContainsKey(statName) && _thresholdStats[statName] != 0)
                    {
                        newValue += (int)_thresholdStats[statName];
                        _thresholdStats[statName] = 0;
                    }
                    SteamUserStats.SetStat(statName, newValue);
                }
                else if (typeof(T) == typeof(float))
                {
                    var newValue = (float)(object)statValue; // Conversão segura para float
                    if (_thresholdStats.ContainsKey(statName) && _thresholdStats[statName] != 0)
                    {
                        newValue += _thresholdStats[statName];
                        _thresholdStats[statName] = 0;
                    }
                    SteamUserStats.SetStat(statName, newValue);
                }
                else
                {
                    DebugManager.LogError<SteamStatsService>("Valor inválido");
                }
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamStatsService>($"Erro ao configurar o state {statName} - Erro: {e}");
            }
        }

        public void AddStat<T>(string statName, T value, float threshold, bool instant = true)
        {
            try
            {
                _thresholdStats.TryAdd(statName, 0);

                if (typeof(T) == typeof(int))
                {
                    _thresholdStats[statName] += Convert.ToInt32(value);
                }
                else if (typeof(T) == typeof(float))
                {
                    _thresholdStats[statName] += Convert.ToSingle(value);
                }

                if (_thresholdStats[statName] >= threshold)
                {
                    _thresholdStats[statName] %= threshold;
                    _offlineStats[statName] = typeof(T) == typeof(int) ? (int)threshold : (object)threshold;

                    if (ConnectedToSteam)
                    {
                        if (typeof(T) == typeof(int))
                        {
                            SteamUserStats.AddStat(statName, (int)threshold);
                        }
                        else if (typeof(T) == typeof(float))
                        {
                            SteamUserStats.AddStat(statName, threshold);
                        }
                    }
                }

                if (instant)
                {
                    SteamUserStats.StoreStats();
                }
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamStatsService>($"Erro ao adicionar valor ao state {statName} - Erro: {e}");
            }
        }

        public void SyncStates()
        {
            SaveOfflineStats();
            if (!SteamConnectionManager.ConnectedToSteam)
                return;

            foreach (var state in _offlineStats.ToList())
            {
                switch (state.Value)
                {
                    case int intValue:
                        Debug.Log($"Chave: {state.Key}, Valor (int): {intValue}");
                        SetStat(state.Key, intValue);
                        break;
                    case float floatValue:
                        Debug.Log($"Chave: {state.Key}, Valor (float): {floatValue}");
                        SetStat(state.Key, floatValue);
                        break;
                    default:
                        Debug.LogWarning($"Chave: {state.Key}, Tipo não suportado.");
                        break;
                }
            }
            _offlineStats.Clear();
            SteamUserStats.StoreStats();
        }

        public void ResetAllStats()
        {
            if (ConnectedToSteam)
            {
                SteamUserStats.ResetAll(false);
                SteamUserStats.StoreStats();
            }

            _offlineStats.Clear();
            SaveOfflineStats();
            DebugManager.Log<SteamAchievementService>("Estados Resetados!");
        }

        #endregion

        #region Auxiliares

        private void UpdateOfflineStatsWithThresholds()
        {
            foreach (var (key, thresholdValue) in _thresholdStats)
            {
                if (!_offlineStats.TryGetValue(key, out var currentValue)) continue;

                switch (currentValue)
                {
                    case int intValue:
                        var newIntValue = intValue + (int)thresholdValue;
                        _offlineStats[key] = newIntValue;
                        break;
                    case float floatValue:
                        var newFloatValue = floatValue + thresholdValue;
                        _offlineStats[key] = newFloatValue;
                        break;
                }
            }
        }

        private void LoadOfflineStats()
        {
            var serializedStats = PlayerPrefs.GetString("OfflineStats", "{}");
            var deserializedStats = JsonUtility.FromJson<SerializableDictionary>(serializedStats);
            _offlineStats = deserializedStats.ToDictionary();
        }

        private void SaveOfflineStats()
        {
            UpdateOfflineStatsWithThresholds();
            _thresholdStats.Clear();

            var serializedStats = JsonUtility.ToJson(new SerializableDictionary(_offlineStats));
            PlayerPrefs.SetString("OfflineStats", serializedStats);
            PlayerPrefs.Save();
        }

        private T GetStatFromSteam<T>(string statName) where T : struct
        {
            if (typeof(T) == typeof(float))
            {
                return (T)(object)SteamUserStats.GetStatFloat(statName);
            }
            if (typeof(T) == typeof(int))
            {
                return (T)(object)SteamUserStats.GetStatInt(statName);
            }

            throw new InvalidOperationException("Tipo não suportado.");
        }

        private T GetStatFromOffline<T>(string statName) where T : struct
        {
            if (_offlineStats.TryGetValue(statName, out var value) && value is T typedValue)
            {
                return typedValue;
            }

            return default;
        }

        #endregion

        #region Serializable Dictionary Class

        [Serializable]
        private class SerializableDictionary
        {
            public List<string> keys;
            public List<string> values;

            public SerializableDictionary(Dictionary<string, object> dictionary)
            {
                keys = dictionary.Keys.ToList();
                values = dictionary.Values.Select(v => v.ToString()).ToList();
            }

            public Dictionary<string, object> ToDictionary()
            {
                var dictionary = new Dictionary<string, object>();
                for (var i = 0; i < keys.Count; i++)
                {
                    if (int.TryParse(values[i], out int intValue))
                    {
                        dictionary[keys[i]] = intValue;
                    }
                    else if (float.TryParse(values[i], out float floatValue))
                    {
                        dictionary[keys[i]] = floatValue;
                    }
                    else
                    {
                        dictionary[keys[i]] = values[i];
                    }
                }

                return dictionary;
            }
        }

        #endregion
    }
}
