using ImmersiveGames.PoolSystems;
using UnityEngine;
using ImmersiveGames.PoolSystems.Interfaces;
using NewRiverAttack.BulletsManagers.Interface;

namespace NewRiverAttack.BulletsManagers
{
    public class Bullet : MonoBehaviour, IPoolable
    {
        private Vector3 _direction;
        private float _speed;
        private float _lifetime;
        private float _lifeTimer;
        private bool _isInitialized;

        public PoolObject Pool { get; set; } // Referência ao pool

        // Método chamado quando o projétil é instanciado do pool
        public void OnSpawned(Transform spawnPosition, ISpawnData data)
        {
            // Inicializa os dados da bala a partir do BulletSpawnData
            if (data is BulletSpawnData bulletData)
            {
                _direction = bulletData.Direction;
                _speed = bulletData.Speed;
                _lifetime = bulletData.Timer; // Usamos "Timer" como tempo de vida
            }

            _lifeTimer = _lifetime;

            transform.position = spawnPosition.position;
            transform.rotation = spawnPosition.rotation;

            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;

            // Movimenta o projétil na direção e velocidade fornecidas
            transform.position += _direction * _speed * Time.deltaTime;

            // Reduz o tempo de vida do projétil
            _lifeTimer -= Time.deltaTime;

            // Se o tempo de vida acabar, retorna ao pool
            if (_lifeTimer <= 0)
            {
                ReturnToPool();
            }
        }

        // Método para retornar o projétil ao pool
        private void ReturnToPool()
        {
            _isInitialized = false; // Desativar lógica de movimento
            Pool?.ReturnObject(gameObject); // Retorna ao pool
        }

        private void OnCollisionEnter(Collision collision)
        {
            // Quando a bala colide com algo, retorna ao pool
            ReturnToPool();
        }
    }
}