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
        [Range(1f,5f)]
        public float cadenceFuel;
        [Header("Shooting Settings")]
        [Range(0.1f,2f)] 
        public float cadenceShoot;
        [Range(2f,10f)] 
        public float bulletSpeedMultiply;
        [Range(0,10)] 
        public int bulletDamage;
        [Range(0,10)] 
        public int colliderDamage;
        
        
        public float GetRateSpeed()
        {
            return CalculateRelativeValue(8f, 20f,playerSpeed);
        }
        public float GetRateAgility()
        {
            return CalculateRelativeValue(8f, 20f,playerAgility);
        }
        public float GetRateMaxFuel()
        {
            return CalculateRelativeValue(80f, 300f,maxFuel);
        }
        public float GetRateCadenceFuel()
        {
            return 1 - CalculateRelativeValue(0.8f, 4f,cadenceFuel);
        }
        public float GetRateShoot()
        {
            return 1 - CalculateRelativeValue(0.1f, 1.5f,cadenceShoot);
        }
        private static float CalculateRelativeValue(float min, float max, float current)
        {
            // Garantir que o valor atual está dentro do intervalo [min, max]
            if (current < min)
            {
                current = min;
            }
            else if (current > max)
            {
                current = max;
            }

            // Calcular o valor relativo
            return (current - min) / (max - min);
        }
        
        public Sprite GetSpriteLife()
        {
            return spriteLife;
        }
        public void AddPlayerProductList(int indexPlayer, IStockShop stockShop, int quantity)
        {
            GameOptionsSave.Instance.AddInventory(stockShop.ShopProduct, 1);
        }
        public bool HaveBuyAllProductInList(IEnumerable<ShopProductStock> shopProductList)
        {
            return shopProductList.Select(product => GameOptionsSave.Instance.HaveProduct(product.ShopProduct)).All(check => check);
        }

        public void Use(int indexPlayer, IStockShop stockShop, int quantity)
        {
            //TODO: Aqui precisa avisar o objeto do player que trocou de skin??
            GameOptionsSave.Instance.ChangeSkinToPlayer(indexPlayer,this);
        }
    }
}