using System.Collections.Generic;
using System.Linq;
using ImmersiveGames.DebugManagers;
using ImmersiveGames.InputManager;
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

        private const KeyCode ForwardKey = KeyCode.P;
        private const KeyCode BackwardKey = KeyCode.O;

        #region Delegates

        public delegate void GeneralShoppingEventHandler();

        internal event GeneralShoppingEventHandler EventBuyProduct;

        #endregion
        #region UNity Methods

        private void OnEnable()
        {
            
            _navigationMode = new SmoothInfiniteNavigationMode();
            _shopLayout = new ShopLayoutHorizontal();
            
            _productStocks = stockShopsList.OrderBy(prod => prod.shopProduct.name).ToList();
            _shopLayout.ConfigureLayout(panelContent, stockShopsList.Count, prefabItemShop);
            InputManagerInitializer.ActionManager.ActivateActionMap(GameActionMaps.Shopping);
        }

        private void Start()
        {
            _shopLayout.ConfigureLayout(panelContent, stockShopsList.Count, prefabItemShop);

            foreach (var stock in _productStocks)
            {
                DebugManager.Log($"Stock Quantity: {stock.quantityInStock}, " +
                                 $"Product: {stock.shopProduct.name}, " +
                                 $"Product Type: {stock.productType}");
                InstantiateItemShop(stock);
            }
            //InputManagerInitializer.ActionManager.ActivateActionMap(GameActionMaps.Shopping);
            InputManagerInitializer.RegisterAction("LeftSelection", InputOnMoveLeft );
            InputManagerInitializer.RegisterAction("RightSelection", InputOnMoveRight );
        }

        private void Update()
        {
            if (Input.GetKeyDown(ForwardKey))
            {
                _navigationMode.MoveContent(panelContent, true, this); // Move para frente
            }
            else if (Input.GetKeyDown(BackwardKey))
            {
                _navigationMode.MoveContent(panelContent, false, this); // Move para trás
            }
        }

        private void OnDisable()
        {
            InputManagerInitializer.ActionManager.RestoreActionMap();
            _shopLayout =  null;
            _navigationMode = null;
            _productStocks.Clear();
        }
        #endregion

        #region InputSystem

        private void InputOnMoveLeft(InputAction.CallbackContext context)
        {
            DebugManager.Log($"[Move Left] contexto: {context}");
            ButtonShoppingNavigation(false);
        }
        private void InputOnMoveRight(InputAction.CallbackContext context)
        {
            DebugManager.Log($"[Move Right] contexto: {context}");
            ButtonShoppingNavigation(true);
        }

        #endregion

        public void ButtonShoppingNavigation(bool forward)
        {
            _navigationMode.MoveContent(panelContent, forward, this);
        }
        internal void OnEventBuyProduct()
        {
            EventBuyProduct?.Invoke();
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
                Debug.LogWarning($"Missing ShopProductSettings component on prefab: {prefabItemShop.name}");
            }
        }
    }
}