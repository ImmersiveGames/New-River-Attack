using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.ObstaclesSystems.ObjectsScriptables;
using NewRiverAttack.PlayerManagers.PlayerSystems;
using NewRiverAttack.PlayerManagers.Tags;
using UnityEngine;
using Random = UnityEngine.Random;

namespace NewRiverAttack.ObstaclesSystems.Abstracts
{
    public abstract class ObstacleSkin : MonoBehaviour
    {
        public int forceSkinIndex = -1;
        protected ObstacleMaster ObstacleMaster;
        private GamePlayManager _gamePlayManager;
        private GameObject _skin;

        #region Unity Methods

        protected virtual void OnEnable()
        {
            RemoveSkin();
            SetInitialReferences();
            ObstacleMaster.EventObstacleDeath += DesativeSkin;
            _gamePlayManager.EventGameRestart += RestoreSkin;
        }

        private void Start()
        {
            ChangePlayerSkin(ObstacleMaster.objectDefault, ObstacleMaster.objectDefault.randomSkin);
        }

        protected virtual void OnDisable()
        {
            ObstacleMaster.EventObstacleDeath -= DesativeSkin;
            _gamePlayManager.EventGameRestart -= RestoreSkin;
        }

        #endregion

        private void ChangePlayerSkin(ObjectsScriptable enemy, bool random)
        {
            if (enemy == null || enemy.defaultPrefabSkin.Length <= 0) return;
            var indexSkin = 0;
            if (random && enemy.defaultPrefabSkin.Length > 0)
            {
                indexSkin = Random.Range(0, enemy.defaultPrefabSkin.Length);
            }

            if (forceSkinIndex >= 0)
            {
                indexSkin = forceSkinIndex;
            }

            var skin = enemy.defaultPrefabSkin[indexSkin].gameObject;

            RemoveSkin();
            CreateSkin(skin);
            ObstacleMaster.OnObstacleChangeSkin();
        }

        private void RemoveSkin()
        {
            var children = GetComponentInChildren<SkinAttach>();
            if (children != true) return;
            var siblingIndex = children.transform.GetSiblingIndex();
            Destroy(transform.GetChild(siblingIndex).gameObject);
        }

        private void CreateSkin(GameObject productSkin)
        {
            _skin = Instantiate(productSkin, transform);
            _skin.transform.SetAsFirstSibling();
        }

        protected virtual void SetInitialReferences()
        {
            ObstacleMaster = GetComponent<ObstacleMaster>();
            _gamePlayManager = GamePlayManager.instance;
        }

        protected virtual void DesativeSkin(PlayerMaster playerMaster)
        {
            if (!ObstacleMaster.objectDefault.canKilled) return;
            DesativeSkin();
        }
        protected void DesativeSkin()
        {
            _skin.SetActive(false);
        }
        private void RestoreSkin()
        {
            if (!ObstacleMaster.objectDefault.canRespawn) return;
            _skin.SetActive(true);
        }
    }
}