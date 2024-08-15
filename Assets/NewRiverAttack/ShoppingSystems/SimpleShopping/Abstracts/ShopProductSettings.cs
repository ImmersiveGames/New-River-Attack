using ImmersiveGames.DebugManagers;
using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace NewRiverAttack.ShoppingSystems.SimpleShopping.Abstracts
{
    public abstract class ShopProductSettings : MonoBehaviour
    {
        [Header("Stock Fields")]
        [SerializeField] private TMP_Text textStockQuantity;
        [SerializeField] private TMP_Text textStockType;
        [SerializeField] protected Button buttonBuy;
        [SerializeField] protected Button buttonUse;
        
        [Header("Products Fields")]
        [SerializeField] private TMP_Text textProductName;
        [SerializeField] private TMP_Text textProductDescription;
        [SerializeField] private TMP_Text textProductPrice;
        [SerializeField] private Image imageProduct;

        protected SimpleShoppingManager SimpleShoppingManager;
        protected IStockShop _stockShop;

        #region Unity Methods

        private void Awake()
        {
            SimpleShoppingManager = GetComponentInParent<SimpleShoppingManager>();
        }

        private void OnEnable()
        {
            SimpleShoppingManager.EventBuyProduct += UpdateDisplays;
            SimpleShoppingManager.EventUseProduct += UpdateUseDisplays;
        }

        private void OnDisable()
        {
            SimpleShoppingManager.EventBuyProduct -= UpdateDisplays;
            SimpleShoppingManager.EventUseProduct -= UpdateUseDisplays;
        }

        #endregion

        private void UpdateUseDisplays(ShopProduct shopProduct, int quantity)
        {
#if DEBUG
            DebugManager.Log<ShopProductSettings>("Updating Displays...");
#endif
            DisplayStock(_stockShop);
        }

        public void UpdateDisplays()
        {
#if DEBUG
            DebugManager.Log<ShopProductSettings>("Updating Displays...");
#endif
            DisplayStock(_stockShop);
        }

        protected internal virtual void DisplayStock(IStockShop stockShop)
        {
            _stockShop = stockShop;

            textStockQuantity.text = stockShop?.quantityInStock.ToString() ?? "N/A";
            textStockType.text = stockShop?.productType.ToString() ?? "N/A";

            DisplayProduct(stockShop?.shopProduct);

            SettingButtons(stockShop, 1);
        }

        protected virtual void DisplayProduct(IShopProduct shopProduct)
        {
            if (shopProduct == null) return;

            textProductName.text = shopProduct.GetName();
            textProductDescription.text = shopProduct.GetDescription();
            textProductPrice.text = shopProduct.GetPrice().ToString();
            imageProduct.sprite = shopProduct.GetImage();
        }
        public abstract IShopProduct GetAssociatedProduct();

        private void SettingButtons(IStockShop stockShop, int quantity)
        {
            InteractableButtonBuy(stockShop, quantity);
            InteractableButtonUse(stockShop, quantity);
        }

        protected abstract void InteractableButtonBuy(IStockShop stockShop, int quantity);

        protected abstract void InteractableButtonUse(IStockShop stockShop, int quantity);

        protected abstract void BuyProduct(int indexPlayer, IStockShop stockShop, int quantity = 1);

        protected abstract void UseProduct(int indexPlayer, IStockShop stockShop, int quantity = 1);
    }
}
