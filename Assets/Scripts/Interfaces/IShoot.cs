using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    public interface IShoot
    {

        void Fire();
        bool ShouldFire();
        void SetTarget(Transform toTarget);
    }
}
