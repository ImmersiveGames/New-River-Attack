using System.Collections.Generic;
using System.Linq;
using NewRiverAttack.HUBManagers;
using UnityEngine;

namespace NewRiverAttack.LevelBuilder
{
    [CreateAssetMenu(fileName = "NewMission", menuName = "ImmersiveGames/RiverAttack/Mission", order = 302)]
    [System.Serializable]
    public class LevelListData : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string developerDescription = "";
#endif
        public List<LevelData> value = new List<LevelData>();
        
        public void SetValue(List<LevelData> newValue)
        {
            this.value = newValue;
        }

        public void Add(LevelData items)
        {
            value.Add(items);
        }

        public void Remove(LevelData items)
        {
            value.Remove(items);
        }

        public LevelData Index(int index)
        {
            return value.Count - 1 < 0 ? null : value[index];
        }
        
        public int Count => value.Count;

        public List<HubData> GetHubDatas()
        {
            return value.Select(hudData => hudData.hudPath).ToList();
        }
    }
}