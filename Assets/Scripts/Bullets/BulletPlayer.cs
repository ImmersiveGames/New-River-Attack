using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RiverAttack
{
    public class BulletPlayer : MonoBehaviour
    {
         #region Variable Private Inspector
    [SerializeField] AudioEventSample audioShoot;
    [SerializeField] float shootVelocity = 10f;
    [SerializeField] bool bulletLifeTime = false;
    [SerializeField] float lifeTime = 2f;
    private float startTime;
    public PlayerMaster OwnerShoot { get; set; }
    public Transform myPool { get; set; }
    #endregion

    private void OnEnable()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        audioShoot.Play(audioSource);
    }
    /// <summary>
    /// Executa quando o objeto esta ativo
    /// </summary>
    /// 
    private void Start()
    {
        startTime = Time.time + lifeTime;
    }

    public void SetSpeedShoot(double speedy)
    {
        shootVelocity = (float)speedy;
    }
    /// <summary>
    /// Executa a cada atualização de frame da fisica
    /// </summary>
    /// 
    void FixedUpdate()
    {
        MoveShoot();
        AutoDestroy();
    }
    /// <summary>
    /// Proper this object forward
    /// </summary>
    /// 
    private void MoveShoot()
    {
        if (GamePlayManager.instance.shouldBePlayingGame)
        {
            float speedy = shootVelocity * Time.deltaTime;
            transform.Translate(Vector3.forward * speedy);
        }
        else
        {
            DestroyMe();
        }
        
    }
    private void OnTriggerEnter(Collider collision)
    {
        if (!collision.transform.root.CompareTag(GameSettings.instance.playerTag))
        {
            if (collision.transform.root.CompareTag(GameSettings.instance.collectionTag)){
                var collectibles = (CollectibleScriptable)collision.transform.root.GetComponent<EnemiesMaster>().enemy;
                //if (collectibles.PowerUp != null) return;
            } 
            DestroyMe();
        }
    }
    /// <summary>
    /// If OnLifeTime set, auto destroyer this object
    /// </summary>
    /// 
    private void AutoDestroy()
    {
        if (bulletLifeTime && Time.time >= startTime)
        {
            DestroyMe();
        }
    }
    /// <summary>
    /// Atalho para destruir este objeto
    /// </summary>
    private void DestroyMe()
    {
        //Destroy(this.gameObject);
        
        gameObject.SetActive(false);
        gameObject.transform.SetParent(myPool);
        gameObject.transform.SetAsLastSibling();
    }
    /// <summary>
    /// Se o objeto sai da tela ele é destruido
    /// </summary>
    private void OnBecameInvisible()
    {
        Invoke("DestroyMe", .1f);
    }
    }
}


