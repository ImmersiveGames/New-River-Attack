using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Threading.Tasks;
using RiverAttack;
using Utils;
using TMPro;
using System.Collections.Generic;

namespace Shopping
{
    public class ShopMaster : Singleton<ShopMaster>
    {
        [SerializeField]
        Transform contentShop;
        [SerializeField]
        ScrollRect scrollBarShop;
        [SerializeField]
        GameObject objProduct;
        [SerializeField]
        RectTransform refCenter;
        [SerializeField]
        ListShopStock productStock;
        [SerializeField]
        GameObject productForward, productBackward;
        [SerializeField]
        TMP_Text refuggiesText;

        [Header("Carousel"), SerializeField]
        bool infinityLooping;
        [SerializeField]
        float spaceBetweenPanels = 0, maxPosition = 0;
        [SerializeField]
        Color selectedColor;
        [SerializeField]
        Color normalColor;
 
        PlayersInputActions m_inputSystem;
        PlayerSettings m_PlayerSettings;
        EventSystem m_EventSystem;
        ShopCarousel m_Shop;
        Task m_Task;
        int lastSelectedSkin = 0;


        List<PlayerMaster> playerMasterList = new List<PlayerMaster>();

        public delegate void GeneralUpdateButtons(PlayerSettings player, ShopProductStock item);
        public GeneralUpdateButtons eventButtonSelect;
        public GeneralUpdateButtons eventButtonBuy;

        #region UNITY METHODS
        void OnEnable()
        {
            //SaveGame.DeleteAll();
            SetInitialReferences();
            SetupShop();
            scrollBarShop.horizontalScrollbar.numberOfSteps = m_Shop.getProducts.Length;
        }

        void Start()
        {
            RefuggieDisplayUpdate();

            m_inputSystem.UI_Controlls.BuyButton.performed += ctx => BuyButton(ctx);
            m_inputSystem.UI_Controlls.SelectButton.performed += ctx => SelectButton(ctx);
            m_inputSystem.UI_Controlls.LeftSelection.performed += ctx => ButtonNavegation(-1);
            m_inputSystem.UI_Controlls.RightSelection.performed += ctx => ButtonNavegation(1);
        }

        void LateUpdate()
        {
            ButtonNavegation(0);
            //m_Shop?.Update();
        }
        void OnDisable()
        {
            //GameManagerSaves.Instance.SavePlayer(activePlayer);
            m_inputSystem = new PlayersInputActions();
            m_inputSystem.UI_Controlls.Disable();
        }
  #endregion
        void SetInitialReferences()
        {
            var playerMaster = GameManager.instance.playerObjectAvailableList[0].GetComponent<PlayerMaster>();
            playerMasterList.Add(playerMaster);
            m_PlayerSettings = playerMaster.GetPlayersSettings();
            
            
            m_EventSystem = EventSystem.current;

            m_inputSystem = new PlayersInputActions();
            m_inputSystem.UI_Controlls.Enable();                  

            //GameManagerSaves.Instance.LoadPlayer(ref activePlayer);
            //activePlayer.LoadValues();
        }
        void SetupShop()
        {
            m_Shop = new ShopCarousel(contentShop, objProduct, refCenter)
            {
                spaceBetweenPanels = spaceBetweenPanels,
                infinityLooping = infinityLooping,
                maxPosition = maxPosition
            };
            if (m_Shop == null) return;
            m_Shop.CreateShopping(productStock.value);
            foreach (var t in m_Shop.getProducts)
            {
                var item = t.GetComponent<UIItemShop>();
                if (!item) continue;
                item.SetupButtons(m_PlayerSettings);
                item.getBuyButton.onClick.AddListener(delegate { BuyThisItem(m_PlayerSettings, item.productInStock); });
                item.getSelectButton.onClick.AddListener(delegate { SelectThisItem(m_PlayerSettings, item.productInStock); });
            }
        }

