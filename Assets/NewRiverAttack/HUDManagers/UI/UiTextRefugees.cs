using NewRiverAttack.GamePlayManagers;
using NewRiverAttack.SaveManagers;
using TMPro;
using UnityEngine;

namespace NewRiverAttack.HUDManagers.UI
{
    public class UiTextRefugees : MonoBehaviour
    {
        private GamePlayManager _gamePlayManager;
        private TMP_Text _textRefugees;
        private Animator _animator;
        private static readonly int RefugeesBounce = Animator.StringToHash("Bounce");

        #region UNITYMETHODS

        private void OnEnable()
        {
            SetInitialReferences();
            _gamePlayManager.EventHudRefugiesUpdate += UpdateRefugees;
        }

        private void Start()
        {
            var refugees = GameOptionsSave.instance.wallet;
            UpdateRefugees(refugees, 0);
        }

        private void OnDisable()
        {
            _gamePlayManager.EventHudRefugiesUpdate -= UpdateRefugees;
        }

        #endregion

        private void SetInitialReferences()
        {
            _gamePlayManager = GamePlayManager.instance;
            _textRefugees = GetComponent<TMP_Text>();
            _animator = GetComponent<Animator>();
        }

        private void UpdateRefugees(int refugee, int playerIndex)
        {
            _animator.SetTrigger(RefugeesBounce);
            _textRefugees.text = refugee.ToString();
        }
    }
}