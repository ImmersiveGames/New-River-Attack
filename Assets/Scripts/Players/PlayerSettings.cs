using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shopping;

namespace RiverAttack
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "RiverAttack/Players", order = 101)]
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField]
        public new string name;
        public int score;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;
        [Header("Skin Settings")]
        public ShopProductSkin playerSkin;
        [Header("Shopping")]
        public int wealth;
        public List<ShopProduct> listProducts;
        [Header("Player")]
        public float mySpeedy;
        public float myAgility;
        [Range(1f, 30f)]
        public float speedHorizontal;
        [Range(1f, 10f)]
        public float speedVertical;
        [Range(1.1f, 5f)]
        public float multiplyVelocityUp;
        [Range(.01f, 1f)]
        public float multiplyVelocityDown;
        public float cadenceShoot;
        public float shootVelocity = 10f;
        [Header("PowerUP Effects")]
        public float speedyShoot;
        [Header("Player Lives e HP")]
        public int maxHp;
        public int actualHp;
        public int startLives;
        public int maxLives;
        public int lives;
        public int startBombs;
        public int bombs;
        public int maxBombs;
    }
}
