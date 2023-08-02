using UnityEngine;
using TMPro;

namespace RiverAttack 
{
    public class BombDisplay : MonoBehaviour
    {
        [SerializeField] PlayerSettings playerSettings;
        [SerializeField] TMP_Text textBombAmount;
        

        void Update()
        {
            textBombAmount.text = "X " + playerSettings.bombs.ToString();
        }
    }
}

