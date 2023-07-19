using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    [CreateAssetMenu(fileName = "CollectibleItem", menuName = "RiverAttack/Collectible", order = 2)]
    public class CollectibleScriptable : EnemiesScriptable
    {
        [Header("Collectibles Settings"), Tooltip("Limite de itens coletaveis")]
        public int maxCollectible;
        [Tooltip("Valor em dinheiro do coletavel")]
        public int collectValuable;
        [Tooltip("Quantos itens dele mesmo ele representa")]
        public int amountCollectables = 1;
        [SerializeField]
        PowerUp powerUp;

        public PowerUp getPowerUp { get { return powerUp; } }
    }
}
