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
        [SerializeField] private Transform contentShop;
        [SerializeField] private GameObject objProduct;
        [SerializeField] private RectTransform refCenter;
        [SerializeField] private ListShopStock productStock;
        [SerializeField] private ScrollRect scrollBarShop;
        [SerializeField] private TMP_Text wealthTMPText;
        [SerializeField] private GameObject productForward, productBackward;
        [SerializeField] private MenuPlayerSkinManager mainMenuPlayer;
        [SerializeField] private ListShopProduct allSkins;

        [Header("Carousel"), SerializeField] private bool infinityLooping;
        [SerializeField] private float spaceBetweenPanels, maxPosition;
        public Color selectedColor;
        public Color buyerColor;
        public Color normalColor;

        public AudioClip clickSound;

        private int m_LastSelectedSkin;

        private ShopCarousel m_Shop;
        private AudioSource m_AudioSource;
        private PlayersInputActions m_InputSystem;
        [SerializeField] private PlayerSettings playerSettings;

        #region Delegates
        public delegate void GeneralUpdateButtons(PlayerSettings player);
        public GeneralUpdateButtons eventButtonSelect;
        public GeneralUpdateButtons eventButtonBuy;

        //TODO: Diferenciar que está atualmente Selecionado e as que são possiveis de compra.
  #endregion
        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            SetControllersInput();
            WealthDisplayUpdate(playerSettings.wealth);
            SetupShop(playerSettings);
            scrollBarShop.horizontalScrollbar.numberOfSteps = m_Shop.getProducts.Length;
        }

        private void OnDisable()
        {
            m_InputSystem.UI_Controlls.Disable();
        }
        protected override void OnDestroy()
        {
            //base.OnDestroy();
        }
  #endregion

  private void SetInitialReferences()
        {
            m_AudioSource = GetComponentInParent<AudioSource>();
            if (m_AudioSource == null) Debug.LogWarning("Componente de Audio não encontrado.");
        }

        private void SetControllersInput()
        {
            m_InputSystem = new PlayersInputActions();
            m_InputSystem.UI_Controlls.Enable();

            m_InputSystem.UI_Controlls.BuyButton.performed += BuyInputButton;
            m_InputSystem.UI_Controlls.SelectButton.performed += SelectButton;
            m_InputSystem.UI_Controlls.LeftSelection.performed += _ => ControllerNavigationArrows(-1);
            m_InputSystem.UI_Controlls.RightSelection.performed += _ => ControllerNavigationArrows(1);
        }

        private void SetupShop(PlayerSettings player)
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
                item.SetupButtons(player);
                item.getBuyButton.onClick.AddListener(delegate { BuyThisItem(player, item.productInStock); });
                item.getSelectButton.onClick.AddListener(delegate { SelectThisItem(player, item.productInStock); });

                if (item.getSelectButton.interactable == false)
                    item.getSelectButton.GetComponent<Image>().color = selectedColor;
                if (item.getBuyButton.interactable)
                    item.getBuyButton.GetComponent<Image>().color = buyerColor;
            }
        }

        private void BuyThisItem(PlayerSettings player, ShopProductStock product)
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
            GameSteamManager.UnlockAchievement("ACH_BUY_SKIN");
            m_AudioSource.PlayOneShot(clickSound);

            if (Tools.CheckSameElements(allSkins.value, player.listProducts))
            {
                GameSteamManager.UnlockAchievement("ACH_BUY_SKIN_ALL");
            }
            
        }

        private void SelectThisItem(PlayerSettings player, ShopProductStock shopProductStock)
        {
            shopProductStock.shopProduct.ConsumeProduct(player);
            player.playerSkin = shopProductStock.shopProduct as ShopProductSkin;
            CallEventButtonSelect(player);
            mainMenuPlayer.ChangePlayerSkin();

            var lastSelectBtn = m_Shop.getProducts[m_LastSelectedSkin].GetComponent<UIItemShop>().getSelectButton;
            lastSelectBtn.GetComponent<Image>().color = normalColor;

            var selectBtn = m_Shop.getProducts[m_Shop.getActualProduct].GetComponent<UIItemShop>().getSelectButton;
            selectBtn.GetComponent<Image>().color = selectedColor;

            m_LastSelectedSkin = m_Shop.getActualProduct;

            m_AudioSource.PlayOneShot(clickSound);
        }

        private void WealthDisplayUpdate(int wealth)
        {
            wealthTMPText.text = wealth.ToString();
        }

        #region INPUT BUTTONS

        private void BuyInputButton(InputAction.CallbackContext context)
        {
            //Debug.Log("Comprar o item");
            var item = m_Shop.getProducts[m_Shop.getActualProduct].GetComponent<UIItemShop>();

            BuyThisItem(playerSettings, item.productInStock);
        }

        private void SelectButton(InputAction.CallbackContext context)
        {

            var item = m_Shop.getProducts[m_Shop.getActualProduct].GetComponent<UIItemShop>();

            if (!item.productInStock.PlayerAlreadyBuy(playerSettings))
            {
                return;
            }

            SelectThisItem(playerSettings, item.productInStock);
        }

        private void ControllerNavigationArrows(int value)
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
            m_AudioSource.PlayOneShot(clickSound);

            m_Shop.ButtonNavigation(next);

            float scrollbarValue = (m_Shop.getActualProduct) / ((float)m_Shop.getProducts.Length - 1);

            scrollBarShop.horizontalScrollbar.value = scrollbarValue;

            if (!infinityLooping)
            {
                if (m_Shop.getActualProduct == 0)
                {
                    productBackward.SetActive(false);
                    productForward.SetActive(true);
                }
                else
                {
                    productBackward.SetActive(true);
                    productForward.SetActive(true);
                }
                if (m_Shop.getProducts.Length - 1 != m_Shop.getActualProduct) return;
                productBackward.SetActive(true);
                productForward.SetActive(false);
            }
            else
            {
                productBackward.SetActive(true);
                productForward.SetActive(true);
            }
        }
  #endregion

        #region Calls

        private void CallEventButtonSelect(PlayerSettings player)
        {
            eventButtonSelect?.Invoke(player);
        }

        private void CallEventButtonBuy(PlayerSettings player)
        {
            eventButtonBuy?.Invoke(player);
        }
  #endregion

    }
}
