using System.Collections.Generic;
using RiverAttack;
using UnityEngine;
namespace Utils
{
    [CreateAssetMenu(fileName = "ListLevels", menuName = "RiverAttack/Lists/Levels", order = 1)]
    public class ListLevels : ScriptableObject
    {
        
#if UNITY_EDITOR
        [Multiline]
        public string developerDescription = "";
#endif
        public Vector3 offset = new Vector3();
        public Sprite levelIcon;
        public List<Levels> value = new List<Levels>();

        public void SetValue(List<Levels> newValue)
        {
            this.value = newValue;
        }

        public void Add(Levels items)
        {
            value.Add(items);
        }

        public void Remove(Levels items)
        {
            value.Remove(items);
        }

        public Levels Index(int index)
        {
            return value.Count - 1 < 0 ? null : value[index];
        }

        public int count
        {
            get { return value.Count; }
        }
    }
}
