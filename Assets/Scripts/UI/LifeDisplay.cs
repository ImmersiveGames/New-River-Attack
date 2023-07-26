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

        private int lives;
        private GamePlayManager gamePlay;
        private PlayerMaster playerMaster;
        private void Start()
        {
            SetInitialReferences();
            CreateLiveIcon(this.transform, lives);
            playerMaster.EventPlayerMasterReSpawn += UpdateUI;
            playerMaster.EventPlayerMasterOnDestroy += UpdateUI;
            playerMaster.EventPlayerAddLive += UpdateUI;
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

        private void SetLivesUI()
        {
            int i = this.transform.childCount;

            if (i < lives)
            {
                CreateLiveIcon(this.transform, lives - i);
            }
            for (int x = 0; x < i; x++)
            {
                if (x < lives)
                    this.transform.GetChild(x).gameObject.SetActive(true);
                else
                    this.transform.GetChild(x).gameObject.SetActive(false);
            }
        }
        private void CreateLiveIcon(Transform parent, int quant)
        {
            for (int x = 0; x < quant; x++)
            {
                GameObject icon = Instantiate(iconLives, parent);
                icon.GetComponent<Image>().sprite = playerMaster.GetPlayersSettings().playerSkin.hubSprite;
            }
        }
    }

}
