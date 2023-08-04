namespace RiverAttack
{
    public class EffectAreaMaster : EnemiesMaster
    {
        public event GeneralEventHandler EventEnterAreaEffect;
        public event GeneralEventHandler EventExitAreaEffect;

        public void CallEventAreaEffect()
        {
            EventEnterAreaEffect?.Invoke();
        }

        public void CallEventExitAreaEffect()
        {
            EventExitAreaEffect?.Invoke();
        }
    }
}

