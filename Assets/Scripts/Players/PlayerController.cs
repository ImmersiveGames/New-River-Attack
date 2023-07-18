using UnityEngine;
using Utils;

namespace RiverAttack
{
    public class PlayerController : MonoBehaviour
    {
        PlayersInputActions m_PlayersInputActions;

#region UNITYMETHODS
        void Awake()
        {
            m_PlayersInputActions = new PlayersInputActions();
            m_PlayersInputActions.Enable();
        }
    #endregion
        /*
         * Pega o valor do input, normaliza e retorna um vetor para ser usado com o transform do objeto
         */
        public Vector2 GetMovementVector2Normalized()
        {
            var inputVector = m_PlayersInputActions.Player.Move.ReadValue<Vector2>();
            inputVector = inputVector.normalized;

            return inputVector;
        }
    }
}
