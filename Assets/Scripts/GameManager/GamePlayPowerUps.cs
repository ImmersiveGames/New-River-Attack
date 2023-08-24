using UnityEngine;
using UnityEngine.Serialization;
using Utils;

namespace RiverAttack
{
    public class GamePlayPowerUps : Singleton<GamePlayPowerUps>
    {
        //ATENÇÃO NÃO ACEITA EVENTS PORQUE ELE NÃO VAI APRA A MEMORIA CHAMANDO PELO SCRIPTABLE
        //Este script precisa estar num prefab fora de scene geralmente _GamePlayEffects_
        public static PlayerSettings target;
        
        public void RecoveryFuel(int amount)
        {
            if (target == null) return;
            if (target.actualFuel >= GameSettings.instance.maxFuel) return;
                
            target.actualFuel += amount;
            GamePLayLogFuel(amount);
        }

        static void GamePLayLogFuel(int fuel)
        {
            var gamePlaySettings = GamePlaySettings.instance;
            gamePlaySettings.fuelStocked += fuel;
        }
    
        [FormerlySerializedAs("minRapidFire")]
        [Header("RapidFire PowerUp")]
        [Range(0.1f, 1f)]
        public float cadenceRapidFireMin;

        public void SetPowerUpTarget(PlayerSettings player)
        {
            target = player;
        }

        public void RapidFireStart(float amount)
        {
            //TODO: Ajustar RapidFire
            if (target == null) return;
            float buff = (amount < cadenceRapidFireMin) ? cadenceRapidFireMin : amount;
            target.cadenceShootPowerUp = buff;
            //GamePlayManager.instance.CallEventRapidFire(true);
        }

        public void RapidFireEnd(float amount)
        {
            if (target == null) return;
            target.cadenceShootPowerUp = amount;
            //GamePlayManager.instance.CallEventRapidFire(false);
        }

        public void GainBomb(int amount)
        {
            if (target != null)
            {
                target.bombs += amount;
            }
            GamePlayManager.instance.OnEventUpdateBombs(target.bombs);
        }
    }
}
