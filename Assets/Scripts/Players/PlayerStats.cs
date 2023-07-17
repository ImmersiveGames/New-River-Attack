using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace RiverAttack
{
    [CreateAssetMenu(fileName = "PlayerStats", menuName = "RiverAttack/Players", order = 101)]
    public class PlayerStats : ScriptableObject
    {
        [SerializeField]
        public new string name;
        public int score;
        public Vector3 spawnPosition;
        public Vector3 spawnRotation;
        /*[Header("Skin Settings")]
        [SerializeField]
        public ShopProductSkin playerSkin;
        [Header("Shopping")]
        public int wealth;
        [SerializeField]
        public List<ShopProduct> listProducts;
        [Header("Controller Settings")]
        public ControllerMap controllerMap;*/

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
        [FormerlySerializedAs("maxHP")]
        [Header("Player Lives e HP")]
        public int maxHp;
        public int actualHp;
        public int startLives;
        public int maxLives;
        public int lives;
        public int bombs;
        public int maxBombs;

        /*public void ChangeLife(int life)
        {
            if (maxLives != 0 && lives.Value + life >= maxLives)
                lives.SetValue(maxLives);
            else if (lives.Value + life <= 0)
                lives.SetValue(0);
            else
                lives.ApplyChange(life);
        }
    
        public void InitPlayer()
        {
            lives.SetValue(startLives);
            bombs.SetValue(3);
            actualHp.SetValue(maxHp);
            //LoadListShop();
        }*/
    }
}