        void BuyThisItem(PlayerSettings player, ShopProductStock product)
        {
            player.listProducts.Add(product.shopProduct);
            player.wealth += -product.shopProduct.priceItem;
            product.shopProduct.ConsumeProduct(player);
            product.RemoveStock(1);
            CallEventButtonBuy(player, product);
            RefuggieDisplayUpdate();

            var item = m_Shop.getProducts[m_Shop.getActualProduct].GetComponent<UIItemShop>();
            item.getSelectButton.interactable = true;
        }
        void SelectThisItem(PlayerSettings player, ShopProductStock shopProductStock)
        {
            shopProductStock.shopProduct.ConsumeProduct(player);
            CallEventButtonSelect(player, shopProductStock);

            foreach (PlayerMaster playerMaster in playerMasterList) 
            {
                playerMaster.CallEventChangeSkin(player.playerSkin);
            }

            var lastSelectBTN = m_Shop.getProducts[lastSelectedSkin].GetComponent<UIItemShop>().getSelectButton;
            lastSelectBTN.GetComponent<Image>().color = normalColor;

            var selectBTN = m_Shop.getProducts[m_Shop.getActualProduct].GetComponent<UIItemShop>().getSelectButton;
            selectBTN.GetComponent<Image>().color = selectedColor;

            lastSelectedSkin = m_Shop.getActualProduct;
        }

        public void ButtonNavegation(int next)
        {
            m_Shop.ButtonNavegation(next);

            //Debug.Log(m_Shop.getActualProduct);
            //Debug.Log(m_Shop.getProducts.Length);

            float scrollbarValue = ((float)m_Shop.getActualProduct) / ((float)m_Shop.getProducts.Length -1);
            //Debug.Log(scrollbarValue);

            scrollBarShop.horizontalScrollbar.value = scrollbarValue;

            if (!infinityLooping)
            {
                if (m_Shop.getActualProduct == 0)
                {
                    productBackward.SetActive(false);
                    productForward.SetActive(true);
                    //m_EventSystem.SetSelectedGameObject(productForward);
                }
                else
                {
                    productBackward.SetActive(true);
                    productForward.SetActive(true);
                }
                if (m_Shop.getProducts.Length - 1 != m_Shop.getActualProduct) return;
                productBackward.SetActive(true);
                productForward.SetActive(false);
                //m_EventSystem.SetSelectedGameObject(productBackward);
            }
            else
            {
                productBackward.SetActive(true);
                productForward.SetActive(true);
            }
        }

        void RefuggieDisplayUpdate() 
        {
            refuggiesText.text = m_PlayerSettings.wealth.ToString();
        }

        public void CallEventButtonSelect(PlayerSettings player, ShopProductStock item)
        {
            eventButtonSelect?.Invoke(player, item);
        }
        public void CallEventButtonBuy(PlayerSettings player, ShopProductStock item)
        {
            eventButtonBuy?.Invoke(player, item);
        }

        void BuyButton(InputAction.CallbackContext context) 
        {
            Debug.Log("Comprar o item");
            var item = m_Shop.getProducts[m_Shop.getActualProduct].GetComponent<UIItemShop>();

            if (item.productInStock.PlayerAlreadyBuy(m_PlayerSettings))
            {
                Debug.Log("Player já tem o produto. Venda não concluida.");
                return;
            }
            
            BuyThisItem(m_PlayerSettings, item.productInStock);
        }

        void SelectButton(InputAction.CallbackContext context)
        {
            Debug.Log("Selecionar o item");
            
            var item = m_Shop.getProducts[m_Shop.getActualProduct].GetComponent<UIItemShop>();

            if (!item.productInStock.PlayerAlreadyBuy(m_PlayerSettings))
            {
                Debug.Log("Player Não tem o produto. Seleção não concluida.");
                return;
            }

            SelectThisItem(m_PlayerSettings, item.productInStock);
        }

    }
}
