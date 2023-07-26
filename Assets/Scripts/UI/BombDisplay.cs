using UnityEngine;
using TMPro;

namespace RiverAttack 
{
    public class BombDisplay : MonoBehaviour
    {
        [SerializeField] PlayerSettings playerSettings;
        [SerializeField] TMP_Text bombAmoutText;

        void Update()
        {
            bombAmoutText.text = "X " + playerSettings.bombs.ToString();
        }
    }
}

