using UnityEngine;
using System.Threading.Tasks;
using RiverAttack;
using Utils;

namespace Shopping
{

    public class ShopMaster : Singleton<ShopMaster>
    {
        [SerializeField]
        private Transform contentShop;
        [SerializeField]
        private GameObject objProduct;
        [SerializeField]
        private RectTransform refCenter;
        [SerializeField]
        private ListShopStock productStock;
        [SerializeField]
        private GameObject productFoward, productBackward;

        [Header("Carousel"), SerializeField]
        private bool infinityLooping;
        [SerializeField]
        private float gapBeteenPanels = 0, maxposition = 0;

        private PlayerStats activePlayer;
        private ShopCarousel shop;
        private Task task;

        public delegate void GeneralUpdateButtons(PlayerStats player, ShopProductStock item);
        public GeneralUpdateButtons EventButtonSelect;
        public GeneralUpdateButtons EventButtonBuy;

        private void OnEnable()
        {
            //SaveGame.DeleteAll();
            SetInitialReferences();
            SetupShop();
        }

        private void Start()
        {
            productFoward.SetActive(true);
            productBackward.SetActive(true);
            if (shop.GetActualProduct == 0 && !infinityLooping)
            {
                productBackward.SetActive(false);
            }
        }

        private void SetInitialReferences()
        {
            activePlayer = GameManager.instance.GetFirstPlayer(0);
            //GameManagerSaves.Instance.LoadPlayer(ref activePlayer);
            //activePlayer.LoadValues();
        }

        private void SetupShop()
        {
            shop = new ShopCarousel(contentShop, objProduct, refCenter);
            shop.maxposition = maxposition;
            shop.gapBeteenPanels = gapBeteenPanels;
            shop.infinityLooping = infinityLooping;
            if (shop != null)
            {
                shop.CreateShopping(productStock.value);
                for (int i = 0; i < shop.GetProducts.Length; i++)
                {
                    UIItemShop item = shop.GetProducts[i].GetComponent<UIItemShop>();
                    if (item)
                    {
                        item.SetupButtons(activePlayer);
                        item.GetBuyButton.onClick.AddListener(delegate { BuyThisItem(activePlayer, item.myproductStock); });
                        item.GetSelectButton.onClick.AddListener(delegate { SelectThisItem(activePlayer, item.myproductStock); });
                    }
                }
            }
        }

        public void BuyThisItem(PlayerStats player, ShopProductStock product)
        {
            player.listProducts.Add(product.shopProduct);
            player.wealth += -product.shopProduct.priceItem;
            product.shopProduct.ConsumeProduct(player);
            product.RemoveStock(1);
            CallEventButtonBuy(player, product);
        }
        private void SelectThisItem(PlayerStats player, ShopProductStock productStock)
        {
            productStock.shopProduct.ConsumeProduct(player);
            CallEventButtonSelect(player, productStock);
        }

        private void LateUpdate()
        {
            if (shop != null)
                shop.Update();
        }

        public void ButtonNavegation(int next)
        {
            shop.ButtonNavegation(next);
            if (!infinityLooping)
            {
                if (shop.GetActualProduct == 0)
                {
                    productBackward.SetActive(false);
                    productFoward.SetActive(true);
                }
                else
                {
                    productBackward.SetActive(true);
                    productFoward.SetActive(true);
                }
                if (shop.GetProducts.Length - 1 == shop.GetActualProduct)
                {
                    productBackward.SetActive(true);
                    productFoward.SetActive(false);
                }
            }
            else
            {
                productBackward.SetActive(true);
                productFoward.SetActive(true);
            }

        }

        public void CallEventButtonSelect(PlayerStats player, ShopProductStock item)
        {
            if (EventButtonSelect != null)
            {
                EventButtonSelect(player, item);
            }
        }
        public void CallEventButtonBuy(PlayerStats player, ShopProductStock item)
        {
            if (EventButtonBuy != null)
            {
                EventButtonBuy(player, item);
            }
        }

        private void OnDisable()
        {
            //GameManagerSaves.Instance.SavePlayer(activePlayer);
        }
    }
}
