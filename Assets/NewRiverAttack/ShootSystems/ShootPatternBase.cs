using System;
using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.ShootSystems
{
    public abstract class ShootPatternBase : ScriptableObject
    {
        [SerializeField] protected float cooldown = 1.0f;
        private float _lastShootTime = -Mathf.Infinity;  // Inicia para permitir o primeiro disparo

        private void OnEnable()
        {
            _lastShootTime = -cooldown;  // Permite o disparo inicial imediato
        }

        public bool TryShoot(Action executeShootAction)
        {
            if (!(Time.realtimeSinceStartup >= _lastShootTime + cooldown)) return false;
            executeShootAction.Invoke();           // Executa o disparo
            _lastShootTime = Time.realtimeSinceStartup; // Atualiza o tempo para o próximo cooldown
            return true;
        }
       

        public virtual void SetParameter(EnumShootParameter parameter, object value)
        {
            if (parameter == EnumShootParameter.Cooldown && value is float newCooldown)
            {
                cooldown = newCooldown;
            }
            else
            {
                Debug.LogWarning($"Parâmetro '{parameter}' não suportado pelo padrão de tiro base ou tipo inválido.");
            }
        }

        protected virtual object GetParameter(EnumShootParameter parameterName)
        {
            return parameterName == EnumShootParameter.Cooldown ? cooldown : null;
        }


        public abstract void Execute(Transform spawnPoint, ObjectShoot shooter);
    }
}