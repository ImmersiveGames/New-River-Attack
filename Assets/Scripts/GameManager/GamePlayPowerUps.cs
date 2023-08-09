using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class GamePlayPowerUps : Singleton<GamePlayPowerUps>
    {
        //ATENÇÃO NÃO ACEITA EVENTS PORQUE ELE NÃO VAI APRA A MEMORIA CHAMANDO PELO SCRIPTABLE
        //Este script precisa estar num prefab fora de scene geralmente _GamePlayEffects_
        public static PlayerSettings target;
        [Header("RapidFire PowerUp")]
        [Range(0.1f, 2)]
        public float minRapidFire = 0.1f;

        public void SetPowerUpTarget(PlayerSettings player)
        {
            target = player;
        }

        public void RapidFireStart(float amount)
        {
            Debug.Log("Ativou RapidFire");
            //TODO: Ajustar RapidFire
            if (target == null) return;
            float buff = (target.speedyShoot + amount < minRapidFire) ? minRapidFire : target.speedyShoot + amount;
            target.speedyShoot = buff;
            GamePlayManager.instance.CallEventRapidFire(true);
        }

        public void RapidFireEnd(float amount)
        {
            Debug.Log("Parou o RapidFire");
            if (target == null) return;
            target.speedyShoot = amount;
            GamePlayManager.instance.CallEventRapidFire(false);
        }

        public void RecoveryFuel(int amount)
        {
            if (target == null) return;
            if (target.actualHp < target.maxHp)
                target.actualHp += amount;
        }

        public void GainBomb(int amount)
        {
            if (target != null)
            {
                target.bombs += amount;
            }
        }
    }
}
