using RiverAttack;
using UnityEngine;
using UnityEngine.UI;

namespace Shopping
{
    public class UIItemShop : MonoBehaviour
    {
        [SerializeField, Header("Product Display")]
        Text productName;
        [SerializeField]
        Text productDescription;
        [SerializeField]
        Text productPrice;
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
            productDescription.text = shopProduct.descriptionItem;
            productPrice.text = shopProduct.priceItem.ToString();
            productImage.sprite = shopProduct.spriteItem;
        }

        public void SetupButtons(PlayerSettings player)
        {
            SetupBuyButton(player);
            SetupSelectButton(player);
        }

        void UpdateBuyButton(PlayerSettings player, ShopProductStock product)
        {
            SetupButtons(player);
        }

        void SelectThisItem(PlayerSettings player, ShopProductStock product)
        {
            SetupButtons(player);
        }

        void SetupBuyButton(PlayerSettings player)
        {
            btnBuy.gameObject.SetActive(true);
            btnBuy.interactable = productInStock.AvailableForBuy(player);
            if (productInStock.PlayerAlreadyBuy(player) && !productInStock.shopProduct.isConsumable)
                btnBuy.gameObject.SetActive(false);
        }

        void SetupSelectButton(PlayerSettings player)
        {
            btnSelect.gameObject.SetActive(!productInStock.AvailableForBuy(player));
            btnSelect.interactable = false || productInStock.AvailableToSelect(player);
            if (productInStock.shopProduct.isConsumable)
                btnSelect.gameObject.SetActive(false);
            //Debug.Log(myproductStock.shopProduct.GetName + "  Ativo: " + myproductStock.shopProduct.ShouldBeConsume(player));
        }
    }
}
