using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Shopping
{
    [CreateAssetMenu(fileName = "ListShopProducts", menuName = "Shopping/List/ListProduct", order = 101)]
    public class ListShopProduct : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string DeveloperDescription = "";
#endif
        public List<ShopProduct> value = new List<ShopProduct>();

        public void SetValue(List<ShopProduct> newValue)
        {
            this.value = newValue;
        }

        public void Add(ShopProduct shopProduct)
        {
            value.Add(shopProduct);
        }

        public void Remove(ShopProduct shopProduct)
        {
            value.Remove(shopProduct);
        }

        public int count
        {
            get { return value.Count; }
        }

        public bool Contains(ShopProduct shopProduct)
        {
            return value.Contains(shopProduct);
        }
    }
}
