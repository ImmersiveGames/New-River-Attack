using System;
using ImmersiveGames.DebugManagers;
using Steamworks;
using UnityEngine;

namespace ImmersiveGames.SteamServicesManagers
{
    public class SteamConnectionManager : MonoBehaviour
    {
        private static SteamConnectionManager Instance { get; set; }
        public static bool ConnectedToSteam { get; private set; }
        private static string UserName { get; set; }

        private const uint SteamID = 2777110;

        public static event Action EventOnSteamConnected;
        public static event Action EventOnSteamDisconnected;

        #region Unity Methods

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                //DontDestroyOnLoad(gameObject); // Mantém o objeto persistente entre as cenas
                InitializeSteam();
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            if (!ConnectedToSteam) return;
            try
            {
                SteamClient.RunCallbacks();
            }
            catch (Exception e)
            {
                DebugManager.LogError<SteamConnectionManager>($"Erro ao executar callbacks da Steam: {e.Message}");
                ConnectedToSteam = false;
                EventOnSteamDisconnected?.Invoke(); // Notifica sobre a desconexão
            }
        }

        private void OnApplicationQuit()
        {
            ShutdownSteam();
        }

        private void OnDestroy()
        {
            ShutdownSteam();
        }

        #endregion

        private static void InitializeSteam()
        {
            try
            {
                SteamClient.Init(SteamID);

                if (!SteamClient.IsValid || !SteamClient.IsLoggedOn)
                {
                    DebugManager.LogError<SteamConnectionManager>("Falha na conexão com a Steam ou usuário não logado.");
                    ConnectedToSteam = false;
                    return;
                }

                ConnectedToSteam = true;
                UserName = SteamClient.Name;
                DebugManager.Log<SteamConnectionManager>($"Conexão com a Steam foi estabelecida. Usuário logado: {UserName}");
                EventOnSteamConnected?.Invoke(); // Notifica sobre a conexão
            }
            catch (Exception e)
            {
                ConnectedToSteam = false;
                DebugManager.LogError<SteamConnectionManager>($"Erro ao inicializar a Steam: {e.Message}");
            }
        }

        private static void ShutdownSteam()
        {
            if (!ConnectedToSteam) return;
            SteamClient.Shutdown();
            ConnectedToSteam = false;
            DebugManager.Log<SteamConnectionManager>("Conexão com a Steam foi finalizada.");
            EventOnSteamDisconnected?.Invoke();
        }
    }
}
