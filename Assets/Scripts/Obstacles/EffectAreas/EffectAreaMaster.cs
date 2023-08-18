using UnityEngine;
namespace RiverAttack
{
    public class EffectAreaMaster : ObstacleMaster
    {
        /*public event GeneralEventHandler EventEnterAreaEffect;
        public event GeneralEventHandler EventExitAreaEffect;

        public void CallEventAreaEffect()
        {
            EventEnterAreaEffect?.Invoke();
        }

        public void CallEventExitAreaEffect()
        {
            EventExitAreaEffect?.Invoke();
        }*/
        protected override void HitThis(Collider collision)
        {
            throw new System.NotImplementedException();
        }
    }
}

