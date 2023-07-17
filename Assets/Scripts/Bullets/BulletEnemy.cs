using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils;
namespace RiverAttack
{
    [RequireComponent(typeof(Collider), typeof(AudioSource))]
    public class BulletEnemy : MonoBehaviour, IPoolable {
        [HideInInspector]
        public float shootVelocity;
        [HideInInspector]
        public Vector3 shootDirection;
        [SerializeField] AudioEventSample audioShoot;
        [SerializeField]
        private float TimeToDestroy;

        private GameSettings gameSettings;
        private Collider col;
        private AudioSource audioSource;

        private void OnEnable()
        {
            SetInitialReferences();
            audioShoot.Play(audioSource);
            col.enabled = true;
            Invoke("DisableShoot", TimeToDestroy);
        }

        private void SetInitialReferences()
        {
            gameSettings = GameSettings.instance;
            col = GetComponent<Collider>();
            audioSource = GetComponent<AudioSource>();
        }

        private void FixedUpdate()
        {
            float speedy = shootVelocity * Time.deltaTime;
            transform.Translate(shootDirection * speedy);
        }

        private void OnBecameInvisible()
        {
            DisableShoot();
        }
        private void DisableShoot()
        {
            gameObject.SetActive(false);
            col.enabled = false;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(gameSettings.playerTag) || other.CompareTag(gameSettings.shootTag))
            {
                gameObject.SetActive(false);
                col.enabled = false;
            }
        }
    }
}

