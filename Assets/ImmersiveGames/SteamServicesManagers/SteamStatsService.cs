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
                //DontDestroyOnLoad(gameObject);
                DebugManager.Log<SteamStatsService>("SteamStatsService instanciado.");
            }
            else
            {
                Destroy(gameObject);
                DebugManager.LogWarning<SteamStatsService>("Tentativa de criar uma segunda instância de SteamStatsService foi evitada.");
            }
        }

        private void Start()
        {
            DebugManager.Log<SteamStatsService>("Carregando stats offline...");
            LoadOfflineStats();
            DebugManager.Log<SteamStatsService>("Sincronizando stats ao iniciar...");
            SyncStates(); // Tenta sincronizar conquistas ao iniciar
        }

        private void OnApplicationQuit()
        {
            DebugManager.Log<SteamStatsService>("Sincronizando stats antes de sair...");
            SyncStates(); // Sincroniza conquistas ao sair
        }

        #endregion

        #region IStatsService Interface

        public T GetStat<T>(string statName) where T : struct
        {
            DebugManager.Log<SteamStatsService>($"Obtendo stat {statName} do tipo {typeof(T).Name}.");
            return ConnectedToSteam ? GetStatFromSteam<T>(statName) : GetStatFromOffline<T>(statName);
        }

        public void SetStat<T>(string statName, T statValue) where T : struct
        {
            DebugManager.Log<SteamStatsService>($"Configurando stat {statName} com o valor {statValue}.");
            try
            {
                _offlineStats[statName] = statValue;

                if (!ConnectedToSteam)
                {
                    DebugManager.LogWarning<SteamStatsService>($"Steam não está conectado. Stat {statName} salvo offline.");
                    return;
                }

                if (typeof(T) == typeof(int))
                {
                    var newValue = (int)(object)statValue;
                    if (_thresholdStats.ContainsKey(statName) && _thresholdStats[statName] != 0)
                    {
                        newValue += (int)_thresholdStats[statName];
                        _thresholdStats[statName] = 0;
                    }
                    SteamUserStats.SetStat(statName, newValue);
                    DebugManager.Log<SteamStatsService>($"Stat {statName} atualizado no Steam com valor {newValue}.");
                }
                else if (typeof(T) == typeof(float))
                {
                    var newValue = (float)(object)statValue;
                    if (_thresholdStats.ContainsKey(statName) && _thresholdStats[statName] != 0)
                    {
                        newValue += _thresholdStats[statName];
                        _thresholdStats[statName] = 0;
                    }
                    SteamUserStats.SetStat(statName, newValue);
                    DebugManager.Log<SteamStatsService>($"Stat {statName} atualizado no Steam com valor {newValue}.");
                }
                else
                {
                    DebugManager.LogError<SteamStatsService>("Tipo de valor inválido.");
                }
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamStatsService>($"Erro ao configurar o stat {statName} - Erro: {e}");
            }
        }

        public void AddStat<T>(string statName, T value, float threshold, bool instant = true)
        {
            DebugManager.Log<SteamStatsService>($"Adicionando valor {value} ao stat {statName} com threshold {threshold}.");
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
                    DebugManager.Log<SteamStatsService>($"Threshold alcançado para o stat {statName}. Sincronizando...");
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
                        DebugManager.Log<SteamStatsService>($"Stat {statName} atualizado no Steam após threshold.");
                    }
                }

                if (instant)
                {
                    SteamUserStats.StoreStats();
                    DebugManager.Log<SteamStatsService>($"Stats armazenados instantaneamente.");
                }
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamStatsService>($"Erro ao adicionar valor ao stat {statName} - Erro: {e}");
            }
        }

        public void SyncStates()
        {
            DebugManager.Log<SteamStatsService>("Sincronizando stats...");
            SaveOfflineStats();

            if (!SteamConnectionManager.ConnectedToSteam)
            {
                DebugManager.LogWarning<SteamStatsService>("Steam não está conectado. Sincronização abortada.");
                return;
            }

            foreach (var state in _offlineStats.ToList())
            {
                switch (state.Value)
                {
                    case int intValue:
                        DebugManager.Log<SteamStatsService>($"Sincronizando stat {state.Key} com valor (int) {intValue}.");
                        SetStat(state.Key, intValue);
                        break;
                    case float floatValue:
                        DebugManager.Log<SteamStatsService>($"Sincronizando stat {state.Key} com valor (float) {floatValue}.");
                        SetStat(state.Key, floatValue);
                        break;
                    default:
                        DebugManager.LogWarning<SteamStatsService>($"Tipo de stat não suportado: {state.Key}.");
                        break;
                }
            }
            _offlineStats.Clear();
            SteamUserStats.StoreStats();
            DebugManager.Log<SteamStatsService>("Sincronização de stats completa.");
        }

        public void ResetAllStats()
        {
            DebugManager.Log<SteamStatsService>("Resetando todos os stats...");
            if (ConnectedToSteam)
            {
                SteamUserStats.ResetAll(false);
                SteamUserStats.StoreStats();
                DebugManager.Log<SteamStatsService>("Todos os stats foram resetados no Steam.");
            }

            _offlineStats.Clear();
            SaveOfflineStats();
            DebugManager.Log<SteamStatsService>("Todos os stats foram resetados localmente.");
        }

        #endregion

        #region Auxiliares

        private void UpdateOfflineStatsWithThresholds()
        {
            DebugManager.Log<SteamStatsService>("Atualizando stats offline com thresholds...");
            foreach (var (key, thresholdValue) in _thresholdStats)
            {
                if (!_offlineStats.TryGetValue(key, out var currentValue)) continue;

                switch (currentValue)
                {
                    case int intValue:
                        var newIntValue = intValue + (int)thresholdValue;
                        _offlineStats[key] = newIntValue;
                        DebugManager.Log<SteamStatsService>($"Stat {key} atualizado offline com novo valor (int) {newIntValue}.");
                        break;
                    case float floatValue:
                        var newFloatValue = floatValue + thresholdValue;
                        _offlineStats[key] = newFloatValue;
                        DebugManager.Log<SteamStatsService>($"Stat {key} atualizado offline com novo valor (float) {newFloatValue}.");
                        break;
                }
            }
        }

        private void LoadOfflineStats()
        {
            DebugManager.Log<SteamStatsService>("Carregando stats offline de PlayerPrefs...");
            var serializedStats = PlayerPrefs.GetString("OfflineStats", "{}");
            var deserializedStats = JsonUtility.FromJson<SerializableDictionary>(serializedStats);
            _offlineStats = deserializedStats.ToDictionary();
            DebugManager.Log<SteamStatsService>("Stats offline carregados.");
        }

        private void SaveOfflineStats()
        {
            DebugManager.Log<SteamStatsService>("Salvando stats offline...");
            UpdateOfflineStatsWithThresholds();
            _thresholdStats.Clear();

            var serializedStats = JsonUtility.ToJson(new SerializableDictionary(_offlineStats));
            PlayerPrefs.SetString("OfflineStats", serializedStats);
            PlayerPrefs.Save();
            DebugManager.Log<SteamStatsService>("Stats offline salvos em PlayerPrefs.");
        }

        private T GetStatFromSteam<T>(string statName) where T : struct
        {
            DebugManager.Log<SteamStatsService>($"Obtendo stat {statName} do Steam...");
            if (typeof(T) == typeof(float))
            {
                var value = SteamUserStats.GetStatFloat(statName);
                DebugManager.Log<SteamStatsService>($"Stat {statName} (float) obtido do Steam: {value}");
                return (T)(object)value;
            }
            if (typeof(T) == typeof(int))
            {
                var value = SteamUserStats.GetStatInt(statName);
                DebugManager.Log<SteamStatsService>($"Stat {statName} (int) obtido do Steam: {value}");
                return (T)(object)value;
            }

            throw new InvalidOperationException("Tipo não suportado.");
        }

        private T GetStatFromOffline<T>(string statName) where T : struct
        {
            DebugManager.Log<SteamStatsService>($"Tentando obter stat {statName} offline...");
            if (_offlineStats.TryGetValue(statName, out var value) && value is T typedValue)
            {
                DebugManager.Log<SteamStatsService>($"Stat {statName} obtido offline: {typedValue}");
                return typedValue;
            }
            DebugManager.Log<SteamStatsService>($"Stat {statName} não encontrado offline. Retornando valor padrão.");
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
