using System;
using System.Collections.Generic;
using NewRiverAttack.GameManagers;
using NewRiverAttack.LevelBuilder;
using NewRiverAttack.SaveManagers;
using UnityEngine;

namespace NewRiverAttack.HUBManagers
{
    public sealed class HubGameManager : MonoBehaviour
    {
        public static HubGameManager Instance { get; private set; }
        public event Action EventBuildHub;
        public event Action<int> EventExplodeBridge;
        public event Action<int> EventUpdateIndex;
        public event Action<float> EventCursorMove; // Evento para informar a posição ao cursor
        public event Action<int> EventUpdateHub; // Evento para informar a posição ao cursor

        public List<HubOrderData> CachedHubOrderData { get; private set; } = new List<HubOrderData>();
        public int SaveIndex { get; private set; }
        public bool ActiveHub { get; set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                Debug.LogWarning("Tentativa de criar uma segunda instância de HubGameManager foi evitada.");
            }

            ActiveHub = true;
            SaveIndex = GameOptionsSave.Instance.activeIndexMissionLevel;
        }
        
        public static LevelsStates UpdateLevel(int actualIndex, int hubIndex)
        {
            if (actualIndex == hubIndex)
            {
                return LevelsStates.Actual;
            }
            var maxIndex = GameOptionsSave.Instance.activeIndexMissionLevel;
            return maxIndex > hubIndex ? LevelsStates.Open : LevelsStates.Locked;
        }
        
        public float GetPositionByIndex(int index)
        {

            // Validar se o índice está dentro do intervalo
            if (index >= 0 && index < CachedHubOrderData.Count)
            {
                return CachedHubOrderData[index].position;
            }

            // Retornar um valor padrão ou lançar uma exceção caso o índice seja inválido
            Debug.LogWarning($"Índice inválido: {index}. Cache contém {CachedHubOrderData.Count} itens.");
            return -1f; // Valor padrão indicando que a posição não foi encontrada
        }

        public LevelData GetActualDataSave()
        {
            var tempIndex = GameManager.instance.ActiveIndex >= 0 ? GameManager.instance.ActiveIndex : SaveIndex;
            return GetLevelDataByIndex(tempIndex);
        }
        
        public LevelData GetLevelDataByIndex(int index)
        {

            // Validar se o índice está dentro do intervalo
            if (index >= 0 && index < CachedHubOrderData.Count)
            {
                return CachedHubOrderData[index].levelData;
            }

            // Retornar nulo se o índice for inválido
            Debug.LogWarning($"Índice inválido: {index}. Cache contém {CachedHubOrderData.Count} itens.");
            return null;
        }

        #region Call Events

        public void OnEventBuildHub()
        {
            EventBuildHub?.Invoke();
        }
        /// <summary>
        /// Obtém a posição da função `GetPositionByIndex` e envia ao cursor.
        /// </summary>
        public void OnEventCursorMove(int tempIndex)
        {
            //if (!ActiveHub) return;
            var positionZ = GetPositionByIndex(tempIndex);
            if (!(positionZ >= 0)) return;
            EventUpdateHub?.Invoke(tempIndex);
            EventCursorMove?.Invoke(positionZ); // Envia apenas o valor Z ao cursor
        }
        public void OnEventExplodeBridge(int indexBridge)
        {
            EventExplodeBridge?.Invoke(indexBridge);
        }
        public void OnEventUpdateIndex(int indexUpdate)
        {
            EventUpdateIndex?.Invoke(indexUpdate);
            SaveIndex = GameOptionsSave.Instance.activeIndexMissionLevel = indexUpdate;
        }
        
        #endregion
        
    }
}
