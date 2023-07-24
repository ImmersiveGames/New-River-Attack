using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "EnemiesList", menuName = "RiverAttack/Lists/EnemyLists", order = 4)]
    public class EnemiesList : ScriptableObject
    {

#if UNITY_EDITOR
        [Multiline]
        public string developerDescription = "";
#endif
        public List<EnemiesScriptable> value = new List<EnemiesScriptable>();

        public void SetValue(List<EnemiesScriptable> newValue)
        {
            this.value = newValue;
        }

        public void Add(EnemiesScriptable levels)
        {
            if (!value.Contains(levels))
                value.Add(levels);
        }

        public void Remove(EnemiesScriptable levels)
        {
            if (value.Contains(levels))
                value.Remove(levels);
        }

        public int count
        {
            get { return value.Count; }
        }
        public bool Contains(EnemiesScriptable levels)
        {
            return value.Contains(levels);
        }
    }
}
