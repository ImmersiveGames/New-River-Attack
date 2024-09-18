using ImmersiveGames.ShopManagers.Interfaces;
using ImmersiveGames.ShopManagers.ShopProducts;
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
        public Color textNormalColor;  // Cor padrão do texto do botão
        public Color textNoBuyColor = Color.red;  // Cor vermelha para quando o jogador não tem dinheiro suficiente
        public LocalizedString textUnavailable;  // Texto de "Indisponível"
        public LocalizedString textInUse;  // Texto para "Em uso"
        public LocalizedString textNotInUse;  // Texto para "Não está em uso"

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
            
            // Verifica se o jogador já possui o produto no inventário
            var gameOptions = GameOptionsSave.Instance;
            var hasProductInPlayerInventory = gameOptions.listPlayerProductStocks.Exists(p => p.ShopProduct == stockShop?.ShopProduct);

            // Lógica para configurar os botões com base no estoque e na lista do jogador
            if (hasProductInPlayerInventory)
            {
                // Se o jogador já possui o produto, exibe o botão de usar
                buttonUse.gameObject.SetActive(true);
                buttonBuy.gameObject.SetActive(false);  // Não pode comprar se já tem
            }
            else if (stockShop is { QuantityInStock: > 0 })
            {
                // Produto está no estoque, exibe o botão de comprar
                buttonUse.gameObject.SetActive(false);  // Não pode usar se ainda não comprou
                buttonBuy.gameObject.SetActive(true);

                // Configura o botão de comprar para indicar se o jogador tem dinheiro suficiente
                InteractableButtonBuy(stockShop, 1);
            }
            else
            {
                // Produto fora de estoque e jogador não possui o item
                buttonUse.gameObject.SetActive(false);
                buttonBuy.gameObject.SetActive(false);  // Produto indisponível
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

            // Verifica se o jogador tem dinheiro suficiente
            var hasEnoughMoney = stockShop.PlayerHaveMoneyToBuy(gameOptions, quantity);

            // Sempre permite comprar se estiver em estoque, mas altera a cor dependendo do dinheiro
            buttonBuy.interactable = hasEnoughMoney;  // Pode clicar se tiver dinheiro suficiente
            buttonBuy.GetComponentInChildren<TMP_Text>().color = hasEnoughMoney ? textNormalColor : textNoBuyColor;

            buttonBuy.onClick.RemoveAllListeners();
            if (hasEnoughMoney)
            {
                buttonBuy.onClick.AddListener(() => BuyAndUseProduct(0, stockShop, quantity));
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
