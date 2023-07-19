using UnityEngine;

namespace RiverAttack
{
    [RequireComponent(typeof(LevelObstacleSpawnMaster))]
    public class LevelObstacleSpawnSkins : ObstacleSkins
    {
        [ContextMenu("LoadPrefab")]
        void LoadPrefab()
        {
            var spawnMaster = GetComponent<LevelObstacleSpawnMaster>();
            var skin = spawnMaster.getPrefab.GetComponent<ObstacleSkins>();
            if (skin == null) return;
            indexSkin = skin.indexSkin;
            randomSkin = skin.randomSkin;
            skins = skin.enemiesSkins;
        }
    }
}
