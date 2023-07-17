using UnityEngine;
namespace RiverAttack
{
    
[RequireComponent(typeof(EnemiesMaster))]
public sealed class EnemiesDifficulty : MonoBehaviour {

    EnemiesMaster m_EnemyMaster;
    private DifficultyList m_EnemiesDifficulty;
    public EnemySetDifficulty myDifficulty { get; private set; }

    private void Awake()
    {
        if (m_EnemiesDifficulty != null)
            myDifficulty = m_EnemiesDifficulty.difficultiesList[0];
    }

    void OnEnable()
    {
        SetInitialReferences();
        //ChangeDifficulty();
        
    }

    private void Start()
    {
        ChangeDifficulty();
        m_EnemyMaster.EventDestroyEnemy += ChangeDifficulty;
    }

    void SetInitialReferences()
    {
        m_EnemyMaster = GetComponent<EnemiesMaster>();
        m_EnemiesDifficulty = m_EnemyMaster.enemy.enemiesDifficulty;
        if (m_EnemiesDifficulty != null)
            myDifficulty = m_EnemiesDifficulty.difficultiesList[0];
    }

    void ChangeDifficulty()
    {
        if (m_EnemiesDifficulty != null)
        {
            myDifficulty = GetDifficult((int)(GamePlayManager.instance.HightScorePlayers()));
        }
    }
    public EnemySetDifficulty GetDifficult(string difficultName)
    {
        return m_EnemiesDifficulty.difficultiesList.Find(x => x.name == difficultName);
    }
    EnemySetDifficulty GetDifficult(int score)
    {
        return m_EnemiesDifficulty.difficultiesList.Find(x => x.scoreToChange >= (score));
    }
    public DifficultyList GetDifficultList()
    {
        return m_EnemiesDifficulty;
    }

    void OnDisable()
    {
        m_EnemyMaster.EventDestroyEnemy -= ChangeDifficulty;
    }
}
}
