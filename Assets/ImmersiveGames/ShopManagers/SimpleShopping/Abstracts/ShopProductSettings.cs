using ImmersiveGames.SaveManagers;
using ImmersiveGames.ShopManagers.Interfaces;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ImmersiveGames.ShopManagers.SimpleShopping.Abstracts
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

        private SimpleShoppingManager _simpleShoppingManager;
        private IStockShop _stockShop;

        #region Unity Methods

        private void Awake()
        {
            _simpleShoppingManager = GetComponentInParent<SimpleShoppingManager>();
        }

        private void OnEnable()
        {
            _simpleShoppingManager.EventBuyProduct += UpdateDisplays;
        }

        private void OnDisable()
        {
            _simpleShoppingManager.EventBuyProduct -= UpdateDisplays;
        }

        #endregion

        private void UpdateDisplays()
        {
            Debug.Log("Updating Displays...");
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

        private void SettingButtons(IStockShop stockShop, int quantity)
        {
            InteractableButtonBuy(stockShop, quantity);
            InteractableButtonUse(stockShop, quantity);
        }

        protected virtual void InteractableButtonBuy(IStockShop stockShop, int quantity)
        {
            var saveGameOptions = GameOptionsSave.instance;

            buttonBuy.onClick.RemoveAllListeners();
            buttonBuy.onClick.AddListener(() => BuyProduct(stockShop, quantity));

            var interactable = stockShop?.HaveInStock() == true && stockShop.PlayerCanBuy(saveGameOptions);
            buttonBuy.interactable = interactable;
            Debug.Log("Buy Button Interactable: " + interactable);
        }

        protected virtual void InteractableButtonUse(IStockShop stockShop, int quantity)
        {
            buttonUse.onClick.RemoveAllListeners();
            buttonUse.onClick.AddListener(() => UseProduct(stockShop, quantity));

            var interactable = stockShop?.shopProduct is IShopProductUsable;
            buttonUse.interactable = interactable;
            Debug.Log("Use Button Interactable: " + interactable);
        }

        protected virtual void BuyProduct(IStockShop stockShop, int quantity = 1)
        {
            var saveGameOptions = GameOptionsSave.instance;
            if (saveGameOptions == null || stockShop?.shopProduct == null)
            {
                Debug.LogWarning("Unable to buy product. Game options or stock shop is null.");
                return;
            }

            var price = stockShop.shopProduct.GetPrice() * quantity;
            if (saveGameOptions.wallet < price || !stockShop.HaveInStock() || !stockShop.PlayerCanBuy(saveGameOptions))
            {
                Debug.LogWarning("Unable to buy product. Insufficient funds, out of stock, or cannot buy.");
                return;
            }

            saveGameOptions.UpdateWallet(-price);
            stockShop.UpdateStock(-quantity);

            switch (stockShop.shopProduct)
            {
                case IShopProductInventory inventory:
                    inventory.AddPlayerProductList(stockShop, quantity);
                    break;
                case IShopProductUsable itemUse:
                    itemUse.Use(stockShop, quantity);
                    break;
            }

            _simpleShoppingManager.OnEventBuyProduct();
            Debug.Log("Product purchased successfully.");
        }

        protected virtual void UseProduct(IStockShop stockShop, int quantity = 1)
        {
            if (stockShop?.shopProduct is not IShopProductUsable itemUse) return;
            itemUse.Use(stockShop, quantity);
            Debug.Log("Product used successfully.");
        }
    }
}
