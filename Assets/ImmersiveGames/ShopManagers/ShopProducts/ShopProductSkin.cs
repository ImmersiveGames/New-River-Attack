using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using NewRiverAttack.SaveManagers;
using UnityEngine;

namespace ImmersiveGames.ShopManagers.ShopProducts
{
    [CreateAssetMenu(fileName = "itemSkin", menuName = "ImmersiveGames/Shopping/Skins", order = 203)]
    public sealed class ShopProductSkin : ShopProduct, IShopProductInventory, IShopProductUsable
    {
        [Header("Skin fields")]
        public Sprite spriteLife;
        [SerializeField]
        public GameObject prefabSkin;
        [Header("Controllers Settings")]
        [Range(10f,20f)]
        public float playerSpeed;
        [Range(10f,20f)]
        public float playerAgility;
        [Header("Fuel Settings")]
        [Range(100f,300f)]
        public float maxFuel;
        [Range(1f,10f)]
        public float cadenceFuel;
        [Header("Shooting Settings")]
        [Range(0.1f,5f)] 
        public float cadenceShoot;
        [Range(2f,10f)] 
        public float bulletSpeedMultiply;
        [Range(0,10)] 
        public int bulletDamage;
        [Range(0,10)] 
        public int colliderDamage;
        
        public Sprite GetSpriteLife()
        {
            return spriteLife;
        }
        public void AddPlayerProductList(int indexPlayer, IStockShop stockShop, int quantity)
        {
            GameOptionsSave.instance.AddInventory(stockShop.shopProduct, 1);
        }
        public bool HaveBuyAllProductInList(IEnumerable<ShopProductStock> shopProductList)
        {
            return shopProductList.Select(product => GameOptionsSave.instance.HaveProduct(product.shopProduct)).All(check => check);
        }

        public void Use(int indexPlayer, IStockShop stockShop, int quantity)
        {
            //TODO: Aqui precisa avisar o objeto do player que trocou de skin??
            GameOptionsSave.instance.ChangeSkinToPlayer(indexPlayer,this);
        }
    }
}