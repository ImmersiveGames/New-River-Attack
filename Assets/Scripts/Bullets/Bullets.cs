using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    public class Bullets : MonoBehaviour
    {
        [SerializeField] protected AudioEventSample audioShoot;
        [SerializeField] public float shootVelocity;
    }
}
