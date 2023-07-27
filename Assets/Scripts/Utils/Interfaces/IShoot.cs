using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Utils
{
    public interface IShoot
    {
        void Fire();
        void SetTarget(Transform toTarget);
    }
}
