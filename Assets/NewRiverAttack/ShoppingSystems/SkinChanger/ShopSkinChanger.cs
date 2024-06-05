using ImmersiveGames.DebugManagers;
using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.PlayerManagers.Tags;
using NewRiverAttack.ShoppingSystems.SimpleShopping;
using UnityEngine;

namespace NewRiverAttack.ShoppingSystems.SkinChanger
{
    public class ShopSkinChanger: MonoBehaviour
    {
        private SimpleShoppingManager _simpleShoppingManager;
        private TrailRenderer[] _trailRenderers;

        private void Awake()
        {
            _simpleShoppingManager = FindObjectOfType<SimpleShoppingManager>(true);
            if (!_simpleShoppingManager)
            {
                DebugManager.LogError<ShopProductSimpleSkins>($"[MenuPlayerSkin] Não foi encontrado um 'SimpleShoppingManager' em cena");
            }
        }

        private void OnEnable()
        {
            _simpleShoppingManager.EventUseProduct += ShoppingChangeSkin;
        }

        private void OnDisable()
        {
            _simpleShoppingManager.EventUseProduct -= ShoppingChangeSkin;
        }

        private void ShoppingChangeSkin(ShopProduct shopProduct, int quantity)
        {
            var shopProductSkin = shopProduct as ShopProductSkin;

            var children = GetComponentInChildren<SkinAttach>();
            if (shopProductSkin == null) return;
            if (children == true)
            {
                var siblingIndex = children.transform.GetSiblingIndex();
                DestroyImmediate(transform.GetChild(siblingIndex).gameObject);
            }
            var mySkin = Instantiate(shopProductSkin.prefabSkin, transform);
            mySkin.transform.SetAsFirstSibling();
            TurnOffTrails();
        }


        private void TurnOffTrails()
        {
            _trailRenderers = GetComponentsInChildren<TrailRenderer>();

            foreach(var trail in _trailRenderers)
            {
                trail.enabled = false;
            }
        }
    }
}