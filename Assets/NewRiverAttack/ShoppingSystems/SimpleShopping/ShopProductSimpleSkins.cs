﻿using ImmersiveGames.DebugManagers;
using ImmersiveGames.ShopManagers.Interfaces;
using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.SaveManagers;
using NewRiverAttack.ShoppingSystems.SimpleShopping.Abstracts;
using NewRiverAttack.SteamGameManagers;
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
        protected internal override void DisplayStock(IStockShop stockShop)
        {
            base.DisplayStock(stockShop);
            DisplaySlides(stockShop?.shopProduct);
            SelectSkinButton(stockShop, GameOptionsSave.instance);
        }

        private void DisplaySlides(IShopProduct shopProduct)
        {
            switch (shopProduct)
            {
                case null:
                    return;
                case ShopProductSkin productSkin:
                    attSpeed.value = productSkin.GetRateSpeed();
                    attAgility.value = productSkin.GetRateAgility();
                    attMaxFuel.value = productSkin.GetRateMaxFuel();
                    attFuelCadence.value = productSkin.GetRateCadenceFuel();
                    attFireSpeed.value = productSkin.GetRateShoot();
                    break;
            }
        }

        private void SelectSkinButton(IStockShop stockShop, GameOptionsSave gameOptionsSave)
        {
            //Aqui só serve para skins mas se for ampliar precisa achar outra solução
            var hasSkin = stockShop.PlayerAlreadyHave(gameOptionsSave, stockShop.shopProduct);
            buttonUse.gameObject.SetActive(hasSkin);
            buttonBuy.gameObject.SetActive(!hasSkin);
        }
        protected override void InteractableButtonBuy(IStockShop stockShop, int quantity)
        {
            var saveGameOptions = GameOptionsSave.instance;
            DebugManager.Log<ShopProductSimpleSkins>($"[{stockShop.shopProduct.name}] Tem no stock: {stockShop.HaveInStock(quantity)}, " +
                                                     $"Jogador Pode Comprar: {stockShop.PlayerCanBuy(saveGameOptions,quantity)}");
            buttonBuy.GetComponentInChildren<TMP_Text>().color = textNormalColor;
            buttonBuy.GetComponentInChildren<LayoutElement>().GetComponent<Image>().color = textNormalColor;
            buttonBuy.onClick.RemoveAllListeners();
            if (!stockShop.HaveInStock(quantity))
            {
                buttonBuy.interactable = false;
                buttonBuy.GetComponentInChildren<TMP_Text>().text = textUnavailable.GetLocalizedString();
                return;
            }
            if (!stockShop.PlayerHaveMoneyToBuy(saveGameOptions, quantity))
            {
                buttonBuy.interactable = false;
                buttonBuy.GetComponentInChildren<TMP_Text>().color = textNoBuyColor;
                buttonBuy.GetComponentInChildren<LayoutElement>().GetComponent<Image>().color = textNoBuyColor;
                return;
            }
            buttonBuy.onClick.AddListener(() => BuyProduct(0,stockShop, quantity));
            
            DebugManager.Log<ShopProductSimpleSkins>($"[{stockShop.shopProduct.name}] Tem no stock: {stockShop.HaveInStock(quantity)}, " +
                                                     $"Jogador Pode Comprar: {stockShop.PlayerCanBuy(saveGameOptions,quantity)}");
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
            
            buttonUse.GetComponentInChildren<TMP_Text>().text = textNotInUse.GetLocalizedString();
            DebugManager.Log<ShopProductSimpleSkins>("Já possui e esta selecionado: " + stockShop.shopProduct.name);
            if (saveGameOptions.SkinIsActualInPlayer(0,stockShop.shopProduct) )
            {
                
             //Aqui também é logica exclusiva para skins, para ampliar precisa modificar.
             buttonUse.GetComponentInChildren<TMP_Text>().text = textInUse.GetLocalizedString();
             DebugManager.Log<ShopProductSimpleSkins>("Já possui e esta selecionado: " + stockShop.shopProduct.name);
            }
            // No caso desta loja, não é possível comprar e usar ao mesmo tempo.
            buttonUse.onClick.AddListener(() => UseProduct(0,stockShop, quantity));
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
                product.AddPlayerProductList(indexPlayer,stockShop, quantity);
            }
            if (stockShop.shopProduct is IShopProductUsable itemUse)
            {
                itemUse.Use(indexPlayer,stockShop, quantity);
            }
            DebugManager.LogWarning<ShopProductSimpleSkins>("Achievement: UNLOCKED: Buy a Item.");
            if (SteamGameManager.ConnectedToSteam)
            {
                if (stockShop.shopProduct is ShopProductSkin productSkin)
                {
                    SteamGameManager.UnlockAchievement("ACH_BUY_SKIN");
                    if (productSkin.HaveBuyAllProductInList(SimpleShoppingManager.GetShopList))
                    {
                        SteamGameManager.UnlockAchievement("ACH_BUY_SKIN_ALL");
                    }
                }
            }
            SimpleShoppingManager.OnEventBuyProduct();
            DebugManager.Log<ShopProductSimpleSkins>("Product purchased successfully.");
        }
        protected override void UseProduct(int indexPlayer,IStockShop stockShop, int quantity = 1)
        {
            if (stockShop?.shopProduct is not IShopProductUsable itemUse) return;
            itemUse.Use(indexPlayer,stockShop, quantity);
            SimpleShoppingManager.OnEventUseProduct(stockShop.shopProduct, quantity);
            DebugManager.Log<ShopProductSimpleSkins>("Skin used successfully.");
        }
    }
}