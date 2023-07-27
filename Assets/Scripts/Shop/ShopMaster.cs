using UnityEngine;
using System.Threading.Tasks;
using RiverAttack;
using Utils;

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
        GameObject productForward, productBackward;

        [Header("Carousel"), SerializeField]
        bool infinityLooping;
        [SerializeField]
        float spaceBetweenPanels = 0, maxPosition = 0;

        PlayerSettings m_ActivePlayer;
        ShopCarousel m_Shop;
        Task m_Task;

        public delegate void GeneralUpdateButtons(PlayerSettings player, ShopProductStock item);
        public GeneralUpdateButtons eventButtonSelect;
        public GeneralUpdateButtons eventButtonBuy;

        #region UNITY METHODS
        void OnEnable()
        {
            //SaveGame.DeleteAll();
            SetInitialReferences();
            SetupShop();
        }

        void Start()
        {
            productForward.SetActive(true);
            productBackward.SetActive(true);
            if (m_Shop.getActualProduct == 0 && !infinityLooping)
            {
                productBackward.SetActive(false);
            }
        }
        void LateUpdate()
        {
            m_Shop?.Update();
        }
        void OnDisable()
        {
            //GameManagerSaves.Instance.SavePlayer(activePlayer);
        }
  #endregion
        void SetInitialReferences()
        {
            var activePlayer = GameManager.instance.GetActivePlayerTransform(0);
            m_ActivePlayer = activePlayer.GetComponent<PlayerMaster>().GetPlayersSettings();
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
                item.SetupButtons(m_ActivePlayer);
                item.getBuyButton.onClick.AddListener(delegate { BuyThisItem(m_ActivePlayer, item.productInStock); });
                item.getSelectButton.onClick.AddListener(delegate { SelectThisItem(m_ActivePlayer, item.productInStock); });
            }
        }

        void BuyThisItem(PlayerSettings player, ShopProductStock product)
        {
            player.listProducts.Add(product.shopProduct);
            player.wealth += -product.shopProduct.priceItem;
            product.shopProduct.ConsumeProduct(player);
            product.RemoveStock(1);
            CallEventButtonBuy(player, product);
        }
        void SelectThisItem(PlayerSettings player, ShopProductStock shopProductStock)
        {
            shopProductStock.shopProduct.ConsumeProduct(player);
            CallEventButtonSelect(player, shopProductStock);
        }

        public void ButtonNavegation(int next)
        {
            m_Shop.ButtonNavegation(next);
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

        public void CallEventButtonSelect(PlayerSettings player, ShopProductStock item)
        {
            eventButtonSelect?.Invoke(player, item);
        }
        public void CallEventButtonBuy(PlayerSettings player, ShopProductStock item)
        {
            eventButtonBuy?.Invoke(player, item);
        }
    }
}
