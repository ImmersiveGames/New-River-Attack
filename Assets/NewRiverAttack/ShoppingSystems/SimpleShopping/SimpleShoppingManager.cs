using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using ImmersiveGames.ShopManagers.Layouts;
using ImmersiveGames.ShopManagers.NavigationModes;
using ImmersiveGames.ShopManagers.ShopProducts;
using NewRiverAttack.ShoppingSystems.SimpleShopping.Abstracts;
using UnityEngine;
using UnityEngine.InputSystem;

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
            _productStocks = stockShopsList.OrderBy(prod => prod.shopProduct.name).ToList();
            CreateShopping(panelContent, _productStocks);
        }

        private void OnDisable()
        {
            InputGameManager.ActionManager.RestoreActionMap();
            InputGameManager.UnregisterAction("LeftSelection", InputOnMoveLeft);
            InputGameManager.UnregisterAction("RightSelection", InputOnMoveRight);
            InputGameManager.UnregisterAction("UseButton", InputOnUse);
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
            DebugManager.Log<SimpleShoppingManager>($"[Button A] Buy/Select contexto: {context}");
            UpdateStockProducts(panelContent);
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
                var child = content.GetChild(i);
                DestroyImmediate(child.gameObject);
            }
        }

        private void CreateShopping(RectTransform content, List<ShopProductStock> productStocks)
        {
            ClearShopping(content);
            foreach (var stock in productStocks)
            {
                DebugManager.Log<SimpleShoppingManager>($"Stock Quantity: {stock.quantityInStock}, " +
                                                        $"Product: {stock.shopProduct.name}, " +
                                                        $"Product Type: {stock.productType}");
                InstantiateItemShop(stock);
            }

            _navigationMode.UpdateSelectedItem(content, 0);
            _shopLayout.ResetContentPosition(content);
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

        private void UpdateStockProducts(Transform content)
        {
            var childCount = content.childCount;
            if (childCount < 1) return;

            ClearShopping(content);

            foreach (var stock in stockShopsList)
            {
                InstantiateItemShop(stock);
            }

            _navigationMode.UpdateSelectedItem((RectTransform)content, 0);
            _shopLayout.ResetContentPosition((RectTransform)content);
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
