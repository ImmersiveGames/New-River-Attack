using NewRiverAttack.PlayerManagers.Tags;
using UnityEngine;
using UnityEngine.Localization;

namespace NewRiverAttack.ObstaclesSystems.ObjectsScriptable
{
    public abstract class ObjectsScriptable : ScriptableObject
    {
        [Header("Game Settings")]
        public bool canRespawn;
        public bool canKilled;
        public bool ignoreWalls;
        public bool ignoreEnemies;
        public bool ignoreBullets;
        [Header("Default Settings")]
        public new string name;
        public LocalizedString localizeName;
        public ObstacleTypes obstacleTypes;
        public int hitPoints = 1;
        public int valueScore;
        public SkinAttach[] defaultPrefabSkin;
        public bool randomSkin;
        [Header("Death Explosion Settings")]
        public GameObject deadParticlePrefab;
        [Range(1f,2f)]
        public float timeoutDestroy= 1.5f;
        [Range(0.1f,2f)]
        public float explodeSize= 0.7f;
        [Range(0f,5f)]
        public float shakeIntensity = 2f;
        [Range(0f,1f)]
        public float shakeTime = 0.1f;
        
        
        public string GetName()
        {
            return localizeName.IsEmpty ? name : localizeName.GetLocalizedString();
        }

        public int GetScore()
        {
            //TODO prever a mudança de nível de dificuldade.
            return valueScore;
        }
    }
}