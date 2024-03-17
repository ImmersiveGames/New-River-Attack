using ImmersiveGames.SaveManagers;
using ImmersiveGames.ShopManagers.SimpleShopping;
using TMPro;
using UnityEngine;

namespace ImmersiveGames.ShopManagers.WalletDisplay
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
            textWallet.text = GameOptionsSave.instance.wallet.ToString();
        }
    }
}