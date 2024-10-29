using NewRiverAttack.ObstaclesSystems.Abstracts;
using UnityEngine;

namespace NewRiverAttack.ShootSystems
{
    public abstract class ShootPatternBase : ScriptableObject
    {
        [SerializeField] protected float cooldown = 1.0f;
        private float _lastShootTime;

        private void OnEnable()
        {
            // Configura o tempo inicial para o cooldown iniciar corretamente
            _lastShootTime = Time.realtimeSinceStartup;
        }

        // Método para verificar se o cooldown está completo usando realtimeSinceStartup
        public virtual bool CanShoot()
        {
            return Time.realtimeSinceStartup >= _lastShootTime + cooldown;
        }

        // Método abstrato para executar o padrão de tiro específico
        public abstract void Execute(Transform spawnPoint, ObjectShoot shooter);

        // Define parâmetros específicos para o padrão de tiro
        public virtual void SetParameter(EnumShootParameter parameter, object value)
        {
            if (parameter == EnumShootParameter.Cooldown && value is float newCooldown)
            {
                cooldown = newCooldown;
            }
        }

        // Obtém parâmetros específicos para o padrão de tiro
        public virtual object GetParameter(EnumShootParameter parameterName)
        {
            return parameterName == EnumShootParameter.Cooldown ? cooldown : null;
        }

        // Atualiza o tempo do último tiro usando realtimeSinceStartup para manter o controle preciso do cooldown
        public void UpdateLastShootTime()
        {
            _lastShootTime = Time.realtimeSinceStartup;
        }
    }
}