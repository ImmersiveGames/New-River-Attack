using UnityEngine;
using Shopping;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "ShopBomb", menuName = "RiverAttack/Shopping/Bombs", order = 3)]
    public class ShopProductBomb : ShopProduct
    {
        [SerializeField] private CollectibleScriptable bombPowerUpScriptable;
        [SerializeField] private int quantity;

        #region UNITY METHODS

        private void OnEnable()
        {
            isConsumable = true;
        }
  #endregion
        public override bool ShouldBeConsume(PlayerSettings player)
        {
            return player.bombs + quantity <= bombPowerUpScriptable.maxCollectible;
        }

        public override void ConsumeProduct(PlayerSettings player)
        {
            player.bombs += quantity;
        }
    }
}
