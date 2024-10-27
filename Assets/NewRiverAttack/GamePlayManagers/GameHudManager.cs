using System;
using ImmersiveGames.DebugManagers;
using NewRiverAttack.GameStatisticsSystem;
using UnityEngine;

namespace NewRiverAttack.GamePlayManagers
{
    public class GameHudManager : MonoBehaviour
    {
        public static GameHudManager Instance { get; private set; }
        
        public event Action<float,int> EventHudRapidFireUpdate;
        public event Action<float,int> EventHudRapidFireEnd;
 
        public event Action<int,int> EventHudScoreUpdate;
        public event Action<int,int> EventHudDistanceUpdate;
        public event Action<int,int> EventHudLivesUpdate;
        public event Action<int,int> EventHudBombUpdate;
        public event Action<int,int> EventHudRefugiesUpdate;
        
        #region Unity Methods
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;

                DebugManager.Log<GameHudManager>("GameHudManager instanciado.");
            }
            else
            {
                Destroy(gameObject);
                DebugManager.LogWarning<GameHudManager>(
                    "Tentativa de criar uma segunda instância de GameHudManager foi evitada.");
            }
        }
        #endregion
        

        #region Calls
        internal void OnEventHudScoreUpdate(int valueUpdate, int playerIndex )
        {
            EventHudScoreUpdate?.Invoke(valueUpdate, playerIndex);
            GameStatisticManager.instance.LogMaxScore(valueUpdate);
        }
        internal void OnEventHudDistanceUpdate(int valueUpdate, int playerIndex )
        {
            // Converte a distância total acumulada em um valor inteiro com base na conversão
            EventHudDistanceUpdate?.Invoke(valueUpdate, playerIndex);
            GameStatisticManager.instance.LogDistance(valueUpdate);
        }
        internal void OnEventHudLivesUpdate(int valueUpdate, int playerIndex)
        {
            EventHudLivesUpdate?.Invoke(valueUpdate, playerIndex);
        }
        internal void OnEventHudRefugiesUpdate(int valueUpdate, int playerIndex)
        {
            EventHudRefugiesUpdate?.Invoke(valueUpdate, playerIndex);
        }
        internal void OnEventHudBombUpdate(int valueUpdate, int playerIndex)
        {
            EventHudBombUpdate?.Invoke(valueUpdate, playerIndex);
        }
        internal void OnEventHudRapidFireUpdate(float valueUpdate, int playerIndex)
        {
            EventHudRapidFireUpdate?.Invoke(valueUpdate, playerIndex);
        }
        internal void OnEventHudRapidFireEnd(float valueUpdate, int playerIndex)
        {
            EventHudRapidFireEnd?.Invoke(valueUpdate, playerIndex);
        }        

        #endregion
    }
}