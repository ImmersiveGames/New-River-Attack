using UnityEngine;
using UnityEngine.UI;

namespace RiverAttack
{
    public class LifeDisplay : MonoBehaviour
    {
        [SerializeField]
        private int playerIndex;
        [SerializeField]
        private GameObject iconLives;
        [SerializeField]
        private Sprite defaultSprite;
        [SerializeField]
        private Transform livesParent;

        private int lives;
        private GamePlayManager gamePlay;
        private PlayerMaster playerMaster;
        private void OnEnable()
        {
            //Debug.Log("Habilitei");
            SetInitialReferences();
            SetLivesUI();
            playerMaster.EventPlayerMasterReSpawn += UpdateUI;
            playerMaster.EventPlayerMasterOnDestroy += UpdateUI;
            playerMaster.EventPlayerAddLive += UpdateUI;
            playerMaster.EventChangeSkin += UpdateUI;
        }

        private void SetInitialReferences()
        {
            gamePlay = GamePlayManager.instance;
            playerMaster = gamePlay.GetPlayerMasterByIndex(playerIndex);            
            lives = (int)playerMaster.GetPlayersSettings().lives;         
        }

        private void OnDisable()
        {
            playerMaster.EventPlayerMasterOnDestroy -= UpdateUI;
            playerMaster.EventPlayerMasterReSpawn -= UpdateUI;
            playerMaster.EventPlayerAddLive -= UpdateUI;
        }

        private void UpdateUI()
        {
            lives = (int)playerMaster.GetPlayersSettings().lives;
            Invoke("SetLivesUI", .1f);
        }

        private void UpdateUI(ShopProductSkin skin) 
        {
            ClearLiveIcon(livesParent);
            SetLivesUI();
        }

        private void SetLivesUI()
        {           

            //Debug.Log("Vou tentar Criar os icones");
            int i = livesParent.childCount;

            if (i < lives)
            {
                //Debug.Log("Chamando a criação dos icones");
                CreateLiveIcon(livesParent, lives - i);
            }
            for (int x = 0; x < i; x++)
            {
                if (x < lives)
                    livesParent.GetChild(x).gameObject.SetActive(true);
                else
                    livesParent.GetChild(x).gameObject.SetActive(false);
            }
        }
        private void CreateLiveIcon(Transform parent, int quant)
        {
            for (int x = 0; x < quant; x++)
            {
                GameObject icon = Instantiate(iconLives, parent);
                icon.GetComponent<Image>().sprite = playerMaster.GetPlayersSettings().playerSkin.hubSprite;
                //Debug.Log("icone criado");
            }

        }

        private void ClearLiveIcon(Transform parent)
        {
            //Debug.Log("Clear Skin");
            for (int x = 0; x < parent.childCount; x++)
            {
                Destroy(parent.GetChild(x).gameObject);
                //Debug.Log("Skin Destroyed");
            }
        }
    }

}
