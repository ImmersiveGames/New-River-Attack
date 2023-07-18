using RiverAttack;
using UnityEngine;
using UnityEngine.UI;

namespace Shopping
{
    public class UIItemShop : MonoBehaviour
    {
        [SerializeField, Header("Product Display")]
        private Text productName;
        [SerializeField]
        private Text productDescription;
        [SerializeField]
        private Text productPrice;
        [SerializeField]
        private Image productImage;
        [SerializeField]
        private Button btnBuy;
        [SerializeField]
        private Button btnSelect;

        public ShopProductStock myproductStock;
        private ShopMaster shopManager;

        public Button GetBuyButton { get { return btnBuy; } }
        public Button GetSelectButton { get { return btnSelect; } }

        private void OnEnable()
        {
            SetInitialReferences();
            shopManager.EventButtonBuy += UpdateBuyButton;
            shopManager.EventButtonSelect += SelectThisItem;
            //Firebase.RemoteConfig.FirebaseRemoteConfig.ActivateFetched();
        }

        private void SetInitialReferences()
        {
            shopManager = ShopMaster.instance;
        }

        public void SetupDisplay(ShopProductStock stockProduct)
        {
            myproductStock = stockProduct;
            //stockProduct.shopProduct.priceItem = (int)FirebaseRemoteConfig.GetValue(myproductStock.shopProduct.refPriceFirebase).LongValue;

            ShopProduct shopProduct = stockProduct.shopProduct;
            productName.text = shopProduct.name;
            productDescription.text = shopProduct.desciptionItem;
            productPrice.text = shopProduct.priceItem.ToString();
            productImage.sprite = shopProduct.spriteItem;
        }

        public void SetupButtons(PlayerStats player)
        {
            SetupBuyButton(player);
            SetupSelectButton(player);
        }

        public void UpdateBuyButton(PlayerStats player, ShopProductStock product)
        {
            SetupButtons(player);
        }

        public void SelectThisItem(PlayerStats player, ShopProductStock product)
        {
            SetupButtons(player);
        }

        private void SetupBuyButton(PlayerStats player)
        {
            btnBuy.gameObject.SetActive(true);
            btnBuy.interactable = false;
            if (myproductStock.AvariableForBuy(player))
                btnBuy.interactable = true;
            if (myproductStock.PlayerAlreadyBuy(player) && !myproductStock.shopProduct.isConsumable)
                btnBuy.gameObject.SetActive(false);
        }

        private void SetupSelectButton(PlayerStats player)
        {
            btnSelect.gameObject.SetActive(!myproductStock.AvariableForBuy(player));
            btnSelect.interactable = false;
            if (myproductStock.AvariableToSelect(player))
                btnSelect.interactable = true;
            if (myproductStock.shopProduct.isConsumable)
                btnSelect.gameObject.SetActive(false);

            //Debug.Log(myproductStock.shopProduct.GetName + "  Ativo: " + myproductStock.shopProduct.ShouldBeConsume(player));
        }

        private void OnDisable()
        {
            shopManager.EventButtonBuy -= UpdateBuyButton;
            shopManager.EventButtonSelect -= SelectThisItem;
        }
    }
}
