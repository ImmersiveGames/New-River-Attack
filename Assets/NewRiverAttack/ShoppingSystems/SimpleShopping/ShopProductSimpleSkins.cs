using ImmersiveGames.DebugManagers;
using ImmersiveGames.ShopManagers.Interfaces;
using ImmersiveGames.ShopManagers.ShopProducts;
using ImmersiveGames.SteamServicesManagers;
using NewRiverAttack.SaveManagers;
using NewRiverAttack.ShoppingSystems.SimpleShopping.Abstracts;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace NewRiverAttack.ShoppingSystems.SimpleShopping
{
    public class ShopProductSimpleSkins : ShopProductSettings
    {
        [Header("Attributes Fields")]
        [SerializeField] private Slider attSpeed;
        [SerializeField] private Slider attAgility;
        [SerializeField] private Slider attMaxFuel;
        [SerializeField] private Slider attFuelCadence;
        [SerializeField] private Slider attFireSpeed;
        
        [Header("Skin Buttons Settings")]
        public Color textNormalColor;
        public Color textNoBuyColor = Color.red;
        public LocalizedString textUnavailable;
        public LocalizedString textInUse;
        public LocalizedString textNotInUse;

        private string _cachedTextUnavailable;
        private string _cachedTextInUse;
        private string _cachedTextNotInUse;

        protected void Awake()
        {
            SimpleShoppingManager = GetComponentInParent<SimpleShoppingManager>();
            CacheLocalizedStrings();
        }

        private void CacheLocalizedStrings()
        {
            _cachedTextUnavailable = textUnavailable.GetLocalizedString();
            _cachedTextInUse = textInUse.GetLocalizedString();
            _cachedTextNotInUse = textNotInUse.GetLocalizedString();
        }

        protected internal override void DisplayStock(IStockShop stockShop)
        {
            base.DisplayStock(stockShop);
            DisplaySlides(stockShop?.ShopProduct);
            
            // Verifica se o jogador já possui esse produto na lista de produtos
            var gameOptions = GameOptionsSave.Instance;
            var hasProductInPlayerInventory = gameOptions.listPlayerProductStocks.Exists(p => p.ShopProduct == stockShop.ShopProduct);

            // Configura o botão de usar ou comprar com base na presença no inventário
            if (hasProductInPlayerInventory)
            {
                buttonUse.gameObject.SetActive(true);
                buttonBuy.gameObject.SetActive(false);
            }
            else if (stockShop is { QuantityInStock: > 0 })
            {
                buttonUse.gameObject.SetActive(false);
                buttonBuy.gameObject.SetActive(true);
            }
            else
            {
                // Produto indisponível se não estiver no estoque e não estiver no inventário
                buttonUse.gameObject.SetActive(false);
                buttonBuy.gameObject.SetActive(false);
                buttonBuy.GetComponentInChildren<TMP_Text>().text = _cachedTextUnavailable;
            }
        }
        

        private void DisplaySlides(IShopProduct shopProduct)
        {
            if (shopProduct is not ShopProductSkin productSkin) return;

            attSpeed.value = productSkin.GetRateSpeed();
            attAgility.value = productSkin.GetRateAgility();
            attMaxFuel.value = productSkin.GetRateMaxFuel();
            attFuelCadence.value = productSkin.GetRateCadenceFuel();
            attFireSpeed.value = productSkin.GetRateShoot();
        }

        protected override void InteractableButtonBuy(IStockShop stockShop, int quantity)
        {
            var gameOptions = GameOptionsSave.Instance;

            // Verifica se o jogador tem dinheiro suficiente e se o item está em estoque
            var canBuy = stockShop.HaveInStock(quantity) && stockShop.PlayerHaveMoneyToBuy(gameOptions, quantity);
            buttonBuy.interactable = canBuy;
            buttonBuy.GetComponentInChildren<TMP_Text>().color = canBuy ? textNormalColor : textNoBuyColor;

            buttonBuy.onClick.RemoveAllListeners();
            if (canBuy)
            {
                buttonBuy.onClick.AddListener(() => BuyAndUseProduct(0, stockShop, quantity));
            }
            else
            {
                buttonBuy.GetComponentInChildren<TMP_Text>().text = _cachedTextUnavailable;
            }
        }

        protected override void InteractableButtonUse(IStockShop stockShop, int quantity)
        {
            var gameOptions = GameOptionsSave.Instance;

            // Verifica se o produto já está em uso pelo jogador
            var isCurrentSkin = gameOptions.SkinIsActualInPlayer(0, stockShop.ShopProduct);

            buttonUse.GetComponentInChildren<TMP_Text>().text = isCurrentSkin ? _cachedTextInUse : _cachedTextNotInUse;
            buttonUse.interactable = true;
            buttonUse.onClick.RemoveAllListeners();
            buttonUse.onClick.AddListener(() => UseProduct(0, stockShop, quantity));
        }

        private void BuyAndUseProduct(int indexPlayer, IStockShop stockShop, int quantity)
        {
            BuyProduct(indexPlayer, stockShop, quantity);
            UseProduct(indexPlayer, stockShop, quantity);
            SimpleShoppingManager.OnEventBuyProduct();
        }

        protected override void BuyProduct(int indexPlayer, IStockShop stockShop, int quantity = 1)
        {
            var gameOptions = GameOptionsSave.Instance;

            var price = stockShop.ShopProduct.GetPrice() * quantity;
            if (gameOptions.wallet < price || !stockShop.HaveInStock(quantity)) return;

            gameOptions.UpdateWallet(-price);
            stockShop.UpdateStock(-quantity);

            if (stockShop.ShopProduct is IShopProductInventory product)
            {
                product.AddPlayerProductList(indexPlayer, stockShop, quantity);
            }
        }

        protected override void UseProduct(int indexPlayer, IStockShop stockShop, int quantity = 1)
        {
            if (stockShop?.ShopProduct is IShopProductUsable itemUse)
            {
                itemUse.Use(indexPlayer, stockShop, quantity);
                SimpleShoppingManager.OnEventUseProduct(stockShop.ShopProduct, quantity);
            }
        }
    }
}
