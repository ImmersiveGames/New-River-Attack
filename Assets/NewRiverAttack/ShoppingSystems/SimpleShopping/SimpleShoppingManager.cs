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

            _productStocks = new List<ShopProductStock>(stockShopsList);
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
                content.GetChild(i);
            }
        }

        private void CreateShopping(RectTransform content)
        {
            ClearShopping(content);
            UpdateContentSize(content);  // Ajusta o tamanho do content
            _navigationMode.UpdateSelectedItem(content, 0);
            _shopLayout.ResetContentPosition(content);
        }

        private void UpdateStockProducts(Transform content)
        {
            var selectedIndex = _navigationMode.SelectedItemIndex;
            ClearShopping(content);

            foreach (var stock in stockShopsList)
            {
                InstantiateItemShop(stock);
            }

            _navigationMode.UpdateSelectedItem((RectTransform)content, selectedIndex);
            _shopLayout.ResetContentPosition((RectTransform)content);

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
                Debug.LogError("HorizontalLayoutGroup não encontrado.");
                return;
            }

            if (content.childCount == 0) return;

            // Obtém a largura do primeiro item após sua criação
            var itemWidth = content.GetChild(0).GetComponent<RectTransform>().rect.width;

            // Calcula o tamanho total do content com base no número de itens e espaçamento
            var totalWidth = (itemWidth * content.childCount) + (layoutGroup.spacing * (content.childCount - 1));

            content.sizeDelta = new Vector2(totalWidth, content.sizeDelta.y);  // Ajusta o tamanho do content
        }
        private void InstantiateItemShop(ShopProductStock stock)
        {
            var item = Instantiate(prefabItemShop, panelContent);
            var stockItemTemplate = item.GetComponent<ShopProductSettings>();
            if (stockItemTemplate != null)
            {
                stockItemTemplate.DisplayStock(stock);
            }
            else
            {
                DebugManager.LogWarning<SimpleShoppingManager>($"Missing ShopProductSettings component on prefab: {prefabItemShop.name}");
            }

            var rectTransform = item.GetComponent<RectTransform>();
            rectTransform.pivot = new Vector2(0, 0.5f);
            rectTransform.anchorMin = new Vector2(0, 0.5f);
            rectTransform.anchorMax = new Vector2(0, 0.5f);
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
