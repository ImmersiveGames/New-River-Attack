using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using Utils;
using Random = UnityEngine.Random;
namespace RiverAttack
{
    public class MineMaster: ObstacleMaster, IPoolable
    {
        internal override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            if (other.GetComponent<BulletPlayer>()) return;
            ComponentToKill(other.GetComponentInParent<PlayerMaster>(), CollisionType.Collider);
            GamePlayManager.instance.OnEventOtherEnemiesKillPlayer();
        }

        async void Start()
        {
            //await InitializationAsync();
            Debug.Log("Inicialização concluída!");
        }

        async Task InitializationAsync()
        { 
            gameObject.SetActive(false);
            await Task.Delay(Random.Range(500, 1500)); // Simulação de uma operação demorada por 1 segundo
            Debug.Log($"Inicialização parte 1 concluída!");
            gameObject.SetActive(true);
            /*await Task.Delay(Random.Range(1000, 1500)); // Simulação de outra operação demorada por 1,5 segundos
            Debug.Log($"Inicialização parte 2 concluída!");*/
        }
    }
}
