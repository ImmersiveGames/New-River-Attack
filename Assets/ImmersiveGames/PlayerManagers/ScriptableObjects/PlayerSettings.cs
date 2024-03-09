using System.Collections.Generic;
using ImmersiveGames.ShopManagers;
using UnityEngine;

namespace ImmersiveGames.PlayerManagers.ScriptableObjects
{
    [CreateAssetMenu(fileName = "PlayerSetting", menuName = "ImmersiveGames/RiverAttack/PlayerSettings", order = 101)]
    public class PlayerSettings: ScriptableObject
    {
        [Header("Shopping")]
        public int wallet;
        public List<ProductStock> listPlayerProductStocks;
    }
}