using UnityEngine;
namespace RiverAttack
{
    public class PlayerMaster : MonoBehaviour
    {
    #region SerilizedField
        [SerializeField] float autoMovement = .5f;
        [SerializeField] float movementSpeed = 8f;
        [SerializeField] bool isMoving = false;
        [SerializeField] float autoMovementUp = 2f;
        [SerializeField] float autoMovementDown = .1f;
  #endregion

        [SerializeField] GameInputs gameInputs;

        GameManager m_GameManager;

    #region UNITYMETHODS
        // Start is called before the first frame update
        void Start()
        {
            //START
        }

        // Update is called once per frame
        void Update()
        {
            if (GameManager.instance.GetStates() != GameManager.States.GamePlay || GameManager.instance.GetPaused())
            {
                isMoving = false;
                return;
            }
            isMoving = true;
            var inputVector = gameInputs.GetMovementVector2Normalized();
            float axisAutoMovement = inputVector.y switch
            {
                > 0 => autoMovementUp,
                < 0 => autoMovementDown,
                _ => autoMovement
            };

            var moveDir = new Vector3(inputVector.x, 0, axisAutoMovement);
            if (isMoving)
                transform.position += moveDir * (movementSpeed * Time.deltaTime);
        }
  #endregion
        public void AllowedMove(bool allowed = true)
        {
            isMoving = allowed;
        }
    }
}