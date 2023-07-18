using UnityEngine;

public abstract class ObstacleSkins : MonoBehaviour
{

    [SerializeField]
    protected int indexStartSkin;
    [SerializeField]
    protected bool randomSkin;
    [SerializeField]
    protected GameObject[] enemySkins;
    protected GameObject obstacleSkin;

    protected Collider myCollider;

    public int IndexSkin { get { return indexStartSkin; } set { indexStartSkin = value; } }
    public bool RandomSkin { get { return randomSkin; } set { randomSkin = value; } }
    public GameObject[] EnemySkins { get { return enemySkins; } set { enemySkins = value; } }

    private void OnEnable()
    {
        LoadDefaultSkin();
    }

    private void LoadDefaultSkin()
    {
        myCollider = GetComponent<Collider>();
        if (enemySkins != null)
        {
            if (randomSkin)
                indexStartSkin = UnityEngine.Random.Range(0, enemySkins.Length);
            if (enemySkins[indexStartSkin] != obstacleSkin)
            {
                // precisa mudar
                obstacleSkin = enemySkins[indexStartSkin];
                if (transform.GetChild(0))
                    DestroyImmediate(transform.GetChild(0).gameObject);
                GameObject go = Instantiate(obstacleSkin, transform);
                go.transform.SetAsFirstSibling();
                if ((myCollider && go.GetComponentInChildren<Collider>()) && (myCollider != go.GetComponentInChildren<Collider>()))
                {
                    Utils.Tools.CopyComponent<Collider>(go.GetComponentInChildren<Collider>(), gameObject);
                }
            }
        }
    }
}
