using ImmersiveGames.SaveManagers;
using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using UnityEngine;

namespace ImmersiveGames.ShopManagers.ShopProducts
{
    [CreateAssetMenu(fileName = "itemSkin", menuName = "ImmersiveGames/Shopping/Skins", order = 203)]
    public class ShopProductSkin : ShopProduct, IShopProductInventory, IShopProductUsable
    {
        [Header("Skin fields")]
        [SerializeField]
        public GameObject prefabSkin;
        public void AddPlayerProductList(int indexPlayer, IStockShop stockShop, int quantity)
        {
            GameOptionsSave.instance.AddInventory(stockShop.shopProduct, 1);
        }

        public void Use(int indexPlayer, IStockShop stockShop, int quantity)
        {
            //TODO: Aqui precisa avisar o objeto do player que trocou de skin??
            GameOptionsSave.instance.SetSkinToPlayer(indexPlayer,this);
        }
    }
}