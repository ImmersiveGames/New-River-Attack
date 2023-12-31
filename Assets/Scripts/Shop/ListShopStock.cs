﻿using System.Collections.Generic;
using UnityEngine;

namespace Shopping
{
    [CreateAssetMenu(fileName = "ListShopStock", menuName = "RiverAttack/Shopping/List/ListShopStock", order = 102)]
    [System.Serializable]
    public class ListShopStock : ScriptableObject
    {
#if UNITY_EDITOR
        [Multiline]
        public string developerDescription = "";
#endif
        public List<ShopProductStock> value = new List<ShopProductStock>();

        public void SetValue(List<ShopProductStock> newValue)
        {
            value = newValue;
        }

        public void Add(ShopProductStock shopStock)
        {
            value.Add(shopStock);
        }

        public void Remove(ShopProductStock shopStock)
        {
            value.Remove(shopStock);
        }

        public void AddToStock(ShopProduct shopProduct, int qnt = 1)
        {

            var inStock = value.Find(x => x.shopProduct == shopProduct);
            if (inStock.shopProduct != null)
            {
                inStock.quantity += qnt;
            }
        }
        public void RemoveFromStock(ShopProduct shopProduct, int qnt = 1)
        {
            var inStock = value.Find(x => x.shopProduct == shopProduct);
            if (inStock.shopProduct != null)
            {
                inStock.quantity -= qnt;
            }
        }

        public int count
        {
            get { return value.Count; }
        }

        public ShopProductStock FindProductInStock(ShopProduct shopProduct)
        {
            return value.Find(x => x.shopProduct == shopProduct);
        }

        public bool Contains(ShopProduct shopProduct)
        {
            var inStock = value.Find(x => x.shopProduct == shopProduct);
            return inStock.shopProduct != null;
        }

        public bool Contains(ShopProductStock shopStock)
        {
            return value.Contains(shopStock);
        }
    }
}
