using ImmersiveGames.DebugManagers;
using ImmersiveGames.ShopManagers.Interfaces;
using ImmersiveGames.ShopManagers.NavigationModes;
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
            DisplaySlides(stockShop?.shopProduct);
            SelectSkinButton(stockShop, GameOptionsSave.instance);
        }

        private void DisplaySlides(IShopProduct shopProduct)
        {
            if (shopProduct is not ShopProductSkin productSkin) return;
            var previousEnabled = attSpeed.interactable;
            SetSliderInteractable(false);

            attSpeed.value = productSkin.GetRateSpeed();
            attAgility.value = productSkin.GetRateAgility();
            attMaxFuel.value = productSkin.GetRateMaxFuel();
            attFuelCadence.value = productSkin.GetRateCadenceFuel();
            attFireSpeed.value = productSkin.GetRateShoot();

            SetSliderInteractable(previousEnabled);
        }

        private void SetSliderInteractable(bool isEnabled)
        {
            attSpeed.interactable = isEnabled;
            attAgility.interactable = isEnabled;
            attMaxFuel.interactable = isEnabled;
            attFuelCadence.interactable = isEnabled;
            attFireSpeed.interactable = isEnabled;
        }

        private void SelectSkinButton(IStockShop stockShop, GameOptionsSave gameOptionsSave)
        {
            var hasSkin = stockShop.PlayerAlreadyHave(gameOptionsSave, stockShop.shopProduct);
            buttonUse.gameObject.SetActive(hasSkin);
            buttonBuy.gameObject.SetActive(!hasSkin);
        }

        protected override void InteractableButtonBuy(IStockShop stockShop, int quantity)
        {
            var saveGameOptions = GameOptionsSave.instance;
            var canBuy = stockShop.HaveInStock(quantity) && stockShop.PlayerHaveMoneyToBuy(saveGameOptions, quantity);

            buttonBuy.interactable = canBuy;
            buttonBuy.GetComponentInChildren<TMP_Text>().color = canBuy ? textNormalColor : textNoBuyColor;
            buttonBuy.GetComponentInChildren<LayoutElement>().GetComponent<Image>().color = canBuy ? textNormalColor : textNoBuyColor;

            buttonBuy.onClick.RemoveAllListeners();
            if (canBuy)
            {
                buttonBuy.onClick.AddListener(() => BuyAndUseProduct(0, stockShop, quantity));
            }
            else
            {
                buttonBuy.GetComponentInChildren<TMP_Text>().text = _cachedTextUnavailable;
            }
            
            DebugManager.Log<ShopProductSimpleSkins>($"[{stockShop.shopProduct.name}] Can Buy: {canBuy}");
        }

        protected override void InteractableButtonUse(IStockShop stockShop, int quantity)
        {
            buttonUse.onClick.RemoveAllListeners();
            var interactable = stockShop.shopProduct is IShopProductUsable;
            
            DebugManager.Log<ShopProductSimpleSkins>("Use Button Interactable: " + interactable);

            if (!interactable)
            {
                buttonUse.interactable = false;
                return;
            }

            var saveGameOptions = GameOptionsSave.instance;
            var isCurrentSkin = saveGameOptions.SkinIsActualInPlayer(0, stockShop.shopProduct);

            buttonUse.GetComponentInChildren<TMP_Text>().text = isCurrentSkin ? _cachedTextInUse : _cachedTextNotInUse;

            buttonUse.onClick.AddListener(() => UseProduct(0, stockShop, quantity));
        }

        private void BuyAndUseProduct(int indexPlayer, IStockShop stockShop, int quantity)
        {
            if (this == null) return; // Verificação de nulidade

            BuyProduct(indexPlayer, stockShop, quantity);

            if (this == null) return; // Verificação de nulidade após compra

            UseProduct(indexPlayer, stockShop, quantity);

            // Notifica outros sistemas para usar o produto
            SimpleShoppingManager.OnEventBuyProduct();

            // Verificação de nulidade antes de mover o painel
            if (this != null)
            {
                MoveToProductPosition(stockShop);
            }
        }


        protected override void BuyProduct(int indexPlayer, IStockShop stockShop, int quantity = 1)
        {
            var saveGameOptions = GameOptionsSave.instance;
            if (saveGameOptions == null || stockShop?.shopProduct == null)
            {
                DebugManager.LogWarning<ShopProductSimpleSkins>("Unable to buy product. Game options or stock shop is null.");
                return;
            }

            var price = stockShop.shopProduct.GetPrice() * quantity;
            if (saveGameOptions.wallet < price || !stockShop.HaveInStock(quantity) || !stockShop.PlayerCanBuy(saveGameOptions, quantity))
            {
                DebugManager.LogWarning<ShopProductSimpleSkins>("Unable to buy product. Insufficient funds, out of stock, or cannot buy.");
                return;
            }

            saveGameOptions.UpdateWallet(-price);
            stockShop.UpdateStock(-quantity);

            if (stockShop.shopProduct is IShopProductInventory product)
            {
                product.AddPlayerProductList(indexPlayer, stockShop, quantity);
            }
            if (stockShop.shopProduct is IShopProductUsable itemUse)
            {
                itemUse.Use(indexPlayer, stockShop, quantity);
            }
            DebugManager.LogWarning<ShopProductSimpleSkins>("Achievement: UNLOCKED: Buy an Item.");

            if (stockShop.shopProduct is not ShopProductSkin productSkin) return;
            SteamAchievementService.Instance.UnlockAchievement("ACH_BUY_SKIN");
                if (productSkin.HaveBuyAllProductInList(SimpleShoppingManager.GetShopList))
                {
                    SteamAchievementService.Instance.UnlockAchievement("ACH_BUY_SKIN_ALL");
                }
        }

        protected override void UseProduct(int indexPlayer, IStockShop stockShop, int quantity = 1)
        {
            if (stockShop?.shopProduct is not IShopProductUsable itemUse) return;
            itemUse.Use(indexPlayer, stockShop, quantity);
            SimpleShoppingManager.OnEventUseProduct(stockShop.shopProduct, quantity);
            DebugManager.Log<ShopProductSimpleSkins>("Skin used successfully.");
        }

        private void MoveToProductPosition(IStockShop stockShop)
        {
            var layoutGroup = GetComponentInParent<HorizontalLayoutGroup>();
            var content = layoutGroup?.transform as RectTransform;

            if (content == null) return;
            var productIndex = FindProductIndexInLayout(stockShop.shopProduct, content);
            var navigation = GetComponentInParent<SmoothFiniteNavigationMode>();

            // Verificação de nulidade e se o índice é válido
            if (navigation != null && productIndex >= 0 && this != null)
            {
                navigation.MoveContentToIndex(content, productIndex);
            }
        }

        public override IShopProduct GetAssociatedProduct()
        {
            return _stockShop?.shopProduct; // Retorna o produto associado ao estoque
        }
        private int FindProductIndexInLayout(IShopProduct shopProduct, RectTransform content)
        {
            // Percorre todos os filhos do conteúdo para encontrar o índice do produto correspondente
            for (var i = 0; i < content.childCount; i++)
            {
                var child = content.GetChild(i);
                var productSettings = child.GetComponent<ShopProductSettings>();
                if (productSettings != null && productSettings.GetAssociatedProduct().Equals(shopProduct))
                {
                    return i;
                }
            }
            return -1; // Retorna -1 se o produto não for encontrado
        }
    }
}
