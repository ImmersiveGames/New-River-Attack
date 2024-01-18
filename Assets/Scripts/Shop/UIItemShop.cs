using RiverAttack;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Shopping
{
    public class UIItemShop : MonoBehaviour
    {
        [SerializeField, Header("Product Display")]
        TMP_Text productName;
        //[SerializeField]
        //TMP_Text productDescription;
        [SerializeField]
        TMP_Text productPrice;
        [SerializeField]
        Image productImage;
        [SerializeField]
        Button btnBuy;
        [SerializeField]
        Button btnSelect;

        public ShopProductStock productInStock;
        ShopMaster m_ShopManager;

        public Button getBuyButton { get { return btnBuy; } }
        public Button getSelectButton { get { return btnSelect; } }

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_ShopManager.eventButtonBuy += UpdateBuyButton;
            m_ShopManager.eventButtonSelect += SelectThisItem;
        }
        void OnDisable()
        {
            m_ShopManager.eventButtonBuy -= UpdateBuyButton;
            m_ShopManager.eventButtonSelect -= SelectThisItem;
        }
  #endregion
        void SetInitialReferences()
        {
            m_ShopManager = ShopMaster.instance;
        }

        public void SetupDisplay(ShopProductStock stockProduct)
        {
            productInStock = stockProduct;
            var shopProduct = stockProduct.shopProduct;
            productName.text = shopProduct.name;
            //productDescription.text = shopProduct.descriptionItem;
            productPrice.text = shopProduct.priceItem.ToString();
            productImage.sprite = shopProduct.spriteItem;
        }

        public void SetupButtons(PlayerSettings player)
        {
            SetupBuyButton(player);
            SetupSelectButton(player);
        }

        void UpdateBuyButton(PlayerSettings player)
        {
            SetupButtons(player);
        }

        void SelectThisItem(PlayerSettings player)
        {
            SetupButtons(player);
        }

        void SetupBuyButton(PlayerSettings player)
        {
            btnBuy.gameObject.SetActive(true);
            //Debug.Log($"Available to buy: {productInStock.shopProduct.name} , {productInStock.AvailableForBuy(player)}");
            btnBuy.interactable = productInStock.AvailableForBuy(player);
            if (btnBuy.interactable)
            {
                btnBuy.GetComponent<Image>().color = ShopMaster.instance.buyerColor;
            }
            if (productInStock.PlayerAlreadyBuy(player) && !productInStock.shopProduct.isConsumable)
                btnBuy.gameObject.SetActive(false);
        }

        void SetupSelectButton(PlayerSettings player)
        {
            btnSelect.gameObject.SetActive(true);
            if (!productInStock.PlayerAlreadyBuy(player) && !productInStock.shopProduct.isConsumable)
            {
                btnSelect.gameObject.SetActive(false);
            }
            btnSelect.interactable = productInStock.AvailableToSelect(player) ;
            if (productInStock.shopProduct.isConsumable)
                btnSelect.gameObject.SetActive(false);
            //Debug.Log(myproductStock.shopProduct.GetName + "  Ativo: " + myproductStock.shopProduct.ShouldBeConsume(player));
        }
    }
}
