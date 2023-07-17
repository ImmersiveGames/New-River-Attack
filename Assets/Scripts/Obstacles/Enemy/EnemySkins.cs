using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace RiverAttack
{
    public class EnemySkins : ObstacleSkins
    {

        private EnemiesMaster m_EnemiesMaster;
        //Usa o Obstacles
        private void Start()
        {
            SetInitialReferences();
            m_EnemiesMaster.SetTagLayer(enemySkins, tag, gameObject.layer);
            m_EnemiesMaster.CallEventChangeSkin();
        }

        private void SetInitialReferences()
        {
            m_EnemiesMaster = GetComponent<EnemiesMaster>();
        }
    }
}
