using System;
using RiverAttack;
using UnityEngine;

public class EnemiesShootSpawn : MonoBehaviour
{
    // Apenas para Marcação de objeto

    void Start()
    {
        var enemiesShoot = GetComponentInParent<EnemiesShoot>();
        if(enemiesShoot == null) return;
        enemiesShoot.spawnPoint = transform;
    }
}
