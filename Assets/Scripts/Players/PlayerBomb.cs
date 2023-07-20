using UnityEngine;
using Utils;
namespace RiverAttack
{
    public class PlayerBomb : MonoBehaviour, ICommand
    {
        [SerializeField]
        int bombQuantity;
        [SerializeField]
        Vector3 bombOffset;
        [SerializeField]
        LayerMask layerMask;
        [SerializeField]
        GameObject prefabBomb;
        [SerializeField]
        int idButtonMap;
        PlayerMaster m_PlayerMaster;
        GamePlayManager m_GamePlayManager;
        public int quantityBomb { get { return (int)bombQuantity; } }

        #region UNITY METHODS
        void OnEnable()
        {
            SetInitialReferences();
            m_GamePlayManager.EventCollectItem += UpdateBombs;
        }
        void OnDisable()
        {
            m_GamePlayManager.EventCollectItem -= UpdateBombs;
        }
  #endregion
        void UpdateBombs(CollectibleScriptable collectibles)
        {
            if (bombQuantity <= collectibles.maxCollectible)
            {
                bombQuantity += collectibles.amountCollectables;
            }
        }
        public void AddBomb(int ammoAmount)
        {
            bombQuantity += ammoAmount;
            m_PlayerMaster.CallEventPlayerBomb();
        }

        void SetInitialReferences()
        {
            m_PlayerMaster = GetComponent<PlayerMaster>();
            m_GamePlayManager = GamePlayManager.instance;
            prefabBomb.SetActive(false);
        }
        public void Execute()
        {
            if (bombQuantity <= 0 || !m_GamePlayManager.shouldBePlayingGame || !m_PlayerMaster.ShouldPlayerBeReady())
                return;
            bombQuantity -= 1;
            var bomb = Instantiate(prefabBomb);
            bomb.transform.localPosition = transform.localPosition + bombOffset;
            bomb.SetActive(true);
            m_PlayerMaster.CallEventPlayerBomb();
            LogBomb(1);
        }
        public void UnExecute()
        {
            throw new System.NotImplementedException();
        }
        static void LogBomb(int bomb)
        {
            GamePlaySettings.instance.bombSpent += Mathf.Abs(bomb);
        }
    }
}
