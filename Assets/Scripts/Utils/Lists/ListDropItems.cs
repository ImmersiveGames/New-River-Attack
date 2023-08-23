using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GD.MinMaxSlider;
using UnityEngine;
namespace Utils
{
    [CreateAssetMenu(fileName = "ListItemDrop", menuName = "RiverAttack/Lists/DropItems", order = 1)]
    public class ListDropItems : ScriptableObject
    {

#if UNITY_EDITOR
        [Multiline]
        public string developerDescription = "";
#endif
        public List<ItemsDrop> value = new List<ItemsDrop>();

        public void SetValue(List<ItemsDrop> newValue)
        {
            this.value = newValue;
        }

        public void Add(ItemsDrop items)
        {
            value.Add(items);
        }

        public void Remove(ItemsDrop items)
        {
            value.Remove(items);
        }

        public int count
        {
            get { return value.Count; }
        }

        public ItemsDrop TakeRandomItem(float range)
        {
            //Debug.Log("ItemsDrop: OK");
            int powerUpListCount = value.Count;
            float realNum = 0;
            switch (powerUpListCount)
            {
                case <= 0:
                    return new ItemsDrop();
                //Debug.Log("ItemsDrop: "+ n);
                case 1:
                    return value[0];
            }
            float totals = value.Sum(x => x.sortChances);
            value.Sort((a, b) => b.sortChances.CompareTo(a.sortChances));
            //var orderByDescending = value.OrderByDescending(x => x.realChance);
            foreach (var dropItem in value)
            {
                dropItem.SetRealChance(totals);
                realNum += dropItem.realSort;
                //Debug.Log("Real Chance: " + Value[i].realsort);
                if (realNum >= range) return dropItem;
            }
            //Debug.Log("Real Numero : " + realnum);
            return new ItemsDrop();
        }
    }

    [System.Serializable]
    public class ItemsDrop
    {
        [SerializeField]
        public GameObject item;
        [SerializeField, MinMaxSlider(0, 1)]
        private Vector2 sortChance;
        [SerializeField, Range(1, 100)]
        public int itemQuantity;
        [HideInInspector]
        public float realChance;

        public float sortChances
        {
            get
            {
                realChance = (sortChance.x != 0) ? Random.Range(sortChance.x, sortChance.y) : sortChance.y;
                return realChance;
            }
        }

        //public float realChance { get; private set; }
        public float realSort { get; set; }

        public void SetRealChance(float totalWeight)
        {
            realSort = realChance / totalWeight;
        }
    }
}
