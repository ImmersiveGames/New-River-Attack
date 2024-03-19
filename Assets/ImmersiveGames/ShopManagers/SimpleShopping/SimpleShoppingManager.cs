using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
using ImmersiveGames.ShopManagers.Abstracts;
using ImmersiveGames.ShopManagers.Interfaces;
using ImmersiveGames.ShopManagers.Layouts;
using ImmersiveGames.ShopManagers.NavigationModes;
using ImmersiveGames.ShopManagers.ShopProducts;
using ImmersiveGames.ShopManagers.SimpleShopping.Abstracts;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ImmersiveGames.ShopManagers.SimpleShopping
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
        public delegate void ProductShoppingEventHandler(ShopProduct shopProduct, int quantity);

        internal event GeneralShoppingEventHandler EventBuyProduct;
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
            InputManagerInitializer.ActionManager.ActivateActionMap(GameActionMaps.Shopping);
            UpdateStockProducts(panelContent);
        }

        private void Start()
        {
            
            _shopLayout.ConfigureLayout(panelContent, stockShopsList.Count, prefabItemShop);
            
            InputManagerInitializer.RegisterAction("LeftSelection", InputOnMoveLeft );
            InputManagerInitializer.RegisterAction("RightSelection", InputOnMoveRight );
            
            InputManagerInitializer.RegisterAction("UseButton", InputOnUse );
            
            _productStocks = stockShopsList.OrderBy(prod => prod.shopProduct.name).ToList();
            CreateShopping(panelContent, _productStocks);
        }

        private void OnDisable()
        {
            InputManagerInitializer.ActionManager.RestoreActionMap();
        }

        private void OnDestroy()
        {
            _shopLayout =  null;
            _navigationMode = null;
            _productStocks.Clear();
        }

        #endregion
        
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
                Debug.LogWarning($"Missing ShopProductSettings component on prefab: {prefabItemShop.name}");
            }
        }

        #region InputSystem

        private void InputOnMoveLeft(InputAction.CallbackContext context)
        {
            //DebugManager.Log($"[Move Left] contexto: {context}");
            ButtonShoppingNavigation(false);
        }
        private void InputOnMoveRight(InputAction.CallbackContext context)
        {
            //DebugManager.Log($"[Move Right] contexto: {context}");
            ButtonShoppingNavigation(true);
        }
        private void InputOnUse(InputAction.CallbackContext context)
        {
            //DebugManager.Log($"[Button A] Buy/Select contexto: {context}");
            UpdateStockProducts(panelContent);
            //ButtonShoppingNavigation(false);
        }
        
        //Click Direto no botão
        public void ButtonShoppingNavigation(bool forward)
        {
            _navigationMode.MoveContent(panelContent, forward, this);
        }

        #endregion

        private void ClearShopping(Transform content)
        {
            var childCount = content.childCount;

            // Loop através de todos os filhos e destrua-os
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
                /*DebugManager.Log($"Stock Quantity: {stock.quantityInStock}, " +
                                 $"Product: {stock.shopProduct.name}, " +
                                 $"Product Type: {stock.productType}");*/
                InstantiateItemShop(stock);
            }
            _navigationMode.UpdateSelectedItem(content, 0);
        }

        private void UpdateStockProducts(Transform content)
        {
            var childCount = content.childCount;
            if (childCount < 1) return;
            // Loop através de todos os filhos e destrua-os
            for (var i = childCount - 1; i >= 0; i--)
            {
                var child = content.GetChild(i);
                var productSettings = child.GetComponent<ShopProductSettings>();
                productSettings.UpdateDisplays();
            }
        }

        #region Call Methods

        internal void OnEventBuyProduct()
        {
            UpdateStockProducts(panelContent);
            EventBuyProduct?.Invoke();
        }
        internal void OnEventUseProduct(ShopProduct shopProduct, int quantity)
        {
            EventUseProduct?.Invoke(shopProduct,quantity);
        }

        #endregion

        
    }
}