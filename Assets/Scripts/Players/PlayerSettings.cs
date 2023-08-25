using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Shopping;
using UnityEngine.Serialization;

namespace RiverAttack
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "RiverAttack/Players", order = 101)]
    public class PlayerSettings : ScriptableObject
    {
        [SerializeField]
        public new string name;
        public int score;
        public float distance;
        [Header("Player Lives e Fuel")]
        public int actualFuel;
        public int lives;
        public int bombs;
        [Header("Shopping")]
        public int wealth;
        public List<ShopProduct> listProducts;
        [Header("Respawn Settings")]
        public Vector3 spawnPosition;
        public Quaternion spawnRotation;
        [Header("Skin Settings")]
        public ShopProductSkin playerSkin;

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
        public float shootVelocity;
        public float shootLifeTime;
        [Header("PowerUP Effects")]
        public float cadenceShootPowerUp;

    }
}
