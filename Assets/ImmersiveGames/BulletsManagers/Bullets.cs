using ImmersiveGames.DebugManagers;
using UnityEngine;

namespace ImmersiveGames.BulletsManagers
{
    public abstract class Bullets: MonoBehaviour
    {
        protected IBulletsData BulletData;

        public virtual void OnSpawned(Transform spawnPosition, IBulletsData bulletData)
        {
            DebugManager.Log<Bullets>("Bullet object spawned.");
            // Atribui a posição e rotação do novo transform ao transform atual do objeto
            var transform1 = transform;
            transform1.position = spawnPosition.position;
            transform1.rotation = spawnPosition.rotation;
            BulletData = bulletData;
        }
        internal virtual void AutoDestroy(float timer)
        {
            //Debug.Log($"Timer In Bullet: {BulletData.BulletTimer}, Timer:{Time.time} , Time: {timer}");
            if (BulletData.BulletTimer > 0 && Time.time >= timer)
            {
                DestroyMe();
            }
        }
        protected virtual void DestroyMe()
        {
            gameObject.SetActive(false);
        }

        public IBulletsData GetBulletData => BulletData;
    }
}