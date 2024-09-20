using System;
using System.Collections.Generic;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using ImmersiveGames.ShopManagers.Layouts;
using ImmersiveGames.ShopManagers.NavigationModes;
using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.ShoppingSystems.SimpleShopping.Abstracts;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace NewRiverAttack.ShoppingSystems.SimpleShopping
{
    public sealed class SimpleShoppingManager : MonoBehaviour
    {
        [SerializeField] private List<ShopProductStock> stockShopsList = new List<ShopProductStock>();
        [SerializeField] private GameObject prefabItemShop;
        [SerializeField] private RectTransform panelContent;

        private List<ShopProductStock> _productStocks;
        private INavigationMode _navigationMode;
        private IShopLayout _shopLayout;

        #region Delegates

        public delegate void GeneralShoppingEventHandler();
        internal event GeneralShoppingEventHandler EventBuyProduct;

        public delegate void ProductShoppingEventHandler(ShopProduct shopProduct, int quantity);
        internal event ProductShoppingEventHandler EventUseProduct;

        #endregion

        #region Unity Methods

        private void Awake()
        {
            _navigationMode = new SmoothFiniteNavigationMode();
            _shopLayout = new ShopLayoutHorizontal();
            ClearShopping(panelContent);
        }

        private void OnEnable()
        {
            InputGameManager.ActionManager.ActivateActionMap(ActionManager.GameActionMaps.Shopping);
            InputGameManager.RegisterAction("LeftSelection", InputOnMoveLeft);
            InputGameManager.RegisterAction("RightSelection", InputOnMoveRight);
            InputGameManager.RegisterAction("UseButton", InputOnUse);
            UpdateStockProducts(panelContent);
        }

        private void Start()
        {
            _shopLayout.ConfigureLayout(panelContent, stockShopsList.Count, prefabItemShop);

            // Garantindo que a lista não está duplicada
            _productStocks = new List<ShopProductStock>(stockShopsList);

            // Ordenando a lista por nome
            _productStocks.Sort((x, y) => string.Compare(x.ShopProduct.name, y.ShopProduct.name, StringComparison.Ordinal));

            CreateShopping(panelContent);
        }

        private void OnDisable()
        {
            InputGameManager.UnregisterAction("LeftSelection", InputOnMoveLeft);
            InputGameManager.UnregisterAction("RightSelection", InputOnMoveRight);
            InputGameManager.UnregisterAction("UseButton", InputOnUse);
            InputGameManager.ActionManager.RestoreActionMap();
        }

        private void OnDestroy()
        {
            _shopLayout = null;
            _navigationMode = null;
            _productStocks.Clear();
        }

        #endregion

        #region InputSystem

        private void InputOnMoveLeft(InputAction.CallbackContext context)
        {
            DebugManager.Log<SimpleShoppingManager>($"[Move Left] contexto: {context}");
            ButtonShoppingNavigation(false);
        }

        private void InputOnMoveRight(InputAction.CallbackContext context)
        {
            DebugManager.Log<SimpleShoppingManager>($"[Move Right] contexto: {context}");
            ButtonShoppingNavigation(true);
        }

        private void InputOnUse(InputAction.CallbackContext context)
        {
            var selectedButton = EventSystem.current.currentSelectedGameObject;
            if (selectedButton == null) return;

            var shopProductSimpleSkins = selectedButton.GetComponentInParent<ShopProductSimpleSkins>();
            if (shopProductSimpleSkins != null)
            {
                if (selectedButton == shopProductSimpleSkins.buttonUse.gameObject)
                {
                    shopProductSimpleSkins.buttonUse.onClick.Invoke();
                }
                else if (selectedButton == shopProductSimpleSkins.buttonBuy.gameObject)
                {
                    shopProductSimpleSkins.buttonBuy.onClick.Invoke();
                }
            }
        }

        public void ButtonShoppingNavigation(bool forward)
        {
            _navigationMode.MoveContent(panelContent, forward, this);
        }

        #endregion

        private void ClearShopping(Transform content)
        {
            var childCount = content.childCount;
            for (var i = childCount - 1; i >= 0; i--)
            {
                Destroy(content.GetChild(i).gameObject);  // Certifique-se de destruir os filhos
            }
        }

        private void CreateShopping(RectTransform content)
        {
            ClearShopping(content);

            foreach (var stock in _productStocks)
            {
                InstantiateItemShop(stock);
            }

            UpdateContentSize(content);
            _navigationMode.UpdateSelectedItem(content, 0);
            _shopLayout.ResetContentPosition(content);
        }

        private void UpdateStockProducts(Transform content)
        {
            if (content == null)
            {
                DebugManager.LogError<SimpleShoppingManager>("Panel content is null.");
                return;
            }

            if (stockShopsList == null || stockShopsList.Count == 0)
            {
                DebugManager.LogError<SimpleShoppingManager>("StockShopsList is null or empty.");
                return;
            }

            // Armazena o índice atual selecionado
            int selectedIndex = _navigationMode.SelectedItemIndex;

            ClearShopping(content);  // Certifique-se de que ClearShopping está funcionando corretamente

            foreach (var stock in stockShopsList)
            {
                if (stock.ShopProduct == null)
                {
                    DebugManager.LogError<SimpleShoppingManager>("Stock or ShopProduct is null.");
                    continue;  // Pula itens nulos para evitar problemas
                }

                InstantiateItemShop(stock);  // Adiciona o item da loja
            }

            // Restaura o índice selecionado anterior
            _navigationMode.UpdateSelectedItem((RectTransform)content, selectedIndex);
            _shopLayout.ResetContentPosition((RectTransform)content);

            // Verifica se o layout existe antes de tentar usá-lo
            var layoutGroup = content.GetComponent<HorizontalLayoutGroup>();
            if (layoutGroup != null)
            {
                _navigationMode.MoveContentToIndex((RectTransform)content, selectedIndex);
            }
            else
            {
                DebugManager.LogError<SimpleShoppingManager>("HorizontalLayoutGroup não encontrado no content.");
            }
        }


        public INavigationMode GetNavigationMode() => _navigationMode;

        private void UpdateContentSize(RectTransform content)
        {
            var layoutGroup = content.GetComponent<HorizontalLayoutGroup>();
            if (layoutGroup == null)
            {
                DebugManager.LogError<SimpleShoppingManager>("HorizontalLayoutGroup não encontrado.");
                return;
            }

            if (content.childCount == 0) return;

            var itemWidth = content.GetChild(0).GetComponent<RectTransform>().rect.width;
            var totalWidth = (itemWidth * content.childCount) + (layoutGroup.spacing * (content.childCount - 1));

            content.sizeDelta = new Vector2(totalWidth, content.sizeDelta.y);
        }

        private void InstantiateItemShop(ShopProductStock stock)
        {
            if (prefabItemShop == null)
            {
                DebugManager.LogError<SimpleShoppingManager>("Prefab item for shop is not assigned.");
                return;
            }

            var item = Instantiate(prefabItemShop, panelContent);  // Instancia o item na loja
            var stockItemTemplate = item.GetComponent<ShopProductSettings>();
            if (stockItemTemplate != null)
            {
                stockItemTemplate.DisplayStock(stock);
            }
            else
            {
                DebugManager.LogWarning<SimpleShoppingManager>($"Missing ShopProductSettings component on prefab: {prefabItemShop.name}");
            }

            // Configura o RectTransform
            var rectTransform = item.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.pivot = new Vector2(0, 0.5f);
                rectTransform.anchorMin = new Vector2(0, 0.5f);
                rectTransform.anchorMax = new Vector2(0, 0.5f);
            }
            else
            {
                DebugManager.LogError<SimpleShoppingManager>("RectTransform not found on instantiated item.");
            }
        }


        public List<ShopProductStock> GetShopList => _productStocks;

        public void OnEventBuyProduct()
        {
            UpdateStockProducts(panelContent);
            EventBuyProduct?.Invoke();
        }

        public void OnEventUseProduct(ShopProduct shopProduct, int quantity)
        {
            EventUseProduct?.Invoke(shopProduct, quantity);
        }
    }
}
