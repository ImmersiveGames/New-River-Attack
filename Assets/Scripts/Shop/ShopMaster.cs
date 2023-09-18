using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using RiverAttack;
using Utils;
using TMPro;

namespace Shopping
{
    public class ShopMaster : Singleton<ShopMaster>
    {
        [SerializeField]
        Transform contentShop;
        [SerializeField]
        GameObject objProduct;
        [SerializeField]
        RectTransform refCenter;
        [SerializeField]
        ListShopStock productStock;
        [SerializeField]
        ScrollRect scrollBarShop;
        [SerializeField]
        TMP_Text wealthTMPText;
        [SerializeField]
        GameObject productForward, productBackward;

        [Header("Carousel"), SerializeField]
        bool infinityLooping;
        [SerializeField]
        float spaceBetweenPanels, maxPosition;
        public Color selectedColor;
        public Color normalColor;

        int m_LastSelectedSkin;

        ShopCarousel m_Shop;
        AudioSource m_AudioSource;
        GamePlayAudio m_GamePlayAudio;
        PlayersInputActions m_InputSystem;
        PlayerSettings m_ActivePlayer;

        #region Delegates
        public delegate void GeneralUpdateButtons(PlayerSettings player);
        public GeneralUpdateButtons eventButtonSelect;
        public GeneralUpdateButtons eventButtonBuy;

        //TODO: Diferenciar que está atualmente Selecionado e as que são possiveis de compra.
  #endregion
        #region UNITYMETHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_ActivePlayer = GameManager.instance.playerSettingsList[0];
            SetControllersInput();
            WealthDisplayUpdate(m_ActivePlayer.wealth);
            SetupShop(m_ActivePlayer);
            scrollBarShop.horizontalScrollbar.numberOfSteps = m_Shop.getProducts.Length;
        }
        void OnDisable()
        {
            m_InputSystem.UI_Controlls.Disable();
            //Debug.Log("Desabilitei os controles");
        }
  #endregion
        void SetInitialReferences()
        {
            m_GamePlayAudio = GamePlayAudio.instance;
            m_AudioSource = GetComponentInParent<AudioSource>();
            if (m_AudioSource == null) Debug.LogWarning("Componente de Audio não encontrado.");
        }

        void SetControllersInput()
        {
            m_InputSystem = new PlayersInputActions();
            m_InputSystem.UI_Controlls.Enable();
            //Debug.Log("Habilitei os controles");

            m_InputSystem.UI_Controlls.BuyButton.performed += BuyInputButton;
            m_InputSystem.UI_Controlls.SelectButton.performed += SelectButton;
            m_InputSystem.UI_Controlls.LeftSelection.performed += _ => ControllerNavigationArrows(-1);
            m_InputSystem.UI_Controlls.RightSelection.performed += _ => ControllerNavigationArrows(1);
        }
        void SetupShop(PlayerSettings playerSettings)
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
                item.SetupButtons(playerSettings);
                item.getBuyButton.onClick.AddListener(delegate { BuyThisItem(playerSettings, item.productInStock); });
                item.getSelectButton.onClick.AddListener(delegate { SelectThisItem(playerSettings, item.productInStock); });

                if (item.getSelectButton.interactable == false)
                    item.getSelectButton.GetComponent<Image>().color = selectedColor;
            }
        }
        void BuyThisItem(PlayerSettings player, ShopProductStock product)
        {
            if (!product.AvailableInStock() || !product.HaveMoneyToBuy(player))
                return;
            player.listProducts.Add(product.shopProduct);
            player.wealth += -product.shopProduct.priceItem;
            product.shopProduct.ConsumeProduct(player);
            product.RemoveStock(1);
            CallEventButtonBuy(player);
            WealthDisplayUpdate(player.wealth);

            var item = m_Shop.getProducts[m_Shop.getActualProduct].GetComponent<UIItemShop>();
            item.getSelectButton.interactable = true;

            m_GamePlayAudio.PlayClickSfx(m_AudioSource);

        }
        void SelectThisItem(PlayerSettings player, ShopProductStock shopProductStock)
        {
            shopProductStock.shopProduct.ConsumeProduct(player);
            player.playerSkin = shopProductStock.shopProduct as ShopProductSkin;
            CallEventButtonSelect(player);

            var lastSelectBtn = m_Shop.getProducts[m_LastSelectedSkin].GetComponent<UIItemShop>().getSelectButton;
            lastSelectBtn.GetComponent<Image>().color = normalColor;

            var selectBtn = m_Shop.getProducts[m_Shop.getActualProduct].GetComponent<UIItemShop>().getSelectButton;
            selectBtn.GetComponent<Image>().color = selectedColor;

            m_LastSelectedSkin = m_Shop.getActualProduct;

            m_GamePlayAudio.PlayClickSfx(m_AudioSource);
        }
        void WealthDisplayUpdate(int wealth)
        {
            wealthTMPText.text = wealth.ToString();
        }

        #region INPUT BUTTONS
        void BuyInputButton(InputAction.CallbackContext context)
        {
            Debug.Log("Comprar o item");
            var item = m_Shop.getProducts[m_Shop.getActualProduct].GetComponent<UIItemShop>();

            /*if (item.productInStock.PlayerAlreadyBuy(m_ActivePlayer))
                return;
            if (!item.productInStock.HaveMoneyToBuy(m_ActivePlayer)) 
                return;*/
            BuyThisItem(m_ActivePlayer, item.productInStock);
        }
        void SelectButton(InputAction.CallbackContext context)
        {
            // Debug.Log("Selecionar o item");

            var item = m_Shop.getProducts[m_Shop.getActualProduct].GetComponent<UIItemShop>();

            if (!item.productInStock.PlayerAlreadyBuy(m_ActivePlayer))
            {
                // Debug.Log("Player Não tem o produto. Seleção não concluida.");
                return;
            }

            SelectThisItem(m_ActivePlayer, item.productInStock);
        }
        void ControllerNavigationArrows(int value)
        {
            switch (value)
            {
                case -1:
                    productBackward.GetComponent<Button>().onClick.Invoke();
                    break;
                case 1:
                    productForward.GetComponent<Button>().onClick.Invoke();
                    break;
            }
        }
        public void ButtonNavigation(int next)
        {
            m_GamePlayAudio.PlayClickSfx(m_AudioSource);

            m_Shop.ButtonNavigation(next);

            //Debug.Log(m_Shop.getActualProduct);
            //Debug.Log(m_Shop.getProducts.Length);

            float scrollbarValue = ((float)m_Shop.getActualProduct) / ((float)m_Shop.getProducts.Length - 1);
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
  #endregion

        #region Calls
        void CallEventButtonSelect(PlayerSettings player)
        {
            eventButtonSelect?.Invoke(player);
        }
        void CallEventButtonBuy(PlayerSettings player)
        {
            eventButtonBuy?.Invoke(player);
        }
  #endregion

    }
}
