using UnityEngine;

namespace NewRiverAttack.ObstaclesSystems.ObjectsScriptable
{
    [CreateAssetMenu(fileName = "Collectables", menuName = "ImmersiveGames/RiverAttack/Collectables", order = 202)]
    public class CollectibleScriptable : ObjectsScriptable
    {
        [Header("Collectibles Settings"), Tooltip("Limite de itens coletáveis")]
        public int maxCollectible;
        [Tooltip("Valor em dinheiro do coletável")]
        public int collectValuable;
        [Tooltip("Quantos itens dele mesmo ele representa")]
        public int amountCollectables = 1;
    }
}