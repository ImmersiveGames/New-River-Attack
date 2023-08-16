using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class BulletPlayerBomb : Bullets
    {
        [SerializeField]
        ParticleSystem pSystem;
        [SerializeField]
        float radiusSize;
        [SerializeField]
        float radiusSpeed;
        [SerializeField]
        float shakeForce;
        [SerializeField]
        float shakeTime;
        [SerializeField]
        long millisecondsVibrate;
        float timeLife
        {
            get;
            set;
        }
        float m_EndLife;
        double m_TParam;
        Collider m_Collider;

        GamePlayManager m_GamePlayManager;

        #region UNITY METHODS
        void OnEnable()
        {
            timeLife = pSystem.main.duration;
            m_Collider = GetComponent<Collider>();
            var audioSource = GetComponent<AudioSource>();
            m_GamePlayManager = GamePlayManager.instance;
            audioShoot.Play(audioSource);
        }

        // Use this for initialization
        void Start()
        {
            m_EndLife = Time.time + timeLife;
        }
        void FixedUpdate()
        {
            ExpandCollider();
            AutoDestroy();
        }
  #endregion

        void AutoDestroy()
        {
            if (Time.time >= m_EndLife)
            {
                DestroyMe();
            }
        }
        void ExpandCollider()
        {
            m_TParam += Time.deltaTime * radiusSpeed;
            m_GamePlayManager.OnEventShakeCam(shakeForce, shakeTime);

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
        new void DestroyMe()
        {
            GameObject o;
            (o = gameObject).SetActive(false);
            Destroy(o);
        }
    }
}
