using NewRiverAttack.SaveManagers;
using NewRiverAttack.ShoppingSystems.SimpleShopping;
using TMPro;
using UnityEngine;

namespace NewRiverAttack.ShoppingSystems.WalletDisplay
{
    public class ShopWalletDisplay:MonoBehaviour
    {
        [SerializeField] private TMP_Text textWallet;
        
        private SimpleShoppingManager _simpleShoppingManager;

        private void Awake()
        {
            _simpleShoppingManager = GetComponentInParent<SimpleShoppingManager>();
        }

        private void OnEnable()
        {
            UpdateWalletDisplay();
            _simpleShoppingManager.EventBuyProduct += UpdateWalletDisplay;
        }

        private void OnDisable()
        {
            _simpleShoppingManager.EventBuyProduct -= UpdateWalletDisplay;
        }

        void UpdateWalletDisplay()
        {
            textWallet.text = GameOptionsSave.Instance.wallet.ToString();
        }
    }
}