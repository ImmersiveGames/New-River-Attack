using System;
using UnityEngine;
namespace RiverAttack
{
    public class EffectAreaMaster : ObstacleMaster
    {
        public event GeneralEventHandler EventEnterAreaEffect;
        public event GeneralEventHandler EventExitAreaEffect;

        #region UNITYMETHODS
        internal override void OnTriggerEnter(Collider other)
        {
            base.OnTriggerEnter(other);
            Debug.Log("Direto do effeito");
        }
        void OnTriggerExit(Collider other)
        {
            throw new NotImplementedException();
        }
  #endregion

        public void OnEventAreaEffect()
        {
            EventEnterAreaEffect?.Invoke();
        }

        public void OnEventExitAreaEffect()
        {
            EventExitAreaEffect?.Invoke();
        }
    }
}

