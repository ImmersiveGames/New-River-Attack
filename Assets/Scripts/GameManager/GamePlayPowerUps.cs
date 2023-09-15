using UnityEngine;
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
        
        [Header("RapidFire PowerUp")]
        [Range(0.1f, 1f)]
        public float cadenceRapidFireMin;

        public void SetPowerUpTarget(PlayerSettings player)
        {
            target = player;
        }

        public void RapidFireStart(float amount)
        {
            //TODO: Ajustar RapidFire - Animação da HUD
            GamePlayManager.instance.OnEventRapidFire();
            if (target == null) return;
            float buff = (amount < cadenceRapidFireMin) ? cadenceRapidFireMin : amount;
            target.cadenceShootPowerUp = buff;
            if (target.onRapidFire)
                return;
            GamePlayAudio.instance.AccelPinch(true);
            target.onRapidFire = true;
        }

        public void RapidFireEnd(float amount)
        {
            if (target == null) return;
            target.cadenceShootPowerUp = amount;
            target.onRapidFire = false;
            GamePlayAudio.instance.AccelPinch(false);
            GamePlayManager.instance.OnEventEndRapidFire();
            //GamePlayManager.instance.CallEventRapidFire(false);
        }

        public void GainBomb(int amount)
        {
            int maxBomb = GameSettings.instance.maxBombs;
            if (target == null || target.bombs >= maxBomb)
                return;
            target.bombs = (target.bombs + amount >= maxBomb) ? maxBomb : target.bombs + amount;
            GamePlayManager.instance.OnEventUpdateBombs(target.bombs);
        }
        
        public void GainLive(int amount)
        {
            int maxLives = GameSettings.instance.maxLives;
            if (target == null || target.lives >= maxLives)
                return;
            target.lives = (target.lives + amount >= maxLives) ? maxLives : target.lives + amount;
            GamePlayManager.instance.OnEventUpdateLives(target.lives);
        }
    }
}
