using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class PlayerBombSet : Bullets
    {

        [SerializeField]
        private ParticleSystem pSystem;
        [SerializeField]
        private float radiusSize;
        [SerializeField]
        private float radiusSpeed;
        [SerializeField]
        private float shakeForce;
        [SerializeField]
        private float shakeTime;
        [SerializeField]
        private long millisecondsVibrate;
        float timeLife
        {
            get;
            set;
        }
        private float m_EndLife;
        private double m_TParam;

        private Collider m_Collider;

        private void OnEnable()
        {
            timeLife = pSystem.main.duration;
            m_Collider = GetComponent<Collider>();
        }

        // Use this for initialization
        void Start()
        {
            m_EndLife = Time.time + timeLife;
        }

        private void AutoDestroy()
        {
            if (Time.time >= m_EndLife)
            {
                DestroyMe();
            }
        }
        private void ExpandCollider()
        {
            m_TParam += Time.deltaTime * radiusSpeed;
            GamePlayManager.instance.CallEventShakeCam(shakeForce, shakeTime);

//TODO: Arrumar nova forma de vibrar o celular
#if UNITY_ANDROID && !UNITY_EDITOR
            ToolsAndroid.Vibrate(millisecondsVibrate);
            Handheld.Vibrate();
#endif
            if (m_Collider.GetType() != typeof(SphereCollider))
                return;
            var sphere = (SphereCollider)m_Collider;
            sphere.radius = Mathf.Lerp(0.5f, radiusSize, (float)m_TParam);
        }
        void FixedUpdate()
        {
            ExpandCollider();
            AutoDestroy();
        }

        private void DestroyMe()
        {
            GameObject o;
            (o = gameObject).SetActive(false);
            Destroy(o);
        }
    }
}
