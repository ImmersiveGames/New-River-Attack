using System.Collections.Generic;
using UnityEngine;
using MyMenus;

namespace RiverAttack
{
    public class PanelManager : MonoBehaviour
    {
        GameSettings m_GameSettings;
        GameManager m_GameManager;
        [SerializeField] List<Transform> panelsMenus;
        [SerializeField] bool firstMenuStartEnable = true;
        [SerializeField] PanelBackButton panelBackButton;
        [SerializeField] List<int> navegationMenu;
        [SerializeField] int menuIndexActive;

        void Awake()
        {
            ClearMenu();
            if (!firstMenuStartEnable) return;
            panelsMenus[0].gameObject.SetActive(true);
            navegationMenu.Add(0);
        }
        void Start()
        {
            m_GameSettings = GameSettings.instance;
            m_GameManager = GameManager.instance;
        }

        void ClearMenu()
        {
            foreach (var child in panelsMenus)
            {
                child.gameObject.SetActive(false);
            }
        }

        void ActiveMenuIndex(int index)
        {
            if (index >= 0 && index < transform.childCount)
            {
                transform.GetChild(index).gameObject.SetActive(true);
            }
            else
            {
                Debug.LogWarning("Invalid child object index!");
            }
        }

        #region ButtonActions
        public void ButtonQuitApplication()
        {
            Application.Quit();
        }
        public void ButtonStartGame()
        {
            ClearMenu();
            m_GameManager.ChangeStatesGamePlay(GameManager.States.WaitGamePlay);
        }
  #endregion
    }
}
