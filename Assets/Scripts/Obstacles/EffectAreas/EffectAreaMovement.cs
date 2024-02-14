using UnityEngine;
using Utils;
namespace RiverAttack
{
    public sealed class EffectAreaMovement: EnemiesMovement
    {
        internal Transform myPool;

        private void Update()
        {
            if (!m_GamePlayManager.shouldBePlayingGame || !m_ObstacleMaster.shouldObstacleBeReady || m_ObstacleMaster.isDestroyed)
                return;
            m_ActualState.UpdateState(transform, m_VectorDirection);
        }
        internal void AutoDestroyMe(float timer)
        {
            Invoke(nameof(DestroyMe), timer);
        }

        private void DestroyMe()
        {
            gameObject.SetActive(false);
            if (!myPool)
                return;
            gameObject.transform.SetParent(myPool);
            gameObject.transform.SetAsLastSibling();
        }

    }
}
