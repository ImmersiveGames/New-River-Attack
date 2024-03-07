using ImmersiveGames.Panels.Interfaces;
using UnityEngine;
using UnityEngine.Events;

namespace ImmersiveGames.Panels
{
    public abstract class PanelBase : MonoBehaviour, IMenuBase
    {
        #region Events
        public UnityEvent onMenuShown;
        public UnityEvent onMenuHidden;
        public UnityEvent<int> onMenuIndexChanged;
        #endregion

        #region Attributes
        [Header("Settings")]
        [SerializeField] private bool startActive = true;
        [SerializeField] private int initialMenuIndex;

        [Header("Menus")]
        [SerializeField] private GameObject[] mainMenu;

        private PanelBase _previousMenu;
        #endregion

        #region Unity Methods
        protected virtual void Awake()
        {
            // Open the initial menu
            if (startActive) Open();
        }
        #endregion

        #region Public Methods (from IMenuBase interface)

        // ... (métodos da interface IMenuBase)

        public abstract bool isOpen { get; set; }
        public abstract bool isSelectable { get; set; }
        public abstract bool isNavigable { get; set; }

        public void Open()
        {
            if (isOpen)
            {
                Debug.LogWarning("Open: Menu is already open.");
                return;
            }

            if (MenuManager.instance == null)
            {
                Debug.LogError("Open: MenuManager instance is null.");
                return;
            }

            Interact();
            // Save the current menu as the previous menu
            _previousMenu = MenuManager.instance.GetCurrentSelectedMenu();

            SetInternalMenu(initialMenuIndex);
            onMenuShown?.Invoke();
        }

        public abstract void Close();
        public abstract void OnSelect();
        public abstract void OnDeselect();
        public abstract void OnNavigateUp();
        public abstract void OnNavigateDown();

        // ... (outros métodos da interface IMenuBase)

        #endregion

        #region Public Methods (Specific to Menu Navigation)
        public void GoBack()
        {
            if (_previousMenu != null)
            {
                _previousMenu.Open();
            }
            else
            {
                Debug.LogWarning("GoBack: Previous menu is null.");
            }
        }
        #endregion

        #region Private Methods
        private void SetActive(bool isActive)
        {
            if (mainMenu != null)
            {
                foreach (var menu in mainMenu)
                {
                    menu.SetActive(isActive);
                }
            }

            isOpen = isActive;
        }

        private void SetInternalMenu(int index)
        {
            if (index >= 0 && index < mainMenu.Length)
            {
                SetActive(true);

                for (var i = 0; i < mainMenu.Length; i++)
                {
                    var isActive = i == index;
                    mainMenu[i].SetActive(isActive);

                    if (isActive)
                    {
                        SetSelectGameObject(mainMenu[i]);
                    }
                }

                onMenuIndexChanged?.Invoke(index);
            }
            else
            {
                Debug.LogWarning($"Attempt to set internal menu with invalid index: {index}");
            }
        }
        public void Interact()
        {
            if (MenuManager.instance != null)
            {
                MenuManager.instance.InteractWithMenu(this);
            }
        }
        private static void SetSelectGameObject(GameObject goButton)
        {
            var eventSystemFirstSelect = goButton.GetComponent<SystemEventFirstSelect>();
            if (eventSystemFirstSelect != null)
            {
                eventSystemFirstSelect.Init();
            }
        }
        #endregion

        // Implement abstract methods Select, Deselect, NavigateUp, and NavigateDown according to your menu's specific logic.
    }
}
