using UnityEngine;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "CollectibleItem", menuName = "RiverAttack/Collectible", order = 2)]
    public class CollectibleScriptable : EnemiesScriptable
    {
        [Header("Collectibles Settings"), Tooltip("Limite de itens coletáveis")]
        public int maxCollectible;
        [Tooltip("Valor em dinheiro do coletável")]
        public int collectValuable;
        [Tooltip("Quantos itens dele mesmo ele representa")]
        public int amountCollectables = 1;
        public PowerUp powerUp;
    }
}